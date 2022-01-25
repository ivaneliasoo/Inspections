using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.Core.Domain.PrintSectionsAggregate;
using Inspections.Core.Interfaces.Queries;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inspections.Infrastructure.Queries
{

    public class PrintSectionsQueries : IPrintSectionsQueries
    {
        private readonly InspectionsContext _context;

        public PrintSectionsQueries(InspectionsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<IEnumerable<PrintSection>> GetAllAsync(string filter)
        {

            return Task.FromResult(_context.Set<PrintSection>()
                                            .Where(s => EF.Functions.Like(s.Code, $"%{filter}%"))
                                            .OrderBy(s => s.Id)
                                            .AsEnumerable());
        }
    }
}
