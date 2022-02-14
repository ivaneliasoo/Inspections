using Inspections.Core.Domain.ReportConfigurationAggregate;
using MediatR;

namespace Inspections.API.Features.ReportsConfiguration.Commands;

public record UpdateOperationalReadings(int Id, DynamicFields FieldsDefinitions) : IRequest<bool>;
