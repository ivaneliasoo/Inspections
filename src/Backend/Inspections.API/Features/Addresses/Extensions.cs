using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Addresses.Models;
using Inspections.Core.Domain;

namespace Inspections.API.Features.Addresses
{
    internal static class NewAddressExtensions
    {
        internal static AddressDTO ToAddressDTO(this NewAddressDTO newAddress)
        {
            Guard.Against.Null(newAddress, nameof(newAddress));

            return new AddressDTO()
            {
                Id = newAddress.Id,
                AddressLine = newAddress.AddressLine,
                AddressLine2 = newAddress.AddressLine2,
                Unit = newAddress.Unit,
                Country = newAddress.Country,
                PostalCode = newAddress.PostalCode,
                LicenseId = newAddress.LicenseId
            };
        }

        internal static Address ToAddress(this NewAddressDTO newAddress)
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
                LicenseId = newAddress.LicenseId,
            };
        }
    }
}
