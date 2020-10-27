using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Inspections.API.Features.Licenses.Models;

namespace Inspections.API.Features.Licenses
{
    [Route("[controller]")]
    [ApiController]
    public class EMALicensesController : ControllerBase
    {
        private readonly InspectionsContext _context;

        public EMALicensesController(InspectionsContext context)
        {
            _context = context;
        }

        // GET: api/EMALicenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LicenseDTO>>> GetLicenses()
        {
            var result = await _context.Licenses.ToListAsync().ConfigureAwait(false);
            return result.Select(l => new LicenseDTO(l)).ToList();
        }

        // GET: api/EMALicenses/5
        [HttpGet("{id}")]
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEMALicense(int id, LicenseDTO eMALicense)
        {
            if (id != eMALicense.LicenseId)
            {
                return BadRequest();
            }

            var license = await _context.Licenses.FindAsync(id);
            license.Id = eMALicense.LicenseId;
            license.Number = eMALicense.Number;
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
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EMALicenses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EMALicense>> PostEMALicense([FromBody]LicenseDTO eMALicense)
        {
            var license = new EMALicense
            {
                Id = eMALicense.LicenseId,
                Number = eMALicense.Number,
                Validity = new Shared.DateTimeRange { Start = eMALicense.ValidityStart, End = eMALicense.ValidityEnd }
            };
            _context.Licenses.Add(license);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return CreatedAtAction("PostEMALicense", new { id = eMALicense.LicenseId }, eMALicense);
        }

        // DELETE: api/EMALicenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EMALicense>> DeleteEMALicense(int id)
        {
            var eMALicense = await _context.Licenses.FindAsync(id).ConfigureAwait(false);
            if (eMALicense == null)
            {
                return NotFound();
            }

            _context.Licenses.Remove(eMALicense);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return eMALicense;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<IEnumerable<LicenseDTO>>> GetLicensesDashboard()
        {
            var expiringLicenses = await _context.Licenses
                .Where(l => l.Validity.End.Year == DateTime.Now.Year 
                            && l.Validity.End.Month == DateTime.Now.Month && l.Validity.End > DateTime.Now.Date)
                .Select(l => new LicenseDTO(l)).ToListAsync().ConfigureAwait(false);

            var expiredLicenses = await _context.Licenses
                .Where(l => l.Validity.End <= DateTime.Now.Date)
                .Select(l => new LicenseDTO(l)).ToListAsync().ConfigureAwait(false);

            return Ok(new { expiring = expiringLicenses, expired = expiredLicenses });
        }

        private bool EMALicenseExists(int id)
        {
            return _context.Licenses.Any(e => e.Id == id);
        }
    }
}
