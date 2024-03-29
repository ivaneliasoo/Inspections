﻿using Inspections.Core.Domain;
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

            await InspectionReportSeeder.CreateInspectionReportConfiguration(context, logger);
            await LightningReportSeeder.CreateLightningReportConfiguration(context, logger);

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
}
