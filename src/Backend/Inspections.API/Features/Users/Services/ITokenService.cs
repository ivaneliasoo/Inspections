using Inspections.Core.Domain;

namespace Inspections.API.Features.Users.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}