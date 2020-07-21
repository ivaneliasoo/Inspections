using Microsoft.AspNetCore.Http;
using System;

namespace Inspections.Core
{
    public class UserNameResolver : IUserNameResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserNameResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public string UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "pruebas";
        public bool IsAdmin => _httpContextAccessor.HttpContext?.User?.HasClaim("IsAdmin", "true") ?? false;
    }
}
