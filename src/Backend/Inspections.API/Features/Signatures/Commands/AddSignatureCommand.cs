using Inspections.Core.Domain.SignaturesAggregate;
using JetBrains.Annotations;
using MediatR;

namespace Inspections.API.Features.Signatures.Commands;

public record AddSignatureCommand(string Title, string? Annotation, ResponsibleType ResponsibleType,
    string ResponsibleName, string? Designation, string? Remarks, DateTimeOffset? Date, bool Principal, int? ReportId,
    int? ReportConfigurationId, short Order,
    ResponsibleType? DefaultResponsibleType,
    bool UseLoggedInUserAsDefault,
    string Signature = ""
) : IRequest<bool>;
