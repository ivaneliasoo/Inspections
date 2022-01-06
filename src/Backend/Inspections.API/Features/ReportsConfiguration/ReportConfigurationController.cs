using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.API.Features.ReportsConfiguration.Model;
using Inspections.Core.Interfaces;
using Inspections.Core.Interfaces.Queries;
using Inspections.Core.QueryModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inspections.API.Features.ReportsConfiguration
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportConfigurationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReportConfigurationsRepository _reportConfigurationsRepository;
        private readonly IReportConfigurationsQueries _reportConfigsQueries;

        public ReportConfigurationController(IMediator mediator
            , IReportConfigurationsRepository reportConfigurationsRepository
            , IReportConfigurationsQueries reportConfigsQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _reportConfigurationsRepository = reportConfigurationsRepository ?? throw new ArgumentNullException(nameof(reportConfigurationsRepository));
            _reportConfigsQueries = reportConfigsQueries ?? throw new ArgumentNullException(nameof(reportConfigsQueries));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateReportConfig([FromBody] AddReportConfigurationCommand report)
        {
            var result = await _mediator.Send(report).ConfigureAwait(false);
            if (result <= 0)
                return Conflict();

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateReportConfig(int id, [FromBody] UpdateReportConfigurationCommand report)
        {
            Guard.Against.Null(report, nameof(report));
            if (id != report.Id)
                return BadRequest();

            var result = await _mediator.Send(report).ConfigureAwait(false);
            if (!result)
                return Conflict();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteReportConfig(int id)
        {
            var result = await _mediator.Send(new DeleteReportConfigurationCommand(id)).ConfigureAwait(false);
            if (!result)
                return Conflict();

            return Ok();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ReportConfigurationDTO>> GetReportConfig(int id)
        {
            var reportConfig = await _reportConfigurationsRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (reportConfig is null)
                return BadRequest();

            return Ok(new ReportConfigurationDTO(reportConfig));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<ResumenReportConfiguration>> GetReportsConfig(string? filter)
        {
            var reportConfig = _reportConfigsQueries.GetByFilter(filter);

            if (reportConfig is null)
                return BadRequest();

            return Ok(reportConfig);
        }
    }
}
