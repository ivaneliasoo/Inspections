using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inspections.Core.Domain.PrintSectionsAggregate;

namespace Inspections.Core.Interfaces.Queries
{
    public interface IPrintSectionsQueries
    {
        Task<IEnumerable<PrintSection>> GetAllAsync(string filter);
    }
}
