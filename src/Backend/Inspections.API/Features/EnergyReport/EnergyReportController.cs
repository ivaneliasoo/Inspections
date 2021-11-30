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
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;

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

            if (prev == null) {
                _context.Add(t);
            } else {
                t.templateDef = categories;
                _context.Update(t);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // [HttpPost("category")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesDefaultResponseType]
        // public async Task<IActionResult> UpdateCategories()
        // {
        //     var deserializedContent = await JsonSerializer.DeserializeAsync<object>(Request.Body);

        //     using var fileStream = System.IO.File.Create(CategoriesFilePath);
        //     await JsonSerializer.SerializeAsync(fileStream, deserializedContent, typeof(object), new JsonSerializerOptions { WriteIndented = true });

        //     return NoContent();
        // }

        //[HttpPost("category/migrate")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> migrateCategories()
        //{
        //    Console.WriteLine("Se ha invocado migrateCategories");
            
        //    string? categories = CategoriesFromFile();

        //    Template t = new Template();
        //    t.id = 1;
        //    t.templateDef = categories;

        //    var prev = await _context.Template.Where(t => t.id == 1)
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync();

        //    if (prev == null) {
        //        Console.WriteLine("*** No template found in the database, copying template from file categories.json");
        //        _context.Add(t);
        //        await _context.SaveChangesAsync();
        //    }

        //    Console.WriteLine("Se ha ejecutado migrateCategories");

        //    return NoContent();
        //}


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

        //public string CategoriesFromFile()
        //{
        //    var reader = System.IO.File.OpenText(CategoriesFilePath);
        //    var fileContent = reader.ReadToEnd();

        //    return fileContent;
        //}

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
            // this propoerties are used for audit (or that was the original intention haha).
            // in the future this properties can be skiped in the entity's OnModelCreating override
            // var results = await _context.CurrentTable
            //     .FromSqlRaw(@"SELECT id, circuit, start_date, end_date, current_data, ""LastEdit"", ""LastEditUser""
            //         FROM current WHERE start_date = {0} AND end_date = {1}", startDate, endDate)
            //     .AsNoTracking().ToListAsync();

            var results = await _context.CurrentTable.Where(c => 
                c.startDate == startDate && 
                // trancking entities is an expensive task so adds AsNoTracking() to speed up
                c.endDate == endDate)
                    .AsNoTracking()
                    .ToListAsync();

            return Ok(results);
        }

        // POST: api/current-table
        [HttpPost("current-table")]
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
           
            if (currentTable == null) {
                return BadRequest();
            }

            var prev = await _context.CurrentTable.Where(c => 
                c.circuit == currentTable.circuit && 
                c.startDate == currentTable.startDate && 
                // trancking entities is an expensive task so adds AsNoTracking() to speed up
                c.endDate == currentTable.endDate)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            if (prev == null) {
                _context.Add(currentTable);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }        

    }
}
