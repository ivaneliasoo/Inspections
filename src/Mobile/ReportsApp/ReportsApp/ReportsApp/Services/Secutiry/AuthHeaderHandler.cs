using ReportsApp.Persistence;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReportsApp.Services.Secutiry
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthorizationStore _authorizationStore;
        public AuthHeaderHandler(IAuthorizationStore authorizationStore, HttpClientHandler innerHandler = null)
            : base(innerHandler ?? new HttpClientHandler() { ServerCertificateCustomValidationCallback = (a, b, c, d) => { return true; } })

        {
            _authorizationStore = authorizationStore ?? throw new ArgumentNullException(nameof(authorizationStore));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authorizationStore.GetAuthorizationToken();

            if(request.Headers.Authorization == null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
