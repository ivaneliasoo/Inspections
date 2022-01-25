using System;
using System.Data.Common;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Inspections.API
{
    public class Program
    {
        private const string DatabaseErrorMessage = "can't seed, error connecting to database";

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var inspectionsContext = services.GetRequiredService<InspectionsContext>();
                    InspectionsSeed.SeedAsync(inspectionsContext, loggerFactory).Wait();
                }
                catch (DbException ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, DatabaseErrorMessage);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
#if DEBUG
                    .UseUrls("http://0.0.0.0:5000")
                    //.UseUrls("https://0.0.0.0:443", "http://0.0.0.0:80", "https://0.0.0.0:5001", "http://0.0.0.0:5000")
#endif
                    .UseSerilog((hostContext, loggerConfiguration) =>
                    {
                        loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                    })
                    .UseStartup<Startup>();
                });
    }
}
