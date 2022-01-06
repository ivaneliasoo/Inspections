using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Interfaces;
using Inspections.Core.Interfaces.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inspections.API.Features.Checklists
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckListsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICheckListsRepository _checkListsRepository;
        private readonly ICheckListsQueries _checkListsQueries;

        public CheckListsController(IMediator mediator, ICheckListsRepository checkListsRepository, ICheckListsQueries checkListsQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
            _checkListsQueries = checkListsQueries ?? throw new ArgumentNullException(nameof(checkListsQueries));
        }

        [HttpPost(Name = "CreateCheckList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateCheckList([FromBody] AddCheckListCommand checkList)
        {
            if (await _mediator.Send(checkList).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPost("{id:int}/items", Name = "AddItemToChecklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddCheckListItem(int id, [FromBody] AddCheckListItemCommand item)
        {
            Guard.Against.Null(item, nameof(item));
            if (id != item.IdCheckList)
                return BadRequest();

            if (await _mediator.Send(item).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}", Name = "UpdateChecklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCheckList(int id, [FromBody] UpdateCheckListCommand checkList)
        {
            Guard.Against.Null(checkList, nameof(checkList));
            if (id != checkList.IdCheckList)
                return BadRequest();

            if (await _mediator.Send(checkList).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}/items/{idItem:int}", Name = "UpdateChecklistItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCheckListItem(int id, int idItem, [FromBody] UpdateCheckListItemCommand item)
        {
            Guard.Against.Null(item, nameof(item));
            if (id != item.CheckListId || idItem != item.Id)
                return BadRequest();

            if (await _mediator.Send(item).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}", Name = "DeleteChecklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCheckList(int id)
        {
            if (await _mediator.Send(new DeleteCheckListCommand(id)).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}/items/{idItem:int}", Name = "DeleteChecklistItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCheckListItem(int id, int idItem)
        {
            if (await _mediator.Send(new DeleteCheckListItemCommand(id, idItem)).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpGet("{id:int}", Name = "GetCheckListbyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCheckListById(int id)
        {
            var checkList = await _checkListsRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (checkList is null)
                return NotFound();

            return Ok(checkList);

        }

        [HttpGet(Name = "GetChecklists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCheckList(string? filter, int? reportConfigurationId, int? reportId, bool? inConfigurationOnly = null)
        {
            var checkList = await _checkListsQueries.GetByFilter(filter, inConfigurationOnly, reportConfigurationId, reportId);

            if (checkList is null)
                return NotFound();

            return Ok(checkList);

        }
    }
}
