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
            return await _context.Reports
                .Include(p => p.CheckList)
                .Include(p=>p.Signatures)
                .Include(p=>p.Notes)
                .Include(p => p.PhotoRecords)
                .ToListAsync();
        }

        public Task DeleteAsync(Report entity)
        {
            _context.CheckLists.RemoveRange(entity.CheckList);
            _context.Signatures.RemoveRange(entity.Signatures);
            _context.Reports.Remove(entity);
            _context.SaveChangesAsync();

            return Task.CompletedTask;
        }

        public async Task<Report> GetByIdAsync(int id)
        {
            return await _context.Reports.Where(r=>r.Id == id)
               .Include(p => p.CheckList)
               .Include(p => p.Signatures)
               .Include(p => p.Notes)
               .Include(p => p.PhotoRecords)
               .SingleOrDefaultAsync();
        }

        public Task UpdateAsync(Report entity)
        {
            throw new NotImplementedException();
        }
    }
}
