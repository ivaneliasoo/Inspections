using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.Infrastructure.Repositories
{
    public class CheckListsRepository : ICheckListsRepository
    {
        private readonly InspectionsContext _context;
        private readonly ILogger<CheckListsRepository> _logger;

        public CheckListsRepository(InspectionsContext context, ILogger<CheckListsRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CheckList> AddAsync(CheckList entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(CheckList entity)
        {
            _context.Set<CheckList>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<CheckList> GetByIdAsync(int id)
        {
            return await _context.Set<CheckList>()
                .Include(p=>p.Checks)
                .Where(c=>c.Id == id).SingleOrDefaultAsync();
        }

        public async Task<CheckListItem> GetItemByIdAsync(int id)
        {
            return await _context.Set<CheckListItem>().FindAsync(id);
        }

        public async Task UpdateAsync(CheckList entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
