using Inspections.Core.Domain;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
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

            if (!context.Database.IsInMemory())
                context.Database.Migrate();

            // TODO OJOOOOO: remove when ready for production
            //context.Database.EnsureDeleted(); //this was killing my azure invoice xD
            context.Database.Migrate();

            var log = logger.CreateLogger<InspectionsSeed>();
            int retries = retriesNumber.Value;
            try
            {
                log.LogInformation($"Initializing data {nameof(InspectionsContext)}");
                //TODO: Load Default Data

                if (!context.Users.Any())
                {
                    context.Users.Add(new User() { UserName = "demo", Password = "demo", Name = "demo", LastName = "user" });
                }

                if (!context.ReportConfigurations.Any())
                {
                    context.Add(new ReportConfiguration()
                    {
                        ChecksDefinition = AddCheckLists(context).ToList(),
                        SignatureDefinitions = AddSignatures(context).ToList(),
                        FormName = "Inspection Report",
                        Title = "Inspection Report",
                        Type = ReportType.Inspection
                    });
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (retries < retriesNumber.Value)
                {
                    retries++;

                    log.LogError(ex.Message);
                    log.LogInformation($"Retries {retries}");
                    await SeedAsync(context, logger);
                }
            }
        }

        private static IEnumerable<Signature> AddSignatures(InspectionsContext context)
        {
            return new Signature[] {new Signature
            {
                Title = "THE INSPECTION WAS WITNESSED BY REPRESENTATIVE OF THE LICENSEE",
                IsConfiguration = true,
                Annotation = ""
            },
                new Signature
                {
                    Title = "DECLARATION BY LEW",
                    IsConfiguration = true,
                    Annotation = "I am satisfied that the electrical installation is fit for operation."
                },
                new Signature
                {
                    Title = "Report Signature",
                    IsConfiguration = true,
                    Annotation = "",
                    Principal = true
                } };
        }

        private static IEnumerable<CheckList> AddCheckLists(InspectionsContext context)
        {
                var item1 = new CheckList(
                       "SITE CONDITIONS",
                       null,
                       "[√: acceptable / X: not acceptable / NA: not applicable ]",
                       true
                    );
                item1.AddCheckItems(new CheckListItem(0, "Access to MSB / Sub-Board / Distribution-Board",
                    CheckValue.False, true, string.Empty, null));

                var item2 = new CheckList(
                       "ENVIRONMENT CONDITIONS",
                       null,
                       "",
                       true
                    );
                item2.AddCheckItems(new CheckListItem(0, "Locking facilities",
                    CheckValue.False, true, string.Empty, null));
                item2.AddCheckItems(new CheckListItem(0, "Roofing or shed condition (for outdoor)",
                    CheckValue.False, true, string.Empty, null));
                item2.AddCheckItems(new CheckListItem(0, "Lighting",
                    CheckValue.False, true, string.Empty, null));
                item2.AddCheckItems(new CheckListItem(0, "Ventilation",
                    CheckValue.False, true, string.Empty, null));
                item2.AddCheckItems(new CheckListItem(0, "Fire extinguisher",
                    CheckValue.False, true, string.Empty, null));
                item2.AddCheckItems(new CheckListItem(0, "No sign of bird/pest habitation",
                    CheckValue.False, true, string.Empty, null));

                var item3 = new CheckList(
                       "SWITCHROOM REQUIREMENTS",
                       null,
                       "",
                       true
                    );
                item3.AddCheckItems(new CheckListItem(0, "Rubber mat",
                    CheckValue.False, true, string.Empty, null));
                item3.AddCheckItems(new CheckListItem(0, "Single-line diagram",
                    CheckValue.False, true, string.Empty, null));
                item3.AddCheckItems(new CheckListItem(0, "LEW’s contact particulars",
                    CheckValue.False, true, string.Empty, null));
                item3.AddCheckItems(new CheckListItem(0, "First-aid chart",
                    CheckValue.False, true, string.Empty, null));
                item3.AddCheckItems(new CheckListItem(0, "Display of Electrical Installation license",
                    CheckValue.False, true, string.Empty, null));
                item3.AddCheckItems(new CheckListItem(0, "Danger sign",
                    CheckValue.False, true, string.Empty, null));
            return new CheckList[] { item1, item2, item3 };
        }
    }
}
