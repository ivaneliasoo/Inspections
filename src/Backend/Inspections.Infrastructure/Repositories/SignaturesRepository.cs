﻿using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inspections.Infrastructure.Repositories;

public class SignaturesRepository : ISignaturesRepository
{
    private readonly InspectionsContext _context;
    private readonly ILogger<SignaturesRepository> _logger;

    public SignaturesRepository(InspectionsContext context, ILogger<SignaturesRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public async Task<Signature> AddAsync(Signature entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(Signature entity)
    {
        _context.Set<Signature>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Signature> GetByIdAsync(int id)
    {
        return await _context.Set<Signature>()
            .Where(s => s.Id == id)
            .Include(p => p.Responsible)
            .OrderBy(s => s.Order)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public async Task UpdateAsync(Signature entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
