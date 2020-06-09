using Inspections.Core.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.Core.Interfaces.Queries
{
    public interface ICheckListsQueries
    {
        IEnumerable<ResumenCheckList> GetByFilter(string filter);
    }
}
