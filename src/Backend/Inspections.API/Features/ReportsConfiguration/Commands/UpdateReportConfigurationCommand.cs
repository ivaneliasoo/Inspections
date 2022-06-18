using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;

namespace Inspections.API.Features.ReportsConfiguration.Commands
{
    public class UpdateReportConfigurationCommand : IRequest<bool>
    {
        public UpdateReportConfigurationCommand(int id,
                                             ReportType type,
                                             string title,
                                             string formName,
                                             string? remarksLabelText,
                                             List<int>? checksDefinition,
                                             List<int> signatureDefinitions,
                                             int printSectionId, 
                                             CheckListDisplay display,
                                             string templateName,
                                             bool useNotes,
                                             bool useCheckList,
                                             bool usePhotoRecord)
        {
            Guard.Against.Null(display, nameof(display));

            Id = id;
            Type = type;
            Title = title;
            FormName = formName;
            RemarksLabelText = remarksLabelText;
            ChecksDefinition = checksDefinition;
            SignatureDefinitions = signatureDefinitions;
            PrintSectionId = printSectionId;
            Display = display ;
            TemplateName = templateName;
            UseNotes = useNotes;
            UseCheckList = useCheckList;
            UsePhotoRecord = usePhotoRecord;
        }

        public bool UsePhotoRecord { get; set; }

        public int Id { get; set; }
        public ReportType Type { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string? RemarksLabelText { get; set; }
        public List<int>? ChecksDefinition { get; }
        public List<int>? SignatureDefinitions { get; }
        public int PrintSectionId { get; }
        public CheckListDisplay Display { get; }
        public string TemplateName { get; }
        public bool UseNotes { get; }
        public bool UseCheckList { get;  }
    }
}
