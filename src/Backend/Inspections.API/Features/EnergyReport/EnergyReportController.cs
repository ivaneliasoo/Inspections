using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inspections.API.Features.Energy
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyReportController : ControllerBase
    {
        [HttpPost("categories")]
        public async Task<IActionResult> UpdateCategories()
        {
            var deserializedContent = await JsonSerializer.DeserializeAsync<object>(Request.Body);

            using var fileStream = System.IO.File.Create(CategoriesFilePath);
            await JsonSerializer.SerializeAsync(fileStream, deserializedContent, typeof(object), new JsonSerializerOptions { WriteIndented = true });

            return NoContent();
        }

        [HttpGet("categories")]
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
        public async Task<IActionResult> Background()
        {
            using var reader = System.IO.File.OpenText(LogoPath);
            var fileContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(fileContent))
                return NotFound();

            return Ok(fileContent);
        }
    }
}
