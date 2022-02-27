
using System;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inspections.Infrastructure.Repositories
{
    public class ReportsConfigurationsRepository : IReportConfigurationsRepository
    {
        private readonly InspectionsContext _context;
        private readonly ILogger<ReportsConfigurationsRepository> _logger;

        public ReportsConfigurationsRepository(InspectionsContext context, ILogger<ReportsConfigurationsRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ReportConfiguration> AddAsync(ReportConfiguration entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(ReportConfiguration entity)
        {
            _context.Set<ReportConfiguration>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ReportConfiguration> GetByIdAsync(int id)
        {
            var result = await _context.ReportConfigurations
           .SingleOrDefaultAsync(s => s.Id == id);

            if (result is { }) throw new NotFoundException(id.ToString(), nameof(ReportConfiguration));

            var signaturesDef = _context.Signatures.Where(s => s.IsConfiguration && s.ReportId == null && s.ReportConfigurationId == id);
            var checksDef = _context.CheckLists.Where(s => s.IsConfiguration && s.ReportId == null)
                .Include(c => c.Checks)
                .Where(s => s.IsConfiguration && s.ReportId == null && s.ReportConfigurationId == id);

            if (result != null)
            {
                result.SignatureDefinitions = signaturesDef.ToList();
                result.ChecksDefinition = checksDef.ToList();
            }

            return result ?? throw new NotFoundException(id.ToString(), nameof(ReportConfiguration));
        }

        public async Task UpdateAsync(ReportConfiguration entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
