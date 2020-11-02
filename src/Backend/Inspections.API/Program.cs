﻿using System;
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

namespace Inspections.API
{
    public class Program
    {
        private const string DATABASE_ERROR_MESSAGE = "ocurrio un error cargando datos a la base de datos";

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                services.GetRequiredService<InspectionsContext>();

                try
                {
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
                    webBuilder.UseStartup<Startup>();
                });
    }
}
