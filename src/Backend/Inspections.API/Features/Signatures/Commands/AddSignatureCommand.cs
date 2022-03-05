using Inspections.Core.Domain.SignaturesAggregate;
using JetBrains.Annotations;
using MediatR;

namespace Inspections.API.Features.Signatures.Commands;

public class AddSignatureCommand : IRequest<bool>
{
    public AddSignatureCommand(string title,
        string annotation,
        ResponsibleType responsibleType,
        string responsibleName,
        string designation,
        string remarks,
        DateTimeOffset date,
        bool principal,
        int reportId,
        int reportConfigurationId,
        short order)
    {
        Title = title;
        Annotation = annotation;
        ResponsableType = responsibleType;
        ResponsibleName = responsibleName;
        Designation = designation;
        Remarks = remarks;
        Date = date;
        Principal = principal;
        ReportId = reportId;
        ReportConfigurationId = reportConfigurationId;
        Order = order;
    }

    private AddSignatureCommand() { }

    public string Title { get; } = default!;
    public string? Annotation { get; }
    public ResponsibleType ResponsableType { get; }
    public string ResponsibleName { get; } = default!;
    public string? Designation { get; }
    public string? Remarks { get; }
    public DateTimeOffset Date { get; }
    public bool Principal { get; }
    // public bool IsConfiguration { get; set; }
    public int? ReportId { get; }
    public int? ReportConfigurationId { get; }
    // ReSharper disable once MemberCanBePrivate.Global
    public short Order { [UsedImplicitly] get; }
}
