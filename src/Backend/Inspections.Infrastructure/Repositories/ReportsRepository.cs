using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.Infrastructure.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly InspectionsContext _context;
        private readonly ILogger<ReportsRepository> _logger;

        public ReportsRepository(InspectionsContext context, ILogger<ReportsRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Report> AddAsync(Report entity)
        {
            

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Report>> GetAll(string filter)
        {
            // TODO: Change
            return await _context.Inspections.ToListAsync();
        }

        public Task DeleteAsync(Report entity)
        {
            throw new NotImplementedException();
        }

        public Task<Report> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Report entity)
        {
            throw new NotImplementedException();
        }
    }
}
