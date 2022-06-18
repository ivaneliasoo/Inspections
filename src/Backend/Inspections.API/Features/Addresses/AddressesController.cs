using Ardalis.GuardClauses;
using Inspections.API.Features.Addresses.Models;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Addresses;

[Authorize]
[Route("api/[controller]")]
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
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddresses(string? filter)
    {
        var result = await _context.Addresses
            .Where(ad => EF.Functions.Like(ad.AddressLine, $"%{filter}%") ||
                         EF.Functions.Like(ad.Unit, $"%{filter}%") ||
                         EF.Functions.Like(ad.Country, $"%{filter}%") ||
                         EF.Functions.Like(ad.PostalCode, $"%{filter}%") ||
                         EF.Functions.Like(ad.License!.Number, $"%{filter}%"))
            .Select(a => new AddressDto(a))
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);
        var mappedResult = result;
        return Ok(mappedResult);
    }

    // GET: api/Addresses/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<AddressDto>> GetAddress(int id)
    {
        var address = await _context.Addresses.AsNoTracking().Where(a => a.Id == id).Select(ad => new AddressDto(ad)).SingleOrDefaultAsync();

        if (address == null)
        {
            return NotFound();
        }

        return address;
    }

    // PUT: api/Addresses/5
    [HttpPut("{id}", Name = "UpdateAddress")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> PutAddress(int id, [FromBody] NewAddressDto address)
    {
        Guard.Against.Null(address, nameof(address));

        if (id != address.Id)
        {
            return BadRequest();
        }

        var savedAddress = await _context.Set<Address>().FindAsync(id).ConfigureAwait(false);
            
        if (savedAddress == null)
        {
            return NotFound("address not found");
        }
            
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Address>> PostAddress([FromBody] NewAddressDto address)
    {
        Guard.Against.Null(address, nameof(address));

        var newAddress = address.ToAddress();

        if (AddressDuplicated(newAddress))
            return BadRequest("Address already exists");

        _context.Addresses.Add(newAddress);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return CreatedAtAction("GetAddress", new { id = address.Id }, new AddressDto(newAddress));
    }

    // DELETE: api/Addresses/5
    [HttpDelete("{id}", Name = "DeleteAddress")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<AddressDto>> DeleteAddress(int id)
    {
        var address = await _context.Addresses.FindAsync(id).ConfigureAwait(false);
        if (address == null)
        {
            return NotFound();
        }

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return new AddressDto(address);
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
