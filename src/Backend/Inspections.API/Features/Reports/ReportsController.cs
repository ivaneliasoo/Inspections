using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.API.Features.Inspections.Commands;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
