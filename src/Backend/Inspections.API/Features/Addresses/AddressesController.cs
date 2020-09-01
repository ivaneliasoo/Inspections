using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Inspections.API.Features.Addresses.Models;

namespace Inspections.API.Features.Addresses
{
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAddresses(string filter)
        {
            var result = await _context.Addresses
                .Where(ad => EF.Functions.Like(ad.AddressLine, $"%{filter}%") ||
                EF.Functions.Like(ad.City, $"%{filter}%") ||
                EF.Functions.Like(ad.Province, $"%{filter}%"))
                .ToListAsync()
                .ConfigureAwait(false);
            var mappedResult = result.Select(a => new AddressDTO(a));
            return Ok(mappedResult);
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDTO>> GetAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id).ConfigureAwait(false);

            if (address == null)
            {
                return NotFound();
            }

            return new AddressDTO(address);
        }

        // PUT: api/Addresses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, [FromBody]AddressDTO address)
        {
            if (id != address.Id)
            {
                return BadRequest();
            }

            var savedAddress = await _context.Set<Address>().FindAsync(id).ConfigureAwait(false);
            savedAddress.AddressLine = address.AddressLine;
            savedAddress.AddressLine2 = address.AddressLine2;
            savedAddress.City = address.City;
            savedAddress.Province = address.Province;

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
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress([FromBody]AddressDTO address)
        {
            var newAddress = new Address()
            {
                AddressLine = address.AddressLine,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                Province = address.Province
            };

            if (AddressDuplicated(newAddress))
                return BadRequest("Address already exists");

            _context.Addresses.Add(newAddress);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return CreatedAtAction("GetAddress", new { id = address.Id }, address);
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
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
            e.City == address.City &&
            e.Province == address.Province);
        }
    }
}
