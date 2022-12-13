using Ardalis.GuardClauses;
using Inspections.Core.Domain;

namespace Inspections.API.Features.Addresses.Models;

public class NewAddressDto
{
    public NewAddressDto()
    {
        Unit = string.Empty;
        Company = string.Empty;
        Country = string.Empty;
        PostalCode = string.Empty;
        AddressLine = string.Empty;
        LicenseId = null;
        AttentionTo = string.Empty;
    }

    public NewAddressDto(int id, string addressLine, string? addressLine2, string unit, string country,
        string postalCode, int licenseId, string company, string attentionTo)
    {
        Id = id;
        Company = company;
        AddressLine = addressLine;
        AddressLine2 = addressLine2;
        Unit = unit;
        Country = country;
        AttentionTo = attentionTo;
        PostalCode = postalCode;
        LicenseId = LicenseId == 0 ? null : LicenseId;
    }

    public NewAddressDto(Address address)
    {
        Guard.Against.Null(address, nameof(address));

        Id = address.Id;
        Company = address.Company;
        AddressLine = address.AddressLine;
        AddressLine2 = address.AddressLine2;
        Unit = address.Unit;
        Country = address.Country;
        PostalCode = address.PostalCode;
        AttentionTo = address.AttentionTo;
        LicenseId = address.LicenseId == 0 ? null : address.LicenseId;
    }

    public int Id { get; init; }
    public string? Company { get; init; }
    public string AddressLine { get; init; }
    public string? AddressLine2 { get; init; }
    public string Unit { get; init; }
    public string Country { get; init; }
    public string PostalCode { get; init; }
    public string AttentionTo { get; init; }
    public int? LicenseId { get; init; }
}
