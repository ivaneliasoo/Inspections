using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces.Queries;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.Infrastructure.Queries
{
    public class SignaturesQueries : ISignaturesQueries
    {
        private readonly InspectionsContext _context;

        public SignaturesQueries(InspectionsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<IEnumerable<Signature>> GetAllAsync(string? filter, bool? inConfigurationOnly, int? reportConfigurationId, int? reportId)
        {
            if (inConfigurationOnly == false)
                inConfigurationOnly = null;

            return Task.FromResult(_context.Set<Signature>()
                                            .Where(s => EF.Functions.Like(s.Title, $"%{filter}%") 
                                            && (s.IsConfiguration == inConfigurationOnly || inConfigurationOnly == null)
                                            && (s.ReportConfigurationId == reportConfigurationId || reportConfigurationId == null)
                                            && (s.ReportId == reportId || reportId == null))
                                            .AsEnumerable());
        }
    }
}
