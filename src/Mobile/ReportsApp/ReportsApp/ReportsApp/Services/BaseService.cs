using Refit;
using ReportsApp.Persistence;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace ReportsApp.Services
{
    public abstract class BaseService<T>
    {
        internal IAuthorizationStore authStore;
        internal T _api;
        public BaseService()
        {
            authStore = DependencyService.Get<IAuthorizationStore>();
            _api = RestService.For<T>(new HttpClient(new AuthHeaderHandler(authStore))
            {
                BaseAddress = new Uri("http://inspectionsapi-dev.eba-r84ntzqp.us-east-2.elasticbeanstalk.com"),
                Timeout = TimeSpan.FromSeconds(5)
            });
        }
    }
}
