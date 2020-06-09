using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core.Interfaces
{
    public interface IReportsRepository : IAsyncRepository<Report>
    {
    }
}
