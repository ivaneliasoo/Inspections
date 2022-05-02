using Ardalis.GuardClauses;
using Inspections.API.Features.Addresses.Models;
using Inspections.Core.Domain;

namespace Inspections.API.Features.Addresses;

internal static class NewAddressExtensions
{
    internal static Address ToAddress(this NewAddressDto newAddress)
    {
        Guard.Against.Null(newAddress, nameof(newAddress));

        return new Address
        {
            Id = newAddress.Id,
            AddressLine = newAddress.AddressLine,
            AddressLine2 = newAddress.AddressLine2,
            Unit = newAddress.Unit,
            Country = newAddress.Country,
            PostalCode = newAddress.PostalCode,
            LicenseId = newAddress.LicenseId == 0 ? null:newAddress.LicenseId,
        };
    }
}
