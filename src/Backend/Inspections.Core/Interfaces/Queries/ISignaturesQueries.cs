using Inspections.Core.Domain.SignaturesAggregate;

namespace Inspections.Core.Interfaces.Queries;

public interface ISignaturesQueries
{
    Task<IEnumerable<Signature>> GetAllAsync(string? filter, bool? inConfigurationOnly, int? reportConfigurationId, int? reportId);
}
