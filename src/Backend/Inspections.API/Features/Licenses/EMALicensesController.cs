using Ardalis.GuardClauses;
using Inspections.API.Features.Licenses.Models;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Licenses;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EMALicensesController : ControllerBase
{
    private readonly InspectionsContext _context;

    public EMALicensesController(InspectionsContext context)
    {
        _context = context;
    }

    // GET: api/EMALicenses
    [HttpGet(Name = "GetLicenses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetLicenses()
    {
        var result = await _context.Licenses.ToListAsync().ConfigureAwait(false);
        if (!result.Any())
            return NoContent();

        return Ok(result.Select(l => new LicenseDTO(l)).ToList());
    }

    // GET: api/EMALicenses/5
    [HttpGet("{id}", Name = "GetLicense")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<LicenseDTO>> GetEMALicense(int id)
    {
        var eMALicense = await _context.Licenses.FindAsync(id).ConfigureAwait(false);

        if (eMALicense == null)
        {
            return NotFound();
        }

        return new LicenseDTO(eMALicense);
    }

    // PUT: api/EMALicenses/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}", Name = "UpdateLicense")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> PutEMALicense(int id, LicenseDTO eMALicense)
    {
        Guard.Against.Null(eMALicense, nameof(eMALicense));

        if (id != eMALicense.LicenseId)
        {
            return BadRequest();
        }

        var license = await _context.Licenses.FindAsync(id).ConfigureAwait(false);
            
        if (license is null) return NotFound();
            
        license.Id = eMALicense.LicenseId;
        license.Number = eMALicense.Number;
        license.Name = eMALicense.Name;
        license.PersonInCharge = eMALicense.PersonInCharge;
        license.Contact = eMALicense.Contact;
        license.Email = eMALicense.Email;
        license.Amp = eMALicense.Amp;
        license.Volt = eMALicense.Volt;
        license.KVA = eMALicense.KVA;
        license.Validity = new Shared.DateTimeRange { Start = eMALicense.ValidityStart, End = eMALicense.ValidityEnd };

        _context.Entry(license).State = EntityState.Modified;
        _context.Entry(license.Validity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EMALicenseExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/EMALicenses
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost(Name = "AddLicense")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<EMALicense>> PostEMALicense([FromBody] LicenseDTO eMaLicense)
    {
        Guard.Against.Null(eMaLicense, nameof(eMaLicense));

        var license = new EMALicense
        {
            Id = eMaLicense.LicenseId,
            Number = eMaLicense.Number,
            Name = eMaLicense.Name,
            PersonInCharge = eMaLicense.PersonInCharge,
            Contact = eMaLicense.Contact,
            Email = eMaLicense.Email,
            Amp = eMaLicense.Amp,
            Volt = eMaLicense.Volt,
            KVA = eMaLicense.KVA,
            Validity = new Shared.DateTimeRange { Start = eMaLicense.ValidityStart, End = eMaLicense.ValidityEnd }
        };
        _context.Licenses.Add(license);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return CreatedAtAction("PostEMALicense", new { id = eMaLicense.LicenseId }, eMaLicense);
    }

    // DELETE: api/EMALicenses/5
    [HttpDelete("{id}", Name = "DeleteLicense")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<EMALicense>> DeleteEMALicense(int id)
    {
        var emaLicense = await _context.Licenses.FindAsync(id).ConfigureAwait(false);
        if (emaLicense == null)
        {
            return NotFound();
        }

        _context.Licenses.Remove(emaLicense);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return emaLicense;
    }

    [HttpGet("dashboard", Name = "GetLicensesDashboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<LicenseDTO>>> GetLicensesDashboard()
    {
        var expiringLicenses = await _context.Licenses
            .Where(l => l.Validity.End.Year == DateTime.Now.Year
                        && l.Validity.End.Month == DateTime.Now.Month && l.Validity.End > DateTime.Now.Date)
            .Select(l => new LicenseDTO(l)).ToListAsync().ConfigureAwait(false);

        var expiredLicenses = await _context.Licenses
            .Where(l => l.Validity.End <= DateTime.Now.Date)
            .Select(l => new LicenseDTO(l)).ToListAsync().ConfigureAwait(false);

        if (!expiredLicenses.Any() && !expiringLicenses.Any())
            return NoContent();

        return Ok(new { expiring = expiringLicenses, expired = expiredLicenses });
    }

    private bool EMALicenseExists(int id)
    {
        return _context.Licenses.Any(e => e.Id == id);
    }
}
