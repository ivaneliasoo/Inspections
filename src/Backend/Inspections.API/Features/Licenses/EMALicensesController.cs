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

            var license = new EMALicense
            {
                Id = eMALicense.LicenseId,
                Number = eMALicense.Number,
                Validity = eMALicense.Validity
            };

            _context.Entry(license).State = EntityState.Modified;

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
        public async Task<ActionResult<EMALicense>> PostEMALicense(LicenseDTO eMALicense)
        {
            var license = new EMALicense
            {
                Id = eMALicense.LicenseId,
                Number = eMALicense.Number,
                Validity = eMALicense.Validity
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

        private bool EMALicenseExists(int id)
        {
            return _context.Licenses.Any(e => e.Id == id);
        }
    }
}
