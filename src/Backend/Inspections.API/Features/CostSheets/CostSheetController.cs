using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Inspections.Core.Domain;
using Inspections.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Text.Json;

#nullable enable
namespace Inspections.API.Features.CostSheets
{
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Route("api/[controller]")]
    public class CostSheetController : ControllerBase
    {
        private readonly ILogger<CostSheetController> _logger;
        private readonly InspectionsContext _context;

        public CostSheetController(ILogger<CostSheetController> logger, InspectionsContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/costsheet/sheet
        [HttpGet("sheet")]
        public IActionResult GetCostSheets()
        {
            var list = _context.CostSheet.Select(x => new CostSheetDTO
            {
                id = x.id,
                project = x.project,
                location = x.location,
                dateCreated = x.dateCreated,
                isTemplate = x.isTemplate
            }).ToList();

            return Ok(list);
        }

        // GET: api/costsheet/sheet/{id}
        [HttpGet("sheet/{id}")]
        public async Task<ActionResult<CostSheet>> GetCostSheet(long id)
        {
            var costSheet = await _context.CostSheet.FindAsync(id);

            if (costSheet == null)
            {
                return NotFound();
            }

            return Ok(costSheet);
        }

        // POST: api/costsheet/sheet
        [HttpPost("sheet")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertCostSheet()
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
            CostSheet? costSheet = JsonSerializer.Deserialize<CostSheet>(body, options);

            if (costSheet == null)
            {
                return BadRequest();
            }

            costSheet.lastUpdate = DateTime.Now;
            costSheet.updated = false;
            _context.CostSheet.Add(costSheet);
            await _context.SaveChangesAsync();

            return Ok(costSheet);
        }

        // PUT: api/costsheet/sheet
        [HttpPut("sheet")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCostSheet()
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
            CostSheet? costSheet = JsonSerializer.Deserialize<CostSheet>(body, options);

            if (costSheet == null)
            {
                return BadRequest();
            }

            var prev = _context.CostSheet.Where(cs => cs.id == costSheet.id).FirstOrDefault();

            if (prev == null) {
                return NotFound();
            }

            var result = Math.Truncate((prev.lastUpdate - costSheet.lastUpdate).Value.TotalSeconds);
            if (result <= 0)
            {
                prev.id = costSheet.id;
                prev.project = costSheet.project;
                prev.location = costSheet.location;
                prev.isTemplate = costSheet.isTemplate;
                prev.dateCreated = costSheet.dateCreated;
                prev.materialMarkup = costSheet.materialMarkup;
                prev.labourDailyRate = costSheet.labourDailyRate;
                prev.labourNightMultiplier = costSheet.labourNightMultiplier;
                prev.finalMarkup = costSheet.finalMarkup;
                prev.sections = costSheet.sections;
                prev.lastUpdate = DateTime.Now;
                prev.updated = false;
                await _context.SaveChangesAsync();
            }

            return Ok(prev);
        }

        // DELETE: api/costsheet/sheet/{id}
        [HttpDelete("sheet/{id}")]
        public async Task<IActionResult> DeleteCostSheet(long id)
        {
            var todoItem = await _context.CostSheet.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.CostSheet.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/costsheet/template
        [HttpGet("template")]
        public IActionResult GetTemplates()
        {
            var list = _context.CSTemplate.Select(x => new CSTemplateDTO
            {
                id = x.id,
                project = x.project,
                location = x.location,
                dateCreated = x.dateCreated,
                isTemplate = x.isTemplate
            }).ToList();

            return Ok(list);
        }

        // GET: api/template/id
        [HttpGet("template/{id}")]
        public async Task<ActionResult<CSTemplate>> GetTemplate(long id)
        {
            var template = await _context.CSTemplate.FindAsync(id);

            if (template == null)
            {
                return NotFound();
            }

            return Ok(template);
        }

        // PUT: api/template
        [HttpPost("template")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> InsertTemplate()
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
            CSTemplate? template = JsonSerializer.Deserialize<CSTemplate>(body, options);

            if (template == null)
            {
                return BadRequest();
            }

            template.lastUpdate = DateTime.Now;
            template.updated = false;
            _context.CSTemplate.Add(template);
            await _context.SaveChangesAsync();

            return Ok(template);
        }

        // PUT: api/template
        [HttpPut("template")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTemplate()
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
            CSTemplate? template = JsonSerializer.Deserialize<CSTemplate>(body, options);

            if (template == null)
            {
                return BadRequest();
            }

            var prev = _context.CSTemplate.Where(cs => cs.id == template.id).FirstOrDefault();

            if (prev == null) {
                return NotFound();
            }

            var result = Math.Truncate((prev.lastUpdate - template.lastUpdate).Value.TotalSeconds);
            if (result <= 0)
            {
                prev.id = template.id;
                prev.project = template.project;
                prev.location = template.location;
                prev.isTemplate = template.isTemplate;
                prev.dateCreated = template.dateCreated;
                prev.materialMarkup = template.materialMarkup;
                prev.labourDailyRate = template.labourDailyRate;
                prev.labourNightMultiplier = template.labourNightMultiplier;
                prev.finalMarkup = template.finalMarkup;
                prev.sections = template.sections;
                prev.lastUpdate = DateTime.Now;
                prev.updated = false;
                await _context.SaveChangesAsync();
            }

            return Ok(prev);
        }

        // DELETE: api/template/{id}
        [HttpDelete("template/{id}")]
        public async Task<IActionResult> DeleteTemplate(long id)
        {
            var todoItem = await _context.CSTemplate.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.CSTemplate.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

    class CostSheetDTO {
        public long id { get; set; }

        public string? project { get; set; }

        public string? location { get; set; }

        public bool isTemplate { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? dateCreated { get; set; }
    }

    class CSTemplateDTO {
        public long id { get; set; }

        public string? project { get; set; }

        public string? location { get; set; }

        public bool isTemplate { get; set; }

        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? dateCreated { get; set; }
    }
}
