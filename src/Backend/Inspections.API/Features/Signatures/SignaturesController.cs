using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inspections.API.Features.Signatures.Commands;
using Inspections.API.Features.Signatures.Models;
using Inspections.Core.Interfaces;
using Inspections.Core.Interfaces.Queries;
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
        private readonly ISignaturesQueries _signaturesQueries;

        public SignaturesController(IMediator mediator
            , ISignaturesRepository signaturesRepository
            , ISignaturesQueries signaturesQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _signaturesRepository = signaturesRepository ?? throw new ArgumentNullException(nameof(signaturesRepository));
            _signaturesQueries = signaturesQueries ?? throw new ArgumentNullException(nameof(signaturesQueries));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateSignature([FromBody] AddSignatureCommand signature)
        {
            if (await _mediator.Send(signature).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateSignature(int id, [FromBody] EditSignatureCommand signature)
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
        public async Task<IActionResult> DeleteSignature(int id)
        {
            if (await _mediator.Send(new DeleteSignatureCommand(id)).ConfigureAwait(false))
                return NoContent();

            return NoContent();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetSignatureById(int id)
        {
            var signature = await _signaturesRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (signature is null)
                return NotFound();

            return Ok(new SignatureDTO(signature));

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<SignatureDTO>>> GetSignatures(string filter, bool? inConfigurationOnly, int? reportConfigurationId, int? reportId)
        {
            var signatures = await _signaturesQueries.GetAllAsync(filter, inConfigurationOnly, reportConfigurationId, reportId).ConfigureAwait(false);

            if (signatures is null)
                return NoContent();

            return Ok(signatures.Select(x => new SignatureDTO(x)));
        }
    }
}
