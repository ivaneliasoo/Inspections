using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using JetBrains.Annotations;
using MediatR;

namespace Inspections.API.Features.ReportsConfiguration.Commands;

public class AddReportConfigurationCommand : IRequest<int>
{
    public AddReportConfigurationCommand(ReportType type,
        string title,
        string formName,
        string? remarksLabelText,
        List<int>? checksDefinition,
        List<int> signatureDefinitions,
        int printSectionId, CheckListDisplay display,
        string templateName,
        bool useNotes,
        bool useCheckList,
        bool usePhotoRecord)
    {
        Guard.Against.Null(display, nameof(display));

        Type = type;
        Title = title;
        FormName = formName;
        RemarksLabelText = remarksLabelText;
        ChecksDefinition = checksDefinition;
        SignatureDefinitions = signatureDefinitions;
        PrintSectionId = printSectionId;
        Display = display;
        TemplateName = templateName;
        UseNotes = useNotes;
        UseCheckList = useCheckList;
        UsePhotoRecord = usePhotoRecord;
    }

    private AddReportConfigurationCommand()
    {
        Display = CheckListDisplay.Numbered;
    }

    public ReportType Type { get; }
    public string Title { get; } = default!;
    public string FormName { get; } = default!;
    public string? RemarksLabelText { get; }
    public List<int>? ChecksDefinition { get; }
    public List<int> SignatureDefinitions { get; } = default!;

    // ReSharper disable once MemberCanBePrivate.Global
    public CheckListDisplay Display { [UsedImplicitly] get; }
    public int PrintSectionId { get; }
    public string? TemplateName { get; }
    public bool UseNotes { get; }
    public bool UseCheckList { get; }
    public bool UsePhotoRecord { get; }
}
