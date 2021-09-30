using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inspections.API.Features.EnergyReport.Models;
using Inspections.Infrastructure.Data;

namespace Inspections.API.Features.EnergyReport
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyReportController : ControllerBase
    {
        // private readonly InspectionsContext _context;
        private readonly EnergyReportContext _context;

        public EnergyReportController(EnergyReportContext context)
        {
            _context = context;
        }        

        [HttpPost("category")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCategories()
        {
            var deserializedContent = await JsonSerializer.DeserializeAsync<object>(Request.Body);

            using var fileStream = System.IO.File.Create(CategoriesFilePath);
            await JsonSerializer.SerializeAsync(fileStream, deserializedContent, typeof(object), new JsonSerializerOptions { WriteIndented = true });

            return NoContent();
        }

        [HttpGet("category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Categories()
        {
            using var reader = System.IO.File.OpenText(CategoriesFilePath);
            var fileContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(fileContent))
                return NotFound();

            return Ok(fileContent);
        }

        private static string CategoriesFilePath => Path.Combine(AppContext.BaseDirectory, "categories.json");
        private static string LogoPath => Path.Combine(AppContext.BaseDirectory, "cse-logo.svg");

        [HttpGet("background")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Background()
        {
            using var reader = System.IO.File.OpenText(LogoPath);
            var fileContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(fileContent))
                return NotFound();

            return Ok(fileContent);
        }

        // GET: api/current-table/startdate/enddate
        [HttpGet("current-table/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<CurrentTable>>> GetCurrentTable(string startDate, string endDate)
        {
            Console.WriteLine("/api/current-table");
            var results = await _context.CurrentTable
                .FromSqlRaw("SELECT id, circuit, start_date, end_date, current_data " +
                    "FROM current WHERE start_date = {0} AND end_date = {1}", startDate, endDate)
                .ToListAsync();

            return Ok(results);
        }

        // POST: api/current-table
        [HttpPost("current-table")]
        public async Task<IActionResult> SetCurrentTable(CurrentTable currentTable)
        {
            if (currentTable == null) {
                return BadRequest();
            }

            var prev = await _context.CurrentTable.Where(c => 
                c.circuit == currentTable.circuit && 
                c.startDate == currentTable.startDate && 
                c.endDate == currentTable.endDate).FirstOrDefaultAsync();

            if (prev == null) {
                _context.Add(currentTable);
            } else {
                _context.Update(prev);
            }

            _context.SaveChanges();
            return NoContent();
        }

    }
}
