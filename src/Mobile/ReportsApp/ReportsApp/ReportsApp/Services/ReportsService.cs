using ReportsApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ReportsApp.Services
{
    public interface IReportsService
    {
        Task<IEnumerable<Report>> GetReportsByFilter(string filter, bool? closed);
    }
    public class ReportsService :BaseService<IReportsApi>, IReportsService
    {
        public async Task<IEnumerable<Report>> GetReportsByFilter(string filter, bool? closed)
        {
            try
            {
                var result = await _api.GetReportsByFilter(filter, closed);
                if (result != null) return result;

                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
