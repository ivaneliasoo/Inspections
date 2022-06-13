using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.ReportsConfiguration.Handlers
{
    public class UpdateReportConfigurationCommandHandler : IRequestHandler<UpdateReportConfigurationCommand, bool>
    {
        private readonly IReportConfigurationsRepository _reportConfigurationsRepository;
        private readonly InspectionsContext _context;

        public UpdateReportConfigurationCommandHandler(IReportConfigurationsRepository reportConfigurationsRepository,
            InspectionsContext context)
        {
            _reportConfigurationsRepository = reportConfigurationsRepository ??
                                              throw new ArgumentNullException(nameof(reportConfigurationsRepository));
            _context = context;
        }

        public async Task<bool> Handle(UpdateReportConfigurationCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var reportConfig = await _reportConfigurationsRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (reportConfig is null)
                return false;

            if (request.ChecksDefinition is { })
            {
                var checks = _context.Set<CheckList>()
                    .Where(s => request.ChecksDefinition.Contains(s.Id)) //TODO: Create Method in repository
                    .Include(p => p.Checks)
                    .ToList();
                reportConfig.ChecksDefinition = checks;
            }


            if (request.SignatureDefinitions is {})
            {
                var signatures = _context.Signatures.Where(s => request.SignatureDefinitions!.Contains(s.Id))
                    .Include(p => p.Responsible)
                    .ToList(); //TODO: Create Method in repository
                reportConfig.SignatureDefinitions = signatures;
            }

            reportConfig.Type = request.Type;
            reportConfig.RemarksLabelText = request.RemarksLabelText;
            reportConfig.Title = request.Title;
            reportConfig.FormName = request.FormName;
            reportConfig.PrintSectionId = request.PrintSectionId;
            reportConfig.CheckListMetadata.Display = request.Display;
            reportConfig.TemplateName = request.TemplateName;
            await _reportConfigurationsRepository.UpdateAsync(reportConfig).ConfigureAwait(false);
            return true;
        }
    }
}
