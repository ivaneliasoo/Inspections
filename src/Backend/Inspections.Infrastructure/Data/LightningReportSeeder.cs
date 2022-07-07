using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.Forms;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inspections.Infrastructure.Data;

public static class LightningReportSeeder
{
    public static async Task CreateLightningReportConfiguration(InspectionsContext context, ILoggerFactory logger)
    {
        if (!await context.ReportConfigurations.AnyAsync(rc=> rc.FormName == "LPS-CSC-2019 (1)"))
        {
            var forms = AddForms();
            await context.AddRangeAsync(forms);
            await context.SaveChangesAsync();
            var config = new ReportConfiguration()
            {
                ChecksDefinition = AddCheckLists().ToList(),
                SignatureDefinitions = AddSignatures().ToList(),
                FormName = "LPS-CSC-2019 (1)",
                Title = "Lightning Inspections Form",
                Type = ReportType.Inspection,
                Footer = "",
                MarginBottom = "80px",
                MarginTop = "20px",
                MarginLeft = "70px",
                MarginRight = "70px",
                PrintSectionId = 1,
                CheckListMetadata = new CheckListPrintingMetadata {Display = CheckListDisplay.Numbered},
                TemplateName = "print",
                RemarksLabelText = "Remarks",
                UseNotes = true,
                Inactive = false,
                UsePhotoRecord = true,
                UseCheckList = true
            };
            
            await context.AddAsync(config);
        }
    }

    private static List<FormDefinition> AddForms()
    {
        return new List<FormDefinition>
        {
            new("ApprovedInfo",
                "Approved Info",
                "mdi-chart-bubble",
                new DynamicFields() {FieldsDefinitions = AddApprovedInfoFields()},
                0),
            new("ElectricalTest",
                "Electrical Test",
                "mdi-chart-bubble",
                new DynamicFields() {FieldsDefinitions = ElectricalTestFields()},
                1),
        };
    }

    private static DynamicFieldMetadata[]? ElectricalTestFields()
    {
        return new[]
        {
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Remarks",
                Order = 4,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "TestRemarks",
                InputType = "textarea",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Lightning Protection System Earthing & Electrical Continuity Test",
                SelectOptions = null,
                RollerOnMobile = true,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Test Date",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "TestDate",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Lightning Protection System Earthing & Electrical Continuity Test",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Brand & Model 1",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "Brand1",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Test Instrument Details",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Serial No.",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "Serial1",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Test Instrument Details",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Calibrated",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "Calibrated1",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Test Instrument Details",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Brand & Model 2",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "Brand2",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Test Instrument Details",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Serial No.",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "Serial2",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Test Instrument Details",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Calibrated",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "Calibrated2",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Test Instrument Details",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Point Resistance [R < N x 10Ω]",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "PointResistance",
                InputType = "slider-range",
                MaxLength = 3,
                DefaultValue = "10,20",
                RollerDigits = 3,
                SectionTitle = "Resistance of Earth Termination System [Ohm]",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Electrical Continuity Test Between N & N+1",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "ElectricalContinuityTest",
                InputType = "slider-range",
                MaxLength = 3,
                DefaultValue = "10,20",
                RollerDigits = 3,
                SectionTitle = "Resistance of Earth Termination System [Ohm]",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "[Roverall ≤ 10Ω]",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "ETSOverallResistance",
                InputType = "slider-range",
                MaxLength = 3,
                DefaultValue = "50,100",
                RollerDigits = 3,
                SectionTitle = "Overall resistance of the Earth Termination System",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Remarks",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "ETSRemarks",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Resistance of Earth Termination System [Ohm]",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Overall value in Ohm [R<0.2Ω]",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "ContinuityTestOverallValue",
                InputType = "slider-range",
                MaxLength = 3,
                DefaultValue = "50,100",
                RollerDigits = 3,
                SectionTitle = "Continuity Test for Down Conductor System [Ohm]",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Remarks",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "ContinuityTestRemarks",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Continuity Test for Down Conductor System [Ohm]",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
        };
    }

    private static DynamicFieldMetadata[]? AddApprovedInfoFields()
    {
        return new[]
        {
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Remarks",
                Order = 4,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "MainRemarks",
                InputType = "textarea",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Main",
                SelectOptions = null,
                RollerOnMobile = true,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "TS/MK & Lot/Plot:",
                Order = 3,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "TSMKLotPlot",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Main",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Building Name (if any)",
                Order = 2,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "BuildingName",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Main",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Project Ref. No.:",
                Order = 1,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "ProjectRefNo",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Main",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Remarks and Alternative Solution provided:",
                Order = 1,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "DesignSolutionProvided",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Design",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Address of Professional Engineer",
                Order = 1,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "DesignAddressOfProfessionalEngineer",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Design",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Phone Number of Professional Engineer",
                Order = 1,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "DesignPhoneNumberOfProfessionalEngineer",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Design",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            
            
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Remarks",
                Order = 1,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "SupervisionSolutionProvided",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Supervision",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Address of Professional Engineer",
                Order = 1,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "SupervisionAddressOfProfessionalEngineer",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Supervision",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
            new DynamicFieldMetadata
            {
                Max = 999,
                Min = 0,
                Step = 1,
                Label = "Phone Number of Professional Engineer",
                Order = 1,
                Prefix = "",
                Suffix = "",
                Enabled = true,
                Visible = true,
                FieldName = "SupervisionPhoneNumberOfProfessionalEngineer",
                InputType = "text",
                MaxLength = 3,
                DefaultValue = "",
                RollerDigits = 3,
                SectionTitle = "Supervision",
                SelectOptions = null,
                RollerOnMobile = false,
                SwitchableSection = ""
            },
        };
    }

    private static IEnumerable<Signature> AddSignatures()
    {
        return new[]
        {
            new Signature
            {
                Title = "INSPECTION BY",
                IsConfiguration = true,
                Annotation = "",
                Principal = true,
                Order = 1,
                DefaultResponsibleType = ResponsibleType.Inspector,
                UseLoggedInUserAsDefault = true,
            },
        };
    }

    private static IEnumerable<CheckList> AddCheckLists()
    {
        var item1 = new CheckList(
            "The design of the lightning protection system is in accordance with - ",
            "",
            true
        );
        item1.AddCheckItems(new CheckListItem(0,
            "the Code of Practice for Protection Against Lightning - SS 555:2010; or  ",
            CheckValue.Acceptable, false, true, string.Empty));
        item1.AddCheckItems(new CheckListItem(0, "the Code of Practice for Protection Against Lightning - SS 555:2018.",
            CheckValue.Acceptable, false, true, string.Empty));
        item1.AddCheckItems(new CheckListItem(0,
            "The design and installation of the lightning protection system are based on alternative solution ",
            CheckValue.Acceptable, false, true, string.Empty));

        return new[] {item1};
    }
}
