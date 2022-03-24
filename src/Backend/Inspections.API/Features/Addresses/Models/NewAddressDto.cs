using Ardalis.GuardClauses;
using Inspections.Core.Domain;

namespace Inspections.API.Features.Addresses.Models;

public class NewAddressDto
{
    public NewAddressDto()
    {
            
    }
    public NewAddressDto(int id, string addressLine, string? addressLine2, string unit, string country, string postalCode, int licenseId)
    {
        Id = id;
        AddressLine = addressLine;
        AddressLine2 = addressLine2;
        Unit = unit;
        Country = country;
        PostalCode = postalCode;
        LicenseId = licenseId;
    }

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

    public int Id { get; init; }
    public string AddressLine { get; init;}
    public string? AddressLine2 { get; init;}
    public string Unit { get; init;}
    public string Country { get; init;}
    public string PostalCode { get; init;}
    public int LicenseId { get; init;}
}
