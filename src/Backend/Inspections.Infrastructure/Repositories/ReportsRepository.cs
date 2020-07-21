using Inspections.Core;
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
        private readonly IUserNameResolver _userNameResolver;

        public ReportsRepository(InspectionsContext context, ILogger<ReportsRepository> logger
            , IUserNameResolver userNameResolver)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));
        }

        public async Task<Report> AddAsync(Report entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Report>> GetAll(string filter, bool? closed)
        {
            var query = _context.Reports
                .Include(p => p.CheckList)
                    .ThenInclude(p => p.Checks)
                .Include(p => p.Signatures)
                .Include(p => p.Notes)
                .Include(p => p.PhotoRecords);

            if(closed.HasValue)
                return await query.AsNoTracking().Where(r => (r.IsClosed)).ToListAsync();

            return await query.AsNoTracking().ToListAsync();
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
                .ThenInclude(p=>p.Checks)
                    .ThenInclude(p=>p.TextParams)
               .Include(p => p.Signatures)
                .ThenInclude(p=>p.Responsable)
               .Include(p => p.Notes)
               .Include(p => p.PhotoRecords)
               .Include(p=>p.License)
               .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Report entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
