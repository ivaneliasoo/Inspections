
using Inspections.Core.Domain.ReportConfigurationAggregate;
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
                .Where(s => s.Id == id)
                .SingleOrDefaultAsync();

            var signaturesDef = _context.Signatures.Where(s => s.IsConfiguration && s.ReportId == null && s.ReportConfigurationId == id);
            var ChecksDef = _context.CheckLists.Where(s => s.IsConfiguration && s.ReportId == null)
                .Include(c=>c.Checks)
                .Where(s => s.IsConfiguration && s.ReportId == null && s.ReportConfigurationId == id);

            result.SignatureDefinitions = signaturesDef.ToList();
            result.ChecksDefinition = ChecksDef.ToList();

            return result;
        }

        public async Task UpdateAsync(ReportConfiguration entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
