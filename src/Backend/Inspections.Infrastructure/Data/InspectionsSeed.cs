using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOSoft.HelpDesk.Infrastructure.Data
{
    public class InspectionsSeed
    {
        public static async Task SeedAsync(InspectionsContext context, ILoggerFactory logger
            , int? retriesNumber = 0)
        {

            if(!context.Database.IsInMemory())
                context.Database.Migrate();

            var log = logger.CreateLogger<InspectionsSeed>();
            int retries = retriesNumber.Value;
            try
            {
                log.LogInformation($"Initializing data {nameof(InspectionsContext)}");
                //TODO: Load Default Data
            }
            catch (Exception ex)
            {
                if (retries < retriesNumber.Value)
                {
                    retries++;

                    log.LogError(ex.Message);
                    log.LogInformation($"Retry Nro {retries}");
                    await SeedAsync(context, logger);
                }
            }
        }
    }
}
