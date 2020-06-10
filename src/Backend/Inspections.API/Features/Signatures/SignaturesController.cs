using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.API.Features.Signatures.Commands;
using Inspections.Core.Interfaces;
using Inspections.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inspections.API.Features.Signatures
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SignaturesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISignaturesRepository _signaturesRepository;

        public SignaturesController(IMediator mediator, ISignaturesRepository signaturesRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _signaturesRepository = signaturesRepository ?? throw new ArgumentNullException(nameof(signaturesRepository));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateCheckList([FromBody] AddSignatureCommand signature)
        {
            if (await _mediator.Send(signature).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCheckList(int id, [FromBody] EditSignatureCommand signature)
        {
            if (id != signature.Id)
                return BadRequest();

            if (await _mediator.Send(signature).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCheckList(int id)
        {
            if (await _mediator.Send(new DeleteSignatureCommand(id)).ConfigureAwait(false))
                return NoContent();

            return NoContent();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCheckListById(int id)
        {
            var signature = await _signaturesRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (signature is null)
                return NotFound();

            return Ok(signature);

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult GetCheckList(string filter)
        {
            //var signature = _signaturesQueries.GetByFilter(filter);

            //if (signature is null)

            //return Ok(signature);

            return NotFound();
        }
    }
}
