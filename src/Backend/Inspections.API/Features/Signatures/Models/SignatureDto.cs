using Ardalis.GuardClauses;
using Inspections.Core.Domain.SignaturesAggregate;
using JetBrains.Annotations;

// ReSharper disable MemberCanBePrivate.Global

namespace Inspections.API.Features.Signatures.Models;

public class SignatureDto
{
    public int Id { [UsedImplicitly] get; }
    public string Title { [UsedImplicitly] get; }
    public string? Annotation { [UsedImplicitly] get; }
    public ResponsibleType ResponsableType { get; }
    public string? ResponsableName { get; }
    public string? Designation { get; }
    public string? Remarks { get; }
    public DateTimeOffset Date { get; }
    public bool Principal { get; }
    public int? ReportId { get; }
    public int? ReportConfigurationId { get; }
    public short Order { get; }

    public SignatureDto(Signature signature)
    {
        Guard.Against.Null(signature, nameof(signature));

        Id = signature.Id;
        Title = signature.Title;
        Annotation = signature.Annotation;
        Designation = signature.Designation;
        Remarks = signature.Remarks;
        Date = signature.Date;
        Principal = signature.Principal;
        if (!signature.IsConfiguration)
        {
            ResponsableType = signature.Responsible.Type;
            ResponsableName = signature.Responsible.Name;
        }
        Order = signature.Order;
        ReportConfigurationId = signature.ReportConfigurationId;
        ReportId = signature.ReportId;
    }
}
