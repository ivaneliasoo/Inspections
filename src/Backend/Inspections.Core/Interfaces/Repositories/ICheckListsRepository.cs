using System.Threading.Tasks;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Shared;

namespace Inspections.Core.Interfaces
{
    public interface ICheckListsRepository : IAsyncRepository<CheckList>
    {
        Task<CheckListItem> GetItemByIdAsync(int id);
    }
}
