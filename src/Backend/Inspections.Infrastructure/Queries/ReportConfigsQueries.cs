using Inspections.Core.Interfaces;
using Inspections.Core.Interfaces.Queries;
using Inspections.Core.QueryModels;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.Infrastructure.Queries
{
    public class ReportConfigurationsQueries : IReportConfigurationsQueries
    {
        private readonly InspectionsContext _context;

        public ReportConfigurationsQueries(InspectionsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<ResumenReportConfiguration> GetByFilter(string filter)
        {
            return _context.ResumenReportConfigurations.FromSqlInterpolated($@"
                SELECT ""Id""
                      , ""Type""
                      , ""Title""
                      , ""FormName""
                      , ""RemarksLabelText""
                      , COALESCE(""DefinedCheckLists"".CheckLists, 0) as ""DefinedCheckLists""
                      , COALESCE(""DefinedSignatures"".Signatures, 0) as ""DefinedSignatures""
                      , COALESCE(Reports, 0) as ""UsedByReports""
                      , ""LastEdit""
                      , ""LastEditUser""
                      , ""Inactive""
                FROM ""Inspections"".""ReportsConfiguration"" Config
                    LEFT OUTER JOIN(
                    SELECT ""ReportConfigurationId"", COUNT(""ReportConfigurationId"") AS CheckLists
                    FROM ""Inspections"".""CheckLists""
                    GROUP BY ""ReportConfigurationId""
                ) AS ""DefinedCheckLists""
                    ON ""DefinedCheckLists"".""ReportConfigurationId"" = Config.""Id""
                    LEFT OUTER JOIN(
                    SELECT ""ReportConfigurationId"", COUNT(""ReportConfigurationId"") AS Signatures
                    FROM ""Inspections"".""Signatures""
                    GROUP BY ""ReportConfigurationId""
                ) AS ""DefinedSignatures""
                    ON ""DefinedSignatures"".""ReportConfigurationId"" = Config.""Id""
                    LEFT OUTER JOIN(
                    SELECT ""ReportConfigurationId"", COUNT(DISTINCT ""ReportId"") AS Reports
                    FROM ""Inspections"".""CheckLists""
                    GROUP BY ""ReportConfigurationId""
                ) AS ""Reports""
                    ON ""Reports"".""ReportConfigurationId"" = Config.""Id""
                    WHERE 1=1
            ").Where(p => !p.Inactive && (EF.Functions.Like(p.Title, $" %{filter ?? string.Empty}%") || EF.Functions.Like(p.FormName, $"%{filter ?? string.Empty}%")));
        }
    }
}
