using Ardalis.GuardClauses;
using Inspections.Core.Domain;

namespace Inspections.API.Features.Addresses.Models;

public class NewAddressDto
{
    public NewAddressDto(Address address)
    {
        Guard.Against.Null(address, nameof(address));

        Id = address.Id;
        AddressLine = address.AddressLine;
        AddressLine2 = address.AddressLine2;
        Unit = address.Unit;
        Country = address.Country;
        PostalCode = address.PostalCode;
        LicenseId = address.LicenseId;
    }

    public int Id { get; }
    public string AddressLine { get; }
    public string? AddressLine2 { get; }
    public string Unit { get; }
    public string Country { get; }
    public string PostalCode { get; }
    public int LicenseId { get; }
}
