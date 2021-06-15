﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.ApplicationServices;
using Inspections.API.Features.Inspections.Commands;
using Inspections.API.Features.Reports.Commands;
using Inspections.API.Models.Configuration;
using Inspections.Core.Domain.ReportsAggregate;
using Inspections.Core.Interfaces;
using Inspections.Core.QueryModels;
using Inspections.Infrastructure.Data;
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
        private readonly InspectionsContext _context;
        private readonly IOptions<ClientSettings> storageOptions;

        public ReportsController(IMediator mediator, IReportsRepository reportsRepository, InspectionsContext context, IOptions<ClientSettings> storageOptions)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _reportsRepository = reportsRepository ?? throw new ArgumentNullException(nameof(reportsRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.storageOptions = storageOptions ?? throw new ArgumentNullException(nameof(storageOptions));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] CreateReportCommand createData)
        {
            var result = await _mediator.Send(createData).ConfigureAwait(false);
            if (result <= 0)
                return BadRequest();

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromBody] UpdateReportCommand udpateData)
        {
            var result = await _mediator.Send(udpateData).ConfigureAwait(false);
            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAll(string? filter, bool? closed, bool myReports = true)
        {
            var result = await _reportsRepository.GetAll(filter, closed, myReports).ConfigureAwait(false);
            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReportQueryResult), 200)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetReport(int id)
        {
            var result = await _reportsRepository.GetByIdAsync(id, true).ConfigureAwait(false);

            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var result = await _mediator.Send(new DeleteReportCommand(id)).ConfigureAwait(false);
            if (!result)
                return BadRequest();

            return NoContent();
        }

        [HttpGet("{id:int}/photorecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult GetPhotoRecords(int id)
        {
            var photos = _context.Set<PhotoRecord>().Where(p => p.ReportId == id)
                            .Select(p => new
                            {
                                p.Label,
                                p.FileName,
                                Base64String = ToBase64String($"{Directory.GetCurrentDirectory()}{p.FileNameResized.Replace("ReportsImages", storageOptions.Value.ReportsImagesFolder, StringComparison.InvariantCultureIgnoreCase)}"),
                            });
            if (photos != null)
                return Ok(photos);

            return BadRequest();
        }

        private static string ToBase64String(string fileName)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
            return "data:image/png;base64," + Convert.ToBase64String(fileBytes);
        }

        [HttpPost("{id:int}/photorecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddNote(int id, [FromBody] AddNoteCommand note)
        {
            Guard.Against.Null(note, nameof(note));
            if (id != note.ReportId)
                return BadRequest();

            var result = await _mediator.Send(note).ConfigureAwait(false);
            if (result > 0)
                return Ok(result);

            return BadRequest();
        }

        [HttpPut("{id:int}/note/{idNote:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EditNote(int id, int idNote, [FromBody] EditNoteCommand note)
        {
            Guard.Against.Null(note, nameof(note));
            if (id != note.ReportId || idNote != note.Id)
                return BadRequest();

            var result = await _mediator.Send(note).ConfigureAwait(false);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpPut("{id:int}/photorecord/{idPhoto:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EditPhotoRecord(int id, int idPhoto, [FromBody] EditPhotoRecordCommand photo)
        {
            Guard.Against.Null(photo, nameof(photo));

            if (photo.Id != idPhoto || photo.ReportId != id)
                return BadRequest();

            var result = await _mediator.Send(new EditPhotoRecordCommand(photo.ReportId, photo.Label, photo.Id)).ConfigureAwait(false);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}/photorecord/{idPhoto:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeletePhotoRecord(int id, int idPhoto)
        {
            var result = await _mediator.Send(new DeletePhotoRecordCommand(idPhoto, id)).ConfigureAwait(false);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:int}/note/{idNote:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteNote(int id, int idNote)
        {
            var result = await _mediator.Send(new DeleteNoteCommand(idNote, id)).ConfigureAwait(false);
            if (result)
                return Ok();

            return BadRequest();
        }

        [HttpPatch("{reportId:int}/checklists/{checkListId:int}", Name = nameof(BulkUpdateChecks))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<bool>> BulkUpdateChecks(int reportId, int checkListId, int newValue)
        {
            var result = await _mediator.Send(new BulkUpdateCheckItemsCommand(reportId, checkListId, newValue)).ConfigureAwait(false);
            if (!result)
                return Conflict();

            return NoContent();
        }

        [HttpPatch("{reportId:int}/complete", Name = nameof(CompleteReport))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<bool>> CompleteReport(int reportId)
        {
            var result = await _mediator.Send(new CompleteReportCommand(reportId)).ConfigureAwait(false);
            if (!result)
                return Conflict();

            return NoContent();
        }

        [HttpPut("{id:int}/readings", Name = nameof(AddOperationalReadings))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> AddOperationalReadings(int id, [FromBody] UpdateOperationalReadingsCommand operationalReadingsCommand)
        {
            Guard.Against.Null(operationalReadingsCommand, nameof(operationalReadingsCommand));
            if (id != operationalReadingsCommand.ReportId)
                return BadRequest();

            var result = await _mediator.Send(operationalReadingsCommand).ConfigureAwait(false);
            if (result)
                return Ok(result);

            return BadRequest();
        }
    }
}
