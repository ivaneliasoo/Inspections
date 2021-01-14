using Refit;
using ReportsApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportsApp.Services.Secutiry
{
    public interface IAuthenticationApi
    {
        [Post("/auth/token")]
        Task<string> LoginAsync([Body] LoginModel login);

        [Get("/users/active")]
        Task<UserInfo> ActiveUserInfo([Authorize("Bearer")] string token);
    }
}
