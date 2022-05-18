namespace Inspections.API.Models.Configuration;

public class JwtSettings
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public double ExpirationHours { get; set; }
}
