using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Users.Services;
using Inspections.API.Models.Configuration;
using Inspections.Core;
using Inspections.Core.Interfaces;
using Inspections.Core.Interfaces.Queries;
using Inspections.Infrastructure.Data;
using Inspections.Infrastructure.Queries;
using Inspections.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ZadERP.Api.Middleware;

namespace Inspections.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            
            var assembly = AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(assembly);

            services.AddAWSService<IAmazonS3>();

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddCors();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"]))
                };
                options.SaveToken = true;
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        // Add the access_token as a claim, as we may actually need it
                        if (context.SecurityToken is JwtSecurityToken accessToken)
                        {
                            if (!ValidUserToken(context, services))
                            {
                                context.Response.StatusCode = 401;
                                context.Fail(new Exception("Error.", new Exception("Invalid Token.")));
                            }
                        }

                        return Task.CompletedTask;
                    },

                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.Value!.Contains("-hub", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inspections API", Version = "v1" });
                c.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme()
                {
                    Description = "Add Token in Headers",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "jwt_auth"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserNameResolver, UserNameResolver>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(typeof(PhotoRecordManager));
            services.AddSingleton<StorageDriver, S3StorageDriver>();

            services.AddScoped<ICheckListsRepository, CheckListsRepository>();
            services.AddScoped<IReportConfigurationsRepository, ReportsConfigurationsRepository>();
            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<ISignaturesRepository, SignaturesRepository>();

            services.AddScoped<ICheckListsQueries, CheckListsQueries>();
            services.AddScoped<ISignaturesQueries, SignaturesQueries>();
            services.AddScoped<IReportConfigurationsQueries, ReportConfigurationsQueries>();

            ConfigurarDbContextInSqlDb(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), Configuration.GetValue<string>("ClientSettings:ReportsImagesFolder"))),
                RequestPath = "/ReportsImages",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                }
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inspections V1");
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();


            app.UseExceptionsMiddleware();

            app.UseCors(builder =>
            {
                builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
            });


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigurarDbContextInSqlDb(IServiceCollection services)
        {
            var logger = LoggerFactory.Create(c => c.AddConsole());
            string cn = Configuration.GetConnectionString("Inspections");
            services.AddDbContext<InspectionsContext>(c =>
            c.UseLoggerFactory(logger).UseNpgsql(cn).EnableSensitiveDataLogging());
        }

        private static bool ValidUserToken(TokenValidatedContext context, IServiceCollection services)

        {
            bool result = false;

            var _userName = context.Principal?.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(_userName))
            {
                InspectionsContext usersRepo = services.BuildServiceProvider().GetRequiredService<InspectionsContext>();
                var savedUser = usersRepo.Users.Where(u => u.UserName == _userName).FirstOrDefault();
                if (savedUser != null)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
