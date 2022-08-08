using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace Inspections.API.Features.EnergyReport
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyReportController : ControllerBase
    {
        // private readonly InspectionsContext _context;
        private readonly InspectionsContext _context;

        public EnergyReportController(InspectionsContext context)
        {
            _context = context;
        }

        [HttpPost("category")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCategories()
        {
            //var categories = await JsonSerializer.DeserializeAsync<object>(Request.Body);

            if (!this.Request.Body.CanSeek)
            {
                this.Request.EnableBuffering();
            }

            this.Request.Body.Position = 0;
            var reader = new StreamReader(this.Request.Body, Encoding.UTF8);
            var categories = await reader.ReadToEndAsync().ConfigureAwait(false);
            this.Request.Body.Position = 0;

            Template t = new Template();
            t.id = 1;
            t.templateDef = categories;

            var prev = await _context.Template.Where(t => t.id == 1)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (prev == null)
            {
                _context.Add(t);
            }
            else
            {
                t.templateDef = categories;
                _context.Update(t);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CategoriesFromFile()
        {
            using var reader = System.IO.File.OpenText(CategoriesFilePath);
            var fileContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(fileContent))
                return NotFound();

            return Ok(fileContent);
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

            var results = await _context.Template.Where(t => t.id == 1)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return Ok(results.templateDef);
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
            // all entities in InspectionsContext has 2 shadow properties: LastEdit, LastEditUser.
            // this properties are used for audit (or that was the original intention haha).
            // in the future this properties can be skipped in the entity's OnModelCreating override
            // var results = await _context.CurrentTable
            //     .FromSqlRaw(@"SELECT id, circuit, start_date, end_date, current_data, ""LastEdit"", ""LastEditUser""
            //         FROM current WHERE start_date = {0} AND end_date = {1}", startDate, endDate)
            //     .AsNoTracking().ToListAsync();

            var results = await _context.CurrentTable.Where(c =>
                c.startDate == startDate &&
                // tracking entities is an expensive task so adds AsNoTracking() to speed up
                c.endDate == endDate)
                    .AsNoTracking()
                    .ToListAsync();

            return Ok(results);
        }

        // POST: api/current-table
        [HttpPost("current-table")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SetCurrentTable()
        {
            if (!this.Request.Body.CanSeek)
            {
                this.Request.EnableBuffering();
            }

            this.Request.Body.Position = 0;
            var reader = new StreamReader(this.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            this.Request.Body.Position = 0;
            var options = new JsonSerializerOptions();
            CurrentTable? currentTable = JsonSerializer.Deserialize<CurrentTable>(body, options);

            if (currentTable == null)
            {
                return BadRequest();
            }

            var prev = await _context.CurrentTable.Where(c =>
                c.circuit == currentTable.circuit &&
                c.startDate == currentTable.startDate &&
                // tracking entities is an expensive task so adds AsNoTracking() to speed up
                c.endDate == currentTable.endDate)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            if (prev == null)
            {
                _context.Add(currentTable);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        // GET: api/report/{id}
        [HttpGet("report")]
        public IActionResult GetPowerAnalyzerReports()
        {
            var list = _context.PowerAnalyzerReport.Select(x => new PowerAnalyzerReportDTO
            {
                id = x.id,
                name = x.name,
                location = x.location,
                template = x.template,
                circuit = x.circuit,
                dateCreated = x.dateCreated
            }).OrderBy(x => x.id).ToList();

            return Ok(list);
        }

        // GET: api/energy-report/{id}
        [HttpGet("report/{id}")]
        public async Task<ActionResult<PowerAnalyzerReport>> GetPowerAnalyzerReport(long id)
        {
            var paReport = await _context.PowerAnalyzerReport.FindAsync(id);

            if (paReport == null)
            {
                return NotFound();
            }

            return Ok(paReport);
        }

        // POST: api/energy-report
        [HttpPost("report")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> InsertPowerAnalyzerReport()
        {
            if (!this.Request.Body.CanSeek)
            {
                this.Request.EnableBuffering();
            }

            this.Request.Body.Position = 0;
            var reader = new StreamReader(this.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            this.Request.Body.Position = 0;
            var options = new JsonSerializerOptions();
            PowerAnalyzerReport? paReport = JsonSerializer.Deserialize<PowerAnalyzerReport>(body, options);

            if (paReport == null)
            {
                return BadRequest();
            }

            paReport.lastUpdate = DateTime.Now;
            paReport.updated = false;
            _context.PowerAnalyzerReport.Add(paReport);
            await _context.SaveChangesAsync();

            return Ok(paReport);
        }

        // PUT: api/energy-report
        [HttpPut("report")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdatePowerAnalyzerReport()
        {
            if (!this.Request.Body.CanSeek)
            {
                this.Request.EnableBuffering();
            }

            this.Request.Body.Position = 0;
            var reader = new StreamReader(this.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            this.Request.Body.Position = 0;
            var options = new JsonSerializerOptions();
            PowerAnalyzerReport? paReport = JsonSerializer.Deserialize<PowerAnalyzerReport>(body, options);

            if (paReport == null)
            {
                return BadRequest();
            }

            var prev = _context.PowerAnalyzerReport.Where(cs => cs.id == paReport.id).FirstOrDefault();

            if (prev == null) {
                return NotFound();
            }

            var result = Math.Truncate((prev.lastUpdate - paReport.lastUpdate).Value.TotalSeconds);
            if (result <= 0)
            {
                prev.id = paReport.id;
                prev.name = paReport.name;
                prev.location = paReport.location;
                prev.template = paReport.template;
                prev.fileName = paReport.fileName;
                prev.circuit = paReport.circuit;
                prev.dateCreated = paReport.dateCreated;
                prev.chartLegendOption = paReport.chartLegendOption;
                prev.cover = paReport.cover;
                prev.rawCsvData = paReport.rawCsvData;
                // prev.csvColumns = paReport.csvColumns;
                prev.lastUpdate = DateTime.Now;
                prev.updated = false;
                await _context.SaveChangesAsync();
            }

            return Ok(prev);
        }

        // DELETE: api/energy-report/{id}
        [HttpDelete("report/{id}")]
        public async Task<IActionResult> DeletePowerAnalyzerReport(long id)
        {
            var todoItem = await _context.PowerAnalyzerReport.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.PowerAnalyzerReport.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    class PowerAnalyzerReportDTO {
        public long id { get; set; }

        public string? name { get; set; }

        public string? location { get; set; }

        public string? circuit { get; set; }

        public int template { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? dateCreated { get; set; }
    }
}
