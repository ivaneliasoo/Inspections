using Refit;
using ReportsApp.Persistence;
using ReportsApp.Services.Secutiry;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ReportsApp.Services
{
    public abstract class BaseService<T>
    {
        internal AuthorizationStore authStore;
        internal T _api;
        public BaseService()
        {
            authStore = new AuthorizationStore();
            _api = RestService.For<T>(new HttpClient(new AuthHeaderHandler(authStore))
            {
                BaseAddress = new Uri(Config.ReportsApi),
                Timeout = TimeSpan.FromSeconds(5)
            });
        }
    }
}
