using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.PrintSections.Commands;
using Inspections.API.Features.PrintSections.Models;
using Inspections.Core.Interfaces;
using Inspections.Core.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inspections.API.Features.PrintSections
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrintSectionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPrintSectionRepository _printSectionsRepository;
        private readonly IPrintSectionsQueries _printSectionsQueries;


        public PrintSectionsController(IMediator mediator
            , IPrintSectionRepository printSectionsRepository
            , IPrintSectionsQueries printSectionsQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _printSectionsRepository = printSectionsRepository ?? throw new ArgumentNullException(nameof(printSectionsRepository));
            _printSectionsQueries = printSectionsQueries ?? throw new ArgumentNullException(nameof(printSectionsQueries));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreatePrintSection([FromBody] AddPrintSectionCommand PrintSection)
        {
            if (await _mediator.Send(PrintSection).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdatePrintSection(int id, [FromBody] EditPrintSectionCommand PrintSection)
        {
            Guard.Against.Null(PrintSection, nameof(PrintSection));
            if (id != PrintSection.Id)
                return BadRequest();

            if (await _mediator.Send(PrintSection).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePrintSection(int id)
        {
            if (await _mediator.Send(new DeletePrintSectionCommand(id)).ConfigureAwait(false))
                return NoContent();

            return NoContent();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetPrintSectionById(int id)
        {
            var PrintSection = await _printSectionsRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (PrintSection is null)
                return NotFound();

            return Ok(new PrintSectionDTO(PrintSection));

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<PrintSectionDTO>>> GetPrintSections(string? filter)
        {
            var printSections = await _printSectionsQueries.GetAllAsync(filter ?? "").ConfigureAwait(false);

            if (printSections is null)
                return NoContent();

            return Ok(printSections.Select(x => new PrintSectionDTO(x)));
        }

    }
}
