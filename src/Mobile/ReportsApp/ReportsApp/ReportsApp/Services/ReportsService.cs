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
        Task<Report> GetById(int id);
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

        public async Task<Report> GetById(int id)
        {
            try
            {
                var result = await _api.GetById(id);
                if (result is null) return default;

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
