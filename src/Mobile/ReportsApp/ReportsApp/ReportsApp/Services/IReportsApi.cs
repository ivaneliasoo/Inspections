using Refit;
using ReportsApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportsApp.Services
{
    public interface IReportsApi
    {
        [Get("/reports")]
        Task<IEnumerable<Report>> GetReportsByFilter(string filter, bool? closed);
    }
}
