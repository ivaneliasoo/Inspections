using System;
using System.Linq;
using System.Threading.Tasks;
using Inspections.Core.Domain.PrintSectionsAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inspections.Infrastructure.Repositories
{
    public class PrintSectionRepository : IPrintSectionRepository
    {
        private readonly InspectionsContext _context;
        private readonly ILogger<PrintSectionRepository> _logger;

        public PrintSectionRepository(InspectionsContext context, ILogger<PrintSectionRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PrintSection> AddAsync(PrintSection entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(PrintSection entity)
        {
            _context.Set<PrintSection>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PrintSection> GetByIdAsync(int id)
        {
            return await _context.Set<PrintSection>()
                .Where(s => s.Id == id)
                .OrderBy(s => s.Code)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(PrintSection entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
