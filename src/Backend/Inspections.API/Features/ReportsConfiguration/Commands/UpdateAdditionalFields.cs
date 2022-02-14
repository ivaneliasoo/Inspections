using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;

namespace Inspections.API.Features.ReportsConfiguration.Commands;

public record UpdateAdditionalFields(int Id, DynamicFields FieldsDefinitions) : IRequest<bool>;
