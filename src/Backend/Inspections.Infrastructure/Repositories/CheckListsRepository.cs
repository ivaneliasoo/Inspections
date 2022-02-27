using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inspections.Infrastructure.Repositories;

public class CheckListsRepository : ICheckListsRepository
{
    private readonly InspectionsContext _context;

    public CheckListsRepository(InspectionsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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

    public async Task<CheckList> GetByIdAsync(int id) {
        var result = await _context.Set<CheckList>()
            .Include(p => p.Checks)
            .SingleOrDefaultAsync(c => c.Id == id);

        if (result is { })
            throw new NotFoundException(id.ToString(), nameof(CheckList));

        return result!;
    }

    public async Task<CheckListItem> GetItemByIdAsync(int id)
    {
        return await _context.Set<CheckListItem>().FindAsync(id) ?? throw new NotFoundException(id.ToString(), nameof(CheckListItem));
    }

    public async Task UpdateAsync(CheckList entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
