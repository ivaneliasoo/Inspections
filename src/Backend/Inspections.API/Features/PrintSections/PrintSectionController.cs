using Ardalis.GuardClauses;
using Inspections.API.Features.PrintSections.Commands;
using Inspections.API.Features.PrintSections.Models;
using Inspections.Core.Interfaces.Queries;
using Inspections.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inspections.API.Features.PrintSections
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrintSectionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPrintSectionRepository _printSectionsRepository;
        private readonly IPrintSectionsQueries _printSectionsQueries;


        public PrintSectionController(IMediator mediator
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
        public async Task<IActionResult> CreatePrintSection([FromBody] AddPrintSectionCommand printSection)
        {
            if (await _mediator.Send(printSection).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdatePrintSection(int id, [FromBody] EditPrintSectionCommand printSection)
        {
            Guard.Against.Null(printSection, nameof(printSection));
            if (id != printSection.Id)
                return BadRequest();

            if (await _mediator.Send(printSection).ConfigureAwait(false))
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
            var printSection = await _printSectionsRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (printSection is null)
                return NotFound();

            return Ok(new PrintSectionDto(printSection));

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<PrintSectionDto>>> GetPrintSections(string? filter)
        {
            var printSections = (await _printSectionsQueries.GetAllAsync(filter ?? "").ConfigureAwait(false)).ToList();

            if (!printSections.Any())
                return NoContent();

            return Ok(printSections.Select(x => new PrintSectionDto(x)));
        }

    }
}
