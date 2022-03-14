using Inspections.Core.Domain.SignaturesAggregate;
using JetBrains.Annotations;
using MediatR;

namespace Inspections.API.Features.Signatures.Commands;

public record AddSignatureCommand(string Title, string? Annotation,ResponsibleType ResponsableType, string ResponsibleName, string? Designation, string? Remarks, DateTimeOffset Date, bool Principal, int? ReportId, int? ReportConfigurationId, short Order, string signature = "") : IRequest<bool>;
