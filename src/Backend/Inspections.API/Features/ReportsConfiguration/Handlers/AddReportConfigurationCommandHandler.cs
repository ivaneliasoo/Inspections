using Ardalis.GuardClauses;
using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.ReportsConfiguration.Handlers
{
    public class AddReportConfigurationCommandHandler : IRequestHandler<AddReportConfigurationCommand, int>
    {
        private readonly IReportConfigurationsRepository _reportConfigurationsRepository;
        private readonly InspectionsContext _context;

        public AddReportConfigurationCommandHandler(IReportConfigurationsRepository reportConfigurationsRepository, InspectionsContext context)
        {
            _reportConfigurationsRepository = reportConfigurationsRepository ?? throw new ArgumentNullException(nameof(reportConfigurationsRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> Handle(AddReportConfigurationCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var checks = _context.Set<CheckList>()
                .Where(s => request.ChecksDefinition.Contains(s.Id)) //TODO: Create Method in repository
                .Include(p => p.Checks)
                    .ThenInclude(p => p.TextParams)
                .Include(p => p.TextParams)
                .ToList();

            var signatures = _context.Signatures.Where(s => request.SignatureDefinitions.Contains(s.Id))
                 .Include(p => p.Report)
                .Include(p => p.Responsable)
                .ToList();//TODO: Create Method in repository

            var repoConfig = new ReportConfiguration()
            {
                Type = request.Type,
                RemarksLabelText = request.RemarksLabelText,
                Title = request.Title,
                FormName = request.FormName,
                ChecksDefinition = PrepareForConfiguration(checks),
                SignatureDefinitions = PrepareForConfiguration(signatures)
                
            };

            var result = await _reportConfigurationsRepository.AddAsync(repoConfig).ConfigureAwait(false);

            return result.Id;
        }

        private static List<CheckList> PrepareForConfiguration(List<CheckList> checks)
        {
            var result = new List<CheckList>();
            foreach (CheckList check in checks)
            {
                result.Add(check.CloneForReportConfiguration());
            }
            return result;
        }

        private static List<Signature> PrepareForConfiguration(List<Signature> signatures)
        {
            var result = new List<Signature>();
            foreach (Signature signature in signatures)
            {
                result.Add(signature.PreparteForNewReportConfiguration());
            }
            return result;
        }
    }
}
