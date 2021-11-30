using Inspections.Core.Domain;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOSoft.HelpDesk.Infrastructure.Data
{
    public class InspectionsSeed
    {
        public static async Task SeedAsync(InspectionsContext context, ILoggerFactory logger
            , int retriesNumber = 0)
        {

            if (!context.Database.IsInMemory())
            {
                context.Database.Migrate();
            }

            var log = logger.CreateLogger<InspectionsSeed>();
            int retries = retriesNumber;
            try
            {
                log.LogInformation($"Initializing data {nameof(InspectionsContext)}");

                if (!context.Users.Any(u => u.UserName == "demo"))
                {
                    context.Users.Add(new User() { UserName = "demo", Password = "demo", Name = "demo", LastName = "User", IsAdmin = true });
                }

                if (!context.Users.Any(u => u.UserName == "developer"))
                {
                    context.Users.Add(new User() { UserName = "developer", Password = "developer@@P@sword", Name = "Developer", LastName = "User", IsAdmin = true });
                }


                if (!context.ReportConfigurations.Any(rc =>rc.FormName == "CSE EI(R8) FORM"))
                {
                    context.Add(new ReportConfiguration()
                    {
                        ChecksDefinition = AddCheckLists().ToList(),
                        SignatureDefinitions = AddSignatures().ToList(),
                        FormName = "CSE EI(R8) FORM",
                        Title = "Inspection Report",
                        Type = ReportType.Inspection,
                        Footer = $@"<footer style=""padding-left: 20px; opacity: 0.5; font-size: 3.2em; display: flex;margin: 10px, 10px;flex-direction: column;color: grey;font-family: 'Times New Roman', Times, serif;"">
                                            <div class='' style='font-size: 3.2em; text-align: right;letter-spacing: 2px;'><label class='pageNumber'></label> | Page</div>
                                            <div class='footer'>
                                              <p style='line-height: 3px;font-size: 3.2em;'>FORM E1(CSE INTERNAL) INSPECTION REPORT FOR LICENSING LEW SINGLE USER PREMISE- REV #8
                                              </p><p style='line-height: 3px;font-size: 3.2em;'>ALL RIGHTS RESERVED TO CHENG SENG ELECTRIC CO PTE LTD</p>
                                            </div>
                                          </footer>
                                        ",
                        MarginBottom = "80px",
                        MarginTop = "20px",
                        MarginLeft = "70px",
                        MarginRight = "70px"
                    });
                }

                var templateId = 1;
                if (!context.Template.Any(t => t.id == templateId)) {
                    log.LogInformation("No template found in the database, copying template from file categories.json");

                    var CategoriesFilePath = Path.Combine(AppContext.BaseDirectory, "categories.json");
                    var reader = System.IO.File.OpenText(CategoriesFilePath);
                    var categories = reader.ReadToEnd();

                    Template t = new Template();
                    t.id = templateId;
                    t.templateDef = categories;

                    context.Add(t);
                }

                if (!context.Team.Any()) {
                    var teams = new List<Team> {
                        new Team { foreman = "Kim", position = 1, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "Gan", position = 2, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "Liton", position = 3, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "Lee", position = 4, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "Hin", position = 5, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "Boomi", position = 6, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "Mani", position = 7, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "Hui", position = 8, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "Store", position = 100, teamMembers = "[]", lastUpdate = DateTime.Now },
                        new Team { foreman = "On leave", position = 101, teamMembers = "[]", lastUpdate = DateTime.Now }
                    };
                    context.AddRange(teams);
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (retries < retriesNumber)
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
                    Title = "INSPECTION CARRIED OUT BY",
                    IsConfiguration = true,
                    Annotation = "",
                    Principal = true,
                    Order = 1
                },
             new Signature
            {
                Title = "THE INSPECTION WAS WITNESSED BY REPRESENTATIVE OF THE LICENSEE",
                IsConfiguration = true,
                Annotation = "",
                    Order = 2
            },
                new Signature
                {
                    Title = "DECLARATION BY LEW",
                    IsConfiguration = true,
                    Annotation = "I am satisfied that the electrical installation is fit for operation."
                    ,
                    Order = 3
                }};
        }

        private static IEnumerable<CheckList> AddCheckLists()
        {
            var item1 = new CheckList(
                   "SITE CONDITIONS",
                   "[√: acceptable / blank(unchecked): not acceptable / Dash(-): not applicable ]",
                   true
                );
            item1.AddCheckItems(new CheckListItem(0, "Access to MSB / Sub-Board / Distribution-Board",
                CheckValue.None, false, true, string.Empty));
            item1.AddCheckItems(new CheckListItem(0, "Locking facilities",
                CheckValue.None, false, true, string.Empty));
            item1.AddCheckItems(new CheckListItem(0, "Roofing or shed condition (for outdoor)",
                CheckValue.None, false, true, string.Empty));
            item1.AddCheckItems(new CheckListItem(0, "Lighting",
                CheckValue.None, false, true, string.Empty));
            item1.AddCheckItems(new CheckListItem(0, "Air Ventilation",
                CheckValue.None, false, true, string.Empty));
            item1.AddCheckItems(new CheckListItem(0, "No sign of nird/pest habilitation",
                CheckValue.None, false, true, string.Empty));

            var item3 = new CheckList(
                   "SWITCH-ROOM REQUIREMENTS",
                   "",
                   true
                );
            item3.AddCheckItems(new CheckListItem(0, "Rubber mat",
                CheckValue.None, false, true, string.Empty));
            item3.AddCheckItems(new CheckListItem(0, "Single-line diagram",
                CheckValue.None, false, true, string.Empty));
            item3.AddCheckItems(new CheckListItem(0, "LEW’s contact particulars",
                CheckValue.None, false, true, string.Empty));
            item3.AddCheckItems(new CheckListItem(0, "First-aid chart",
                CheckValue.None, false, true, string.Empty));
            item3.AddCheckItems(new CheckListItem(0, "Display of Electrical Installation license",
                CheckValue.None, false, true, string.Empty));
            item3.AddCheckItems(new CheckListItem(0, "Danger sign",
                CheckValue.None, false, true, string.Empty));

            var item4 = new CheckList(
                       "SWITCH-BOARD REQUIREMENTS",
                       "",
                       true
                    );
            item4.AddCheckItems(new CheckListItem(0, "Earth bar c/w proper labeling",
                CheckValue.None, false, true, string.Empty));
            item4.AddCheckItems(new CheckListItem(0, "Earth pits condition",
                CheckValue.None, false, true, string.Empty));
            item4.AddCheckItems(new CheckListItem(0, "Equipotential bonding of metallic trunking, metal conduits & water / gas pipe.",
                CheckValue.None, false, true, string.Empty));
            item4.AddCheckItems(new CheckListItem(0, "Protection of fingers to direct live parts (at least IP2X)",
                CheckValue.None, false, true, string.Empty));
            item4.AddCheckItems(new CheckListItem(0, "Incoming & outgoing lights, voltmeter & Ammeter",
                CheckValue.None, false, true, string.Empty));
            item4.AddCheckItems(new CheckListItem(0, "Proper Neutral links sizing",
                CheckValue.None, false, true, string.Empty));
            item4.AddCheckItems(new CheckListItem(0, "Sufficient support & mechanical protection for cables",
                CheckValue.None, false, true, string.Empty));
            item4.AddCheckItems(new CheckListItem(0, "Labelling of circuits",
                CheckValue.None, false, true, string.Empty));
            item4.AddCheckItems(new CheckListItem(0, "Sign of Corrosion",
                CheckValue.None, false, true, string.Empty));

            var item6 = new CheckList(
                       "OUTGOING DB / CIRCUITS",
                       "",
                       true
                    );
            item6.AddCheckItems(new CheckListItem(0, "Appropriate rated fittings/fixtures (i.e. outdoor IP rating, fire rated, explosion proof etc) ",
                CheckValue.None, false, true, string.Empty));
            item6.AddCheckItems(new CheckListItem(0, "30mA RCCBs in sensitive areas or public areas",
                CheckValue.None, false, true, string.Empty));
            item6.AddCheckItems(new CheckListItem(0, "Standby Generator",
                CheckValue.None, false, true, string.Empty));
            item6.AddCheckItems(new CheckListItem(0, "PV system / electric charger system",
                CheckValue.None, false, true, string.Empty));
            item6.AddCheckItems(new CheckListItem(0, "No unused wires/cables or illegal wiring",
                CheckValue.None, false, true, string.Empty));
            item6.AddCheckItems(new CheckListItem(0, "Function test of RCD (* power disruption)",
                CheckValue.None, false, true, string.Empty));

            return new CheckList[] { item1, item3, item4, item6 };
        }
    }
}
