using Inspections.Shared;

namespace Inspections.Core.Domain;

public class Address : Entity<int>
{
    public string AddressLine { get; set; } = default!;
    public string? AddressLine2 { get; set; }
    public string Unit { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public int? LicenseId { get; set; }
    public EMALicense? License { get; set; } = default!;

    public override string ToString()
    {
        return $"{AddressLine} {AddressLine2} {Unit} {Country} {PostalCode}";
    }
}
