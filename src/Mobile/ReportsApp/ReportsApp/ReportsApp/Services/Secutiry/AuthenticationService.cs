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

    public class AuthenticationService : IAuthenticationService
    {
        private AuthorizationStore authStore;
        private IAuthenticationApi _api;
       
        public AuthenticationService()
        {
            authStore = new AuthorizationStore();
            _api = RestService.For<IAuthenticationApi>(new HttpClient(new AuthHeaderHandler(authStore))
            {
                BaseAddress = new Uri("http://inspectionsapi-dev.eba-r84ntzqp.us-east-2.elasticbeanstalk.com"),
                Timeout = TimeSpan.FromSeconds(5)
            });
        }
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
    }
}
