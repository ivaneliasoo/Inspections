using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Inspections.Commands;
using Inspections.API.Features.Reports.Commands;
using Inspections.API.Models.Configuration;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inspections.API.Features.Inspections
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReportsRepository _reportsRepository;

        public ReportsController(IMediator mediator, IReportsRepository reportsRepository)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this._reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateReportCommand createData)
        {
            var result = await _mediator.Send(createData).ConfigureAwait(false);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string filter)
        {
            var result = await _reportsRepository.GetAll(filter).ConfigureAwait(false);
            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReport(int id)
        {
            var result = await _reportsRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var result = await _mediator.Send(new DeleteReportCommand(id)).ConfigureAwait(false);
            if (!result)
                return BadRequest();

            return NoContent();
        }

        [HttpPost("{id:int}/photorecord")]
        public async Task<IActionResult> AddPhotoRecord(int id, [FromForm] string label)        {
            var request = await Request.ReadFormAsync().ConfigureAwait(false);

            if (!request.Files.Any())
                return BadRequest("can't find a file in the request");

            if (request != null)
            {
                var result = await _mediator.Send(new AddPhotoRecordCommand(id, request.Files, label)).ConfigureAwait(false);
                if (result)
                    return Ok();
            }

            return BadRequest();
        }

    }
}
