
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
using Z.EntityFramework.Plus;

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
                .IncludeOptimized(p => p.SignatureDefinitions.Where(s => s.IsConfiguration && s.ReportId == null && s.ReportConfigurationId == id))
                .IncludeOptimized(p => p.ChecksDefinition.Where(cd => cd.IsConfiguration && cd.ReportId == null && cd.ReportConfigurationId == id))
                .IncludeOptimized(p => p.ChecksDefinition.Where(cd => cd.IsConfiguration && cd.ReportId == null && cd.ReportConfigurationId == id)
                .Select(sm => sm.Checks.Where(c => c.Text.Length > 0)))
                .IncludeOptimized(p => p.ChecksDefinition.Where(cd => cd.IsConfiguration && cd.ReportId == null && cd.ReportConfigurationId == id)
                .Select(sm => sm.TextParams.Where(c => c.Key.Length > 0)))
                .SingleOrDefaultAsync();
            
            if(result != null)
            {
                //EF limitations make me do it this way
                foreach (var check in result.ChecksDefinition)
                {
                    foreach (var checkItem in check.Checks)
                    {
                        var checksParams = _context.CheckListParams.Where(clp => clp.CheckListItemId == checkItem.Id).AsEnumerable();
                        checkItem.TextParams.AddRange(checksParams);
                    }
                }
            }

            return result;
        }

        public async Task UpdateAsync(ReportConfiguration entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
