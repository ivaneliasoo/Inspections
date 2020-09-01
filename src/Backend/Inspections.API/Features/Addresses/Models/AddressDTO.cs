using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.Core.Domain;

namespace Inspections.API.Features.Addresses.Models
{
    public class AddressDTO
    {
        public AddressDTO(Address address)
        {
            Id = address.Id;
            AddressLine = address.AddressLine;
            AddressLine2 = address.AddressLine2;
            City = address.City;
            Province = address.Province;
            FormatedAddress = address.ToString();
        }

        public AddressDTO()
        {

        }

        public int Id { get; set; }
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string FormatedAddress { get; set; }
    }
}
