using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Ardalis.GuardClauses;

namespace Inspections.API.Features.Inspections.Commands
{
    public class CreateReportCommand : IRequest<bool>
    {
        public int ConfigurationId { get; }
        public ReportType ReportType { get; }

        public CreateReportCommand(int configurationId, ReportType reportType)
        {
            Guard.Against.NegativeOrZero(configurationId, nameof(configurationId));

            ConfigurationId = configurationId;
            ReportType = reportType;
        }
    }
}
