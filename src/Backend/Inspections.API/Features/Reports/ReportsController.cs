﻿using System;
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromBody] UpdateReportCommand udpateData)
        {
            var result = await _mediator.Send(udpateData).ConfigureAwait(false);
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
        public async Task<IActionResult> AddPhotoRecord(int id, [FromForm] string label)
        {
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

        [HttpPost("{id:int}/note")]
        public async Task<IActionResult> AddNote(int id, [FromBody] AddNoteCommand note)
        {

            var result = await _mediator.Send(note).ConfigureAwait(false);
            if (result>0)
                return Ok(result);

            return BadRequest();
        }

        [HttpPut("{id:int}/note/{idNote:int}")]
        public async Task<IActionResult> EditNote(int id, int idNote, [FromBody] EditNoteCommand note)
        {
            if (id != note.ReportId || idNote != note.Id)
                return BadRequest();

            var result = await _mediator.Send(note).ConfigureAwait(false);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}/photorecord/{idPhoto:int}")]
        public async Task<IActionResult> EditPhotoRecord(int id, int idPhoto, [FromBody] EditPhotoRecordCommand photo)
        {
            if (photo.Id != idPhoto || photo.ReportId != id)
                return BadRequest();

            var result = await _mediator.Send(new EditPhotoRecordCommand(photo.ReportId,photo.Label,photo.Id)).ConfigureAwait(false);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}/photorecord/{idPhoto:int}")]
        public async Task<IActionResult> DeletePhotoRecord(int id, int idPhoto)
        {
            var result = await _mediator.Send(new DeletePhotoRecordCommand(idPhoto,id)).ConfigureAwait(false);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}/note/{idNote:int}")]
        public async Task<IActionResult> DeleteNote(int id, int idNote)
        {
            var result = await _mediator.Send(new DeleteNoteCommand(idNote,id)).ConfigureAwait(false);
            if (result)
                return Ok();

            return BadRequest();
        }

    }
}
