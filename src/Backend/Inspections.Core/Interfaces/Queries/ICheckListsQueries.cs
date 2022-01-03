using Inspections.Core.QueryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inspections.Core.Interfaces.Queries
{
    public interface ICheckListsQueries
    {
        Task<IEnumerable<ResumenCheckList>> GetByFilter(string? filter, bool? inConfigurationOnly, int? reportConfigurationId, int? reportId);
    }
}
