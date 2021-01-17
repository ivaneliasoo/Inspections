using Refit;
using ReportsApp.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace ReportsApp.Services.Secutiry
{
    public interface IAuthenticationService
    {
        Task Login(string user, string password, string remember);
        void Logout();
    }

    public class AuthenticationService : BaseService<IAuthenticationApi>,  IAuthenticationService
    {
        public async Task Login(string user, string password, string remember)
        {
            try
            {
                var token = await _api.LoginAsync(new Models.LoginModel { Username = user, Password = password });
                await authStore.SetAuthorizationInfo(token);
                var userInfo = await _api.ActiveUserInfo(token);
                await authStore.SetAuthorizationInfo(userInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public void Logout()
        {
            try
            {
                authStore.RemoveAuthorizationInfo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> IsLoggedIn()
        {
            var token = await authStore.GetAuthorizationTokenAsync();
            return string.IsNullOrWhiteSpace(token) == false;
        }

    }
}
