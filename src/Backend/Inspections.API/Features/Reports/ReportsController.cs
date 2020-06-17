using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.API.Features.Inspections.Commands;
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

        public ReportsController(IMediator mediator)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Create createData)
        {
            var result = await _mediator.Send(createData).ConfigureAwait(false);
            if (!result)
                return BadRequest();

            return Ok();
        }
    }
}
