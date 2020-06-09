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

        public IEnumerable<ResumenCheckList> GetByFilter(string filter)
        {
            return _context.ResumenCheckLists.FromSqlRaw(@"
                SELECT Id,
                        Text,
                        Annotation,
                        Params.TotalParams,
                        Items.TotalItems,
                        LastEdit,
                        LastEditUser
                    FROM Inspections.CheckLists cl
                        LEFT JOIN (
                        SELECT CheckListId, COUNT([Key]) AS TotalParams
                        FROM Inspections.CheckListParams
                        GROUP BY CheckListId
                    ) Params
                        ON cl.Id = Params.CheckListId
                        INNER JOIN (
                        SELECT CheckListId, COUNT(Id) AS TotalItems
                        FROM Inspections.CheckListItems
                        GROUP BY CheckListId
                    ) Items
                        ON cl.Id = Items.CheckListId
                    WHERE 1=1
                        AND ([Text] LIKE {0}
                        OR Annotation LIKE {0}) 
            ", $"%{filter}%");
        }
    }
}
