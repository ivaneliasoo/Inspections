using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core
{
    public class ReportsFactory
    {
        public Report Create(IReportsRepository reportsRepository, IReportConfigurationsRepository reportConfigurationsRepository)
        {
            Guard.Against.Null(reportsRepository, nameof(reportsRepository));
            Guard.Against.Null(reportConfigurationsRepository, nameof(reportConfigurationsRepository));

            // TODO: Create reports based on configurations

            return null;
        }
    }
}
