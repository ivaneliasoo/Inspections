using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.API.Features.ReportsConfiguration.Model;
using Inspections.Core.Interfaces;
using Inspections.Core.QueryModels;
using Inspections.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.ReportsConfiguration
{
    [Authorize]
    [Route("[controller]")]
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
        public async Task<IActionResult> CreateReportConfig([FromBody] AddReportConfigurationCommand report)
        {
            var result = await _mediator.Send(report).ConfigureAwait(false);
            if (result <= 0)
                return Conflict();

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReportConfig(int id, [FromBody] UpdateReportConfigurationCommand report)
        {
            if (id != report.Id)
                return BadRequest();

            var result = await _mediator.Send(report).ConfigureAwait(false);
            if (!result)
                return Conflict();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReportConfig(int id, [FromBody] DeleteReportConfigurationCommand report)
        {
            if (id != report.Id)
                return BadRequest();

            var result = await _mediator.Send(report).ConfigureAwait(false);
            if (!result)
                return Conflict();

            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReportConfigurationDTO>> GetReportConfig(int id)
        {
            var reportConfig = await _reportConfigurationsRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (reportConfig is null)
                return BadRequest();

            return Ok(new ReportConfigurationDTO(reportConfig));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ResumenReportConfiguration>> GetReportsConfig(string filter)
        {
            var reportConfig = _reportConfigsQueries.GetByFilter(filter);

            if (reportConfig is null)
                return BadRequest();

            return Ok(reportConfig);
        }
    }
}
