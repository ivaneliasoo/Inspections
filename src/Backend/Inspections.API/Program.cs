using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Inspections.Infrastructure.Data;
using IOSoft.HelpDesk.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Inspections.API.Features.EnergyReport.Models;

namespace Inspections.API
{
    public class Program
    {
        private const string DATABASE_ERROR_MESSAGE = "can't seed, error connecting to database";

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                //services.GetRequiredService<InspectionsContext>();

                try
                {
                    services.GetRequiredService<EnergyReportContext>();
                    var inspectionsContext = services.GetRequiredService<InspectionsContext>();
                    //InspectionsSeed.SeedAsync(inspectionsContext, loggerFactory).Wait();
                }
                catch (DbException ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, DATABASE_ERROR_MESSAGE);
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
#endif
                    .UseStartup<Startup>();
                });
    }
}
