using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Shared;
using System.Threading.Tasks;

namespace Inspections.Core.Interfaces
{
    public interface ICheckListsRepository : IAsyncRepository<CheckList>
    {
        Task<CheckListItem> GetItemByIdAsync(int id);
    }
}
