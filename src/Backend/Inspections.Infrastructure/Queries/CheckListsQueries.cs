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
    public class CheckListsQueries : ICheckListsQueries
    {
        private readonly InspectionsContext _context;

        public CheckListsQueries(InspectionsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<ResumenCheckList> GetByFilter(string filter, bool? inConfigurationOnly, int? reportConfigurationId, int? reportId)
        {
            if (inConfigurationOnly == false)
                inConfigurationOnly = null;

            return _context.ResumenCheckLists.FromSqlRaw(@"
                SELECT ""Id"",
                        ""Text"",
                        ""Annotation"",
                        COALESCE(""Items"".TotalItems, 0) ""TotalItems"",
                        ""LastEdit"",
                        ""LastEditUser""
                    FROM ""Inspections"".""CheckLists"" ""cl""
                        LEFT JOIN(
                                            SELECT ""CheckListId"", COUNT(""Id"") AS TotalItems
                        FROM ""Inspections"".""CheckListItems""
                        GROUP BY ""CheckListId""
                                        ) ""Items""
                        ON ""cl"".""Id"" = ""Items"".""CheckListId""
                    WHERE 1=1
                        AND (""Text"" LIKE {0}
                        OR ""Annotation"" LIKE {0})
                        AND (""cl"".""ReportId"" = {1} OR {1} IS NULL)
                        AND (""cl"".""IsConfiguration"" = {2} OR {2} IS NULL)
                        AND (""cl"".""ReportConfigurationId"" = {3} OR {3} IS NULL)
            ", $"%{filter ?? string.Empty}%", reportId, inConfigurationOnly, reportConfigurationId);
        }
    }
}
