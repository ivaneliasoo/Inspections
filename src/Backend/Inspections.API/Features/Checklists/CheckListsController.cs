using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateCheckList([FromBody] AddCheckListCommand checkList)
        {
            if (await _mediator.Send(checkList).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPost("{id:int}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddCheckListItem(int id, [FromBody] AddCheckListItemCommand item)
        {
            if (id != item.IdCheckList)
                return BadRequest();

            if (await _mediator.Send(item).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPost("{id:int}/params")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddCheckListParam([FromBody] AddCheckListParamCommand param)
        {
            if (await _mediator.Send(param).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPost("{id:int}/items/{idItem:int}/params")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddCheckListItemParam([FromBody] AddCheckListItemParamCommand param)
        {
            if (await _mediator.Send(param).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCheckList(int id, [FromBody] UpdateCheckListCommand checkList)
        {
            if (id != checkList.IdCheckList)
                return BadRequest();

            if (await _mediator.Send(checkList).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}/items/{idItem:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCheckListItem(int id, int idItem, [FromBody] UpdateCheckListItemCommand item)
        {
            if (id != item.CheckListId || idItem != item.Id)
                return BadRequest();

            if (await _mediator.Send(item).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}/params/{idParam:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCheckListParam(int id, int idParam, [FromBody] UpdateCheckListParamCommand param)
        {
            if (id != param.IdCheckList || idParam != param.Id)
                return BadRequest();

            if (await _mediator.Send(param).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}/items/{idItem:int}/params")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCheckListItemParam(int id, int idItem, int idParam, [FromBody] UpdateCheckListParamCommand param)
        {
            if (idParam != param.Id || idItem != param.IdCheckListItem || param.IdCheckList != id)
                return BadRequest();

            if (await _mediator.Send(param).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCheckList(int id)
        {
            if (await _mediator.Send(new DeleteCheckListCommand(id)).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}/items/{idItem:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCheckListItem(int id, int idItem)
        {
            if (await _mediator.Send(new DeleteCheckListItem(id, idItem)).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}/params/{idParam:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCheckListParam(int id, int idParam)
        {
            if (await _mediator.Send(new DeleteCheckListParamCommand(id, 0, idParam)).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}/items/{idItem:int}/params")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCheckListItemParam(int id, int idItem, int idParam)
        {
            if (await _mediator.Send(new DeleteCheckListParamCommand(id, idItem, idParam)).ConfigureAwait(false))
                return Ok();

            return BadRequest();
        }

        [HttpGet("{id:int}")]
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult GetCheckList(string filter)
        {
            var checkList = _checkListsQueries.GetByFilter(filter);

            if (checkList is null)
                return NotFound();

            return Ok(checkList);

        }
    }
}
