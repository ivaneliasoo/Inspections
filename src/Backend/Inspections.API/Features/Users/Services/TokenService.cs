using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.GuardClauses;
using Inspections.API.Models.Configuration;
using Inspections.Core.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Inspections.API.Features.Users.Services
{
    public class TokenService : ITokenService
    {
        private const string TOKEN_GENERATED_MESSAGE = "User Token Generated.";
        private readonly ILogger<TokenService> _logger;
        private readonly IOptions<JwtSettings> _options;

        public TokenService(ILogger<TokenService> logger, IOptions<JwtSettings> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Value.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Value.Issuer,
                Audience = _options.Value.Audience,
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddHours(_options.Value.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            _logger.LogInformation(TOKEN_GENERATED_MESSAGE);

            return tokenHandler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            Guard.Against.Null(user, nameof(user));

            string _userName = user.UserName;
            string[] _scopes = new string[] { "inspections-reports", "inspections-configuration" };
            return new ClaimsIdentity(new[]
                     {
                    new Claim(ClaimTypes.NameIdentifier, _userName.Trim()),
                    new Claim(ClaimTypes.Name, _userName.Trim()),
                    new Claim("fullName", $"{user.Name} {user.LastName}"),
                    new Claim("scopes" , string.Join(' ', _scopes))
                });
        }
    }


}
