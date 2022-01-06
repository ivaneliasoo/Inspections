using System;
using Microsoft.AspNetCore.Http;

namespace Inspections.Infrastructure.ApplicationServices
{
    public class UserNameResolver : IUserNameResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserNameResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public string UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "pruebas";
        public string FullName => _httpContextAccessor.HttpContext?.User?.FindFirst("fullName")?.Value ?? "usuario pruebas";
        public bool IsAdmin => _httpContextAccessor.HttpContext?.User?.HasClaim("IsAdmin", "true") ?? false;
    }
}
