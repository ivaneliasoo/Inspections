using Inspections.Core.Domain.PrintSectionsAggregate;

namespace Inspections.Core.Interfaces.Queries;

public interface IPrintSectionsQueries
{
    Task<IEnumerable<PrintSection>> GetAllAsync(string filter);
}
