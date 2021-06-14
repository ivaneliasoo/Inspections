﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Inspections.API.Features.Addresses.Models;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;

namespace Inspections.API.Features.Addresses
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly InspectionsContext _context;

        public AddressesController(InspectionsContext context)
        {
            _context = context;
        }

        // GET: api/Addresses
        [HttpGet(Name = "GetAddresses")]
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAddresses(string? filter)
        {
            var result = await _context.Addresses
                .Where(ad => EF.Functions.Like(ad.AddressLine, $"%{filter}%") ||
                EF.Functions.Like(ad.Unit, $"%{filter}%") ||
                EF.Functions.Like(ad.Country, $"%{filter}%") ||
                EF.Functions.Like(ad.PostalCode, $"%{filter}%") ||
                EF.Functions.Like(ad.License.Number, $"%{filter}%"))
                .Select(a => new AddressDTO
                {
                    Id = a.Id,
                    AddressLine = a.AddressLine,
                    AddressLine2 = a.AddressLine2,
                    Unit = a.Unit,
                    Country = a.Country,
                    PostalCode = a.PostalCode,
                    LicenseId = a.LicenseId,
                    Number = a.License.Number,
                    Name = a.License.Name,
                    Amp = a.License.Amp,
                    KVA = a.License.KVA,
                    Volt = a.License.Volt,
                    Validity = a.License.Validity,
                    FormatedAddress = a.ToString()
                })
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
            var mappedResult = result;
            return Ok(mappedResult);
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDTO>> GetAddress(int id)
        {
            var address = await _context.Addresses.AsNoTracking().Where(a => a.Id == id).Select(ad => new AddressDTO(ad)).SingleOrDefaultAsync();

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        // PUT: api/Addresses/5
        [HttpPut("{id}", Name = "UpdateAddress")]
        public async Task<IActionResult> PutAddress(int id, [FromBody] AddressDTO address)
        {
            Guard.Against.Null(address, nameof(address));

            if (id != address.Id)
            {
                return BadRequest();
            }

            var savedAddress = await _context.Set<Address>().FindAsync(id).ConfigureAwait(false);
            savedAddress.AddressLine = address.AddressLine;
            savedAddress.AddressLine2 = address.AddressLine2;
            savedAddress.Unit = address.Unit;
            savedAddress.Country = address.Country;
            savedAddress.PostalCode = address.PostalCode;
            savedAddress.LicenseId = address.LicenseId;

            _context.Entry(savedAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Addresses
        [HttpPost(Name = "AddAddress")]
        public async Task<ActionResult<Address>> PostAddress([FromBody] AddressDTO address)
        {
            Guard.Against.Null(address, nameof(address));

            var newAddress = new Address()
            {
                AddressLine = address.AddressLine,
                AddressLine2 = address.AddressLine2,
                Unit = address.Unit,
                Country = address.Country,
                PostalCode = address.PostalCode,
                LicenseId = address.LicenseId
            };

            if (AddressDuplicated(newAddress))
                return BadRequest("Address already exists");

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return CreatedAtAction("GetAddress", new { id = address.Id }, address);
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}", Name = "DeleteAddress")]
        public async Task<ActionResult<AddressDTO>> DeleteAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id).ConfigureAwait(false);
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return new AddressDTO(address);
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }

        private bool AddressDuplicated(Address address)
        {
            return _context.Addresses.Any(e => e.AddressLine == address.AddressLine &&
            e.AddressLine2 == address.AddressLine2 &&
            e.Unit == address.Unit &&
            e.Country == address.Country &&
            e.PostalCode == address.PostalCode);
        }
    }
}
