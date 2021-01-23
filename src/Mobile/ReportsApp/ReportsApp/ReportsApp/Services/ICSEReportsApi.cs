using Refit;
using ReportsApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportsApp.Services
{
    public interface ICSEReportsApi
    {
        [Post("/auth/token")]
        Task<string> LoginAsync([Body] LoginModel login);

        [Get("/users/active")]
        Task<UserInfo> ActiveUserInfo(string token);

        [Get("/reports")]
        Task<IEnumerable<Report>> GetReportsByFilter(string filter, bool? closed);

        [Get("/reports/{reportId}")]
        Task<Report> GetById([AliasAs("reportId")] int id);

        [Get("/Addresses")]
        Task<IEnumerable<AddressDTO>> GetAddressesByFilter([AliasAs("filterString")]string filter);
    }
}
