﻿using Inspections.Core.Domain;
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
            {
                context.Database.Migrate();
            }

            var log = logger.CreateLogger<InspectionsSeed>();
            int retries = retriesNumber.Value;
            try
            {
                log.LogInformation($"Initializing data {nameof(InspectionsContext)}");

                if (!context.Users.Any())
                {
                    context.Users.Add(new User() { UserName = "demo", Password = "demo", Name = "demo", LastName = "user", IsAdmin = true });
                }

                if (!context.ReportConfigurations.Any())
                {
                    context.Add(new ReportConfiguration()
                    {
                        ChecksDefinition = AddCheckLists().ToList(),
                        SignatureDefinitions = AddSignatures().ToList(),
                        FormName = "CSE EI(R1) FORM",
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

        private static IEnumerable<Signature> AddSignatures()
        {
            return new Signature[] {
                new Signature
                {
                    Title = "Report Signature",
                    IsConfiguration = true,
                    Annotation = "",
                    Principal = true
                },
             new Signature
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
                }};
        }

        private static IEnumerable<CheckList> AddCheckLists()
        {
            var item1 = new CheckList(
                   "SITE CONDITIONS",
                   null,
                   "[√: acceptable / blank(unchecked): not acceptable / Dash(-): not applicable ]",
                   true
                );
            item1.AddCheckItems(new CheckListItem(0, "Access to MSB / Sub-Board / Distribution-Board",
                CheckValue.False, false, true, string.Empty, null));

            var item2 = new CheckList(
                   "ENVIRONMENT CONDITIONS",
                   null,
                   "",
                   true
                );
            item2.AddCheckItems(new CheckListItem(0, "Locking facilities",
                CheckValue.False, false, true, string.Empty, null));
            item2.AddCheckItems(new CheckListItem(0, "Roofing or shed condition (for outdoor)",
                CheckValue.False, false, true, string.Empty, null));
            item2.AddCheckItems(new CheckListItem(0, "Lighting",
                CheckValue.False, false, true, string.Empty, null));
            item2.AddCheckItems(new CheckListItem(0, "Ventilation",
                CheckValue.False, false, true, string.Empty, null));
            item2.AddCheckItems(new CheckListItem(0, "Fire extinguisher",
                CheckValue.False, false, true, string.Empty, null));
            item2.AddCheckItems(new CheckListItem(0, "No sign of bird/pest habitation",
                CheckValue.False, false, true, string.Empty, null));

            var item3 = new CheckList(
                   "SWITCHROOM REQUIREMENTS",
                   null,
                   "",
                   true
                );
            item3.AddCheckItems(new CheckListItem(0, "Rubber mat",
                CheckValue.False, false, true, string.Empty, null));
            item3.AddCheckItems(new CheckListItem(0, "Single-line diagram",
                CheckValue.False, false, true, string.Empty, null));
            item3.AddCheckItems(new CheckListItem(0, "LEW’s contact particulars",
                CheckValue.False, false, true, string.Empty, null));
            item3.AddCheckItems(new CheckListItem(0, "First-aid chart",
                CheckValue.False, false, true, string.Empty, null));
            item3.AddCheckItems(new CheckListItem(0, "Display of Electrical Installation license",
                CheckValue.False, false, true, string.Empty, null));
            item3.AddCheckItems(new CheckListItem(0, "Danger sign",
                CheckValue.False, false, true, string.Empty, null));

            var item4 = new CheckList(
                       "SWITCHBOARD REQUIREMENTS",
                       null,
                       "",
                       true
                    );
            item4.AddCheckItems(new CheckListItem(0, "Earth bar c/w proper labeling",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Earth pits condition",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Earth pits (R = ___ Ω tested on ___)",
                CheckValue.False, true, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Earth electrode inspection chamber",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Warning notice",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Equipotential bonding of metallic trunking, metal conduits & water / gas pipe.",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Protection of fingers to direct live parts (at least IP2X)",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Incoming & outgoing lights, voltmeter & Ammeter",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Proper Neutral links sizing",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Appropriate circuit breakers size & type",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Sufficient support & mechanical protection for cables",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Labelling of circuits",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Sign of Corrosion",
                CheckValue.False, false, true, string.Empty, null));
            item4.AddCheckItems(new CheckListItem(0, "Servicing of Switchboard (Last Done: ___)",
                CheckValue.False, true, true, string.Empty, null));


            var item5 = new CheckList(
                       "OPERATIONAL REQUIREMENTS",
                       null,
                       "",
                       true
                    );
            item5.AddCheckItems(new CheckListItem(0, "Incoming load         L1: ___A      L2:___A      L3:___A",
                CheckValue.False, true, true, string.Empty, null));
            item5.AddCheckItems(new CheckListItem(0, "Voltage  L1-N:___V   L2-N:___V   L3-N:___V   L-L:___V",
                CheckValue.False, true, true, string.Empty, null));
            item5.AddCheckItems(new CheckListItem(0, "Earth loop impedance (* power disruption)     L-E:___Ω",
                CheckValue.False, true, true, string.Empty, null));
            item5.AddCheckItems(new CheckListItem(0, "Main Breakers’ relay settings   Itrip:___A   t:___s",
                CheckValue.False, true, true, string.Empty, null));
            item5.AddCheckItems(new CheckListItem(0, "Function test of RCD (* power disruption)",
                CheckValue.False, false, true, string.Empty, null));

            var item6 = new CheckList(
                       "OUTGOING DB / CIRCUITS",
                       null,
                       "",
                       true
                    );
            item6.AddCheckItems(new CheckListItem(0, "Appropriate rated fittings/fixtures (i.e. outdoor IP rating, fire rated, explosion proof etc) ",
                CheckValue.False, false, true, string.Empty, null));
            item6.AddCheckItems(new CheckListItem(0, "30mA RCCBs in sensitive areas or public areas",
                CheckValue.False, false, true, string.Empty, null));
            item6.AddCheckItems(new CheckListItem(0, "Standby Generator",
                CheckValue.False, false, true, string.Empty, null));
            item6.AddCheckItems(new CheckListItem(0, "PV system / electric charger system",
                CheckValue.False, false, true, string.Empty, null));
            item6.AddCheckItems(new CheckListItem(0, "No unused wires/cables or illegal wiring",
                CheckValue.False, false, true, string.Empty, null));

            return new CheckList[] { item1, item2, item3, item4, item5, item6 };
        }
    }
}
