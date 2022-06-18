using Inspections.Core.Domain;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.PrintSectionsAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inspections.Infrastructure.Data;

public class InspectionsSeed
{
    public static async Task SeedAsync(InspectionsContext context, ILoggerFactory logger
        , int retriesNumber = 0)
    {

        if (!context.Database.IsInMemory())
        {
            await context.Database.MigrateAsync();
        }

        var log = logger.CreateLogger<InspectionsSeed>();
        int retries = retriesNumber;
        try
        {
            log.LogInformation($"Initializing data {nameof(InspectionsContext)}");

            if (!await context.Users.AnyAsync(u => u.UserName == "demo"))
            {
                await context.Users.AddAsync(new User() { UserName = "demo", Password = "demo", Name = "demo", LastName = "User", IsAdmin = true });
            }

            if (!await context.Users.AnyAsync(u => u.UserName == "developer"))
            {
                await context.Users.AddAsync(new User() { UserName = "developer", Password = "developer@@P@sword", Name = "Developer", LastName = "User", IsAdmin = true });
            }
            
            if (!await context.Users.AnyAsync(u => u.UserName == "reports"))
            {
                await context.Users.AddAsync(new User() { UserName = "reports", Password = "123456", Name = "Reports", LastName = "User", IsAdmin = true });
            }

            if (!await context.PrintSections.AnyAsync(u => u.Code == "Inspection"))
            {
                await context.PrintSections.AddAsync(new PrintSection() { Code = "Inspection", Content = "<h1>Inspection</h1>", Description = "Inspection" });
            }

            if (!(await context.Addresses.AnyAsync(a => a.Id == 1)))
            {
                await context.Addresses.AddAsync(new Address
                {
                    Country = "Singapore",
                    Unit = "unit",
                    AddressLine = "Some Sample Address",
                    License = new EMALicense
                    {
                        Amp = 120,
                        Contact = "Someone",
                        Email = "someone@anotherone.com",
                        Name = "Some Person or Company",
                        Number = "E123456",
                        Validity = new DateTimeRange { Start = DateTime.Now, End = DateTime.Now.AddMonths(1) },
                        Volt = 120,
                        KVA = 120,
                        PersonInCharge = "Some Powered Person" 
                    },
                    PostalCode = "123456"
                });
                
                await context.Addresses.AddAsync(new Address
                {
                    Country = "Singapore",
                    Unit = "unit 2",
                    AddressLine = "Some Sample Address 2",
                    License = new EMALicense
                    {
                        Amp = 400,
                        Contact = "Someone2",
                        Email = "someone2@anotherone.com",
                        Name = "Some Person or Company 2",
                        Number = "E654321",
                        Validity = new DateTimeRange { Start = DateTime.Now, End = DateTime.Now.AddYears(1) },
                        Volt = 400,
                        KVA = 400,
                        PersonInCharge = "Some Powered Person 2" 
                    },
                    PostalCode = "654321"
                });
            }

            if (!await context.ReportConfigurations.AnyAsync(rc => rc.FormName == "CSE EI(R8) FORM"))
            {
                await context.AddAsync(new ReportConfiguration()
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
                    MarginRight = "70px",
                    PrintSectionId = 1,
                    CheckListMetadata = new CheckListPrintingMetadata { Display = CheckListDisplay.Numbered },
                    TemplateName = "print",
                    RemarksLabelText = "Remarks",
                    UseNotes = false,
                    Inactive = false,
                    UsePhotoRecord = true,
                });
            }

            const int templateId = 1;
            if (!await context.Template.AnyAsync(t => t.id == templateId))
            {
                log.LogInformation("No template found in the database, copying template from file categories.json");

                var categoriesFilePath = Path.Combine(AppContext.BaseDirectory, "categories.json");
                var reader = File.OpenText(categoriesFilePath);
                var categories = await reader.ReadToEndAsync();

                Template t = new() {id = templateId, templateDef = categories};

                await context.AddAsync(t);
            }

            const int optionsId = 1;
            if (!await context.Options.AnyAsync(opt => opt.id == optionsId))
            {
                Options opt = new Options {id = 1, scheduleWeeks = 1, autosaveInterval = 120};

                await context.AddAsync(opt);
            }

            if (!await context.Team.AnyAsync())
            {
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
                await context.AddRangeAsync(teams);
            }
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (retries < retriesNumber)
            {
                retries++;

                log.LogError(ex, "Stopped at Retry: {Retries}", retries);
                await SeedAsync(context, logger);
            }
        }
    }

    private static IEnumerable<Signature> AddSignatures()
    {
        return new[] {
            new Signature
            {
                Title = "INSPECTION CARRIED OUT BY",
                IsConfiguration = true,
                Annotation = "",
                Principal = true,
                Order = 1,
                DefaultResponsibleType = ResponsibleType.Inspector,
                UseLoggedInUserAsDefault = true,
            },
            new Signature
            {
                Title = "THE INSPECTION WAS WITNESSED BY REPRESENTATIVE OF THE LICENSEE",
                IsConfiguration = true,
                Annotation = "",
                Order = 2,
                DefaultResponsibleType = ResponsibleType.Witness,
                UseLoggedInUserAsDefault = false,
            },
            new Signature
            {
                Title = "DECLARATION BY LEW",
                IsConfiguration = true,
                Annotation = "I am satisfied that the electrical installation is fit for operation.",
                Order = 3,
                DefaultResponsibleType = ResponsibleType.LEW,
                UseLoggedInUserAsDefault = false,
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
        item4.AddCheckItems(new CheckListItem(0, "Labeling of circuits",
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

        return new[] { item1, item3, item4, item6 };
    }
}
