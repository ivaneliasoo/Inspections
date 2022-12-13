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

            var address = context.Addresses.Where(ad => ad.Id == 1).FirstOrDefault();
            if (address is null)
            {
                Console.WriteLine("\n***No default address. Inserting new default address\n");
                await context.Addresses.AddAsync(new Address
                {
                    Id = 1,
                    Company = "",
                    Country = "",
                    Unit = "",
                    AddressLine = "Not Licensed",
                    License = new EMALicense
                    {
                        Amp = 0,
                        Contact = "",
                        Email = "",
                        Name = "Not Licensed",
                        Number = "",
                        Validity = new DateTimeRange { Start = DateTime.Now, End = DateTime.Now.AddMonths(1) },
                        Volt = 0,
                        KVA = 0,
                        PersonInCharge = ""
                    },
                    PostalCode = ""
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
            else
            {
                Console.WriteLine("\n***Default address found. Updating\n");
                address.Company = "";
                address.Country = "";
                address.Unit = "";
                address.AddressLine = "Not Licensed";
                address.PostalCode = "654321";
                address.AttentionTo = "";
                address.AddressLine2 = "";
                if (address.License is not null)
                {
                    address.License.Amp = 0;
                    address.License.Amp = 0;
                    address.License.Contact = "";
                    address.License.Email = "";
                    address.License.Name = "Not Licensed";
                    address.License.Number = "";
                    address.License.Validity = new DateTimeRange { Start = DateTime.Now, End = DateTime.Now.AddMonths(1) };
                    address.License.Volt = 0;
                    address.License.KVA = 0;
                    address.License.PersonInCharge = "";
                }
            }

            await InspectionReportSeeder.CreateInspectionReportConfiguration(context, logger);
            await LightningReportSeeder.CreateLightningReportConfiguration(context, logger);

            const int templateId = 1;
            if (!await context.Template.AnyAsync(t => t.id == templateId))
            {
                log.LogInformation("No template found in the database, copying template from file categories.json");

                var categoriesFilePath = Path.Combine(AppContext.BaseDirectory, "categories.json");
                var reader = File.OpenText(categoriesFilePath);
                var categories = await reader.ReadToEndAsync();

                Template t = new() { id = templateId, templateDef = categories };

                await context.AddAsync(t);
            }

            const int optionsId = 1;
            if (!await context.Options.AnyAsync(opt => opt.id == optionsId))
            {
                Options opt = new Options { id = 1, scheduleWeeks = 1, autosaveInterval = 120 };

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
}
