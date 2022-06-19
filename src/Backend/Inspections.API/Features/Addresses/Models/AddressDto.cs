using Ardalis.GuardClauses;
using Inspections.Core.Domain;
using Inspections.Shared;

namespace Inspections.API.Features.Addresses.Models;

public class AddressDto
{
    public AddressDto(Address address)
    {
        Guard.Against.Null(address, nameof(address));

        Id = address.Id;
        AddressLine = address.AddressLine;
        AddressLine2 = address.AddressLine2;
        Unit = address.Unit;
        Country = address.Country;
        PostalCode = address.PostalCode;
        LicenseId = address.LicenseId;
        Number = address.License?.Number;
        Validity = address.License?.Validity;
        Name = address.License?.Name ?? string.Empty;
        Amp = address.License?.Amp ?? 0;
        Volt = address.License?.Volt ?? 0;
        KVA = address.License?.KVA ?? 0;
        FormatedAddress = address.ToString();
    }

    public AddressDto()
    {

    }

    public int Id { get; set; }
    public string AddressLine { get; set; } = default!;
    public string? AddressLine2 { get; set; }
    public string Unit { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public int? LicenseId { get; set; }
    public string? Number { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Amp { get; set; }
    public decimal Volt { get; set; }
    public decimal KVA { get; set; }
    public DateTimeRange? Validity { get; set; } = default!;
    public string? FormatedAddress { get; set; } = default!;
}
