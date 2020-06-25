using Inspections.Core.Domain.SignaturesAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inspections.Core.Interfaces.Queries
{
    public interface ISignaturesQueries
    {
        Task<IEnumerable<Signature>> GetAllAsync(string filter, bool? inConfigurationOnly, int? reportConfigurationId, int? reportId);
    }
}
