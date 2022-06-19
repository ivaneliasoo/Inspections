using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inspections.Infrastructure.Data;

public static class InspectionReportSeeder
{
    public static async Task CreateInspectionReportConfiguration(InspectionsContext context, ILoggerFactory logger)
    {
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
                TemplateName = "printtemplates/print",
                RemarksLabelText = "Remark",
                UseNotes = false,
                Inactive = false,
                UsePhotoRecord = true,
            });
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
