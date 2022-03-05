using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Shared;

namespace Inspections.Core.Interfaces.Repositories;

public interface ICheckListsRepository : IAsyncRepository<CheckList>
{
    Task<CheckListItem> GetItemByIdAsync(int id);
}
