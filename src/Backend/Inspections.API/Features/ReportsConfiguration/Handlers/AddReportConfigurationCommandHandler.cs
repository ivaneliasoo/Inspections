using Ardalis.GuardClauses;
using Inspections.API.Features.ReportsConfiguration.Commands;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.ReportsConfiguration.Handlers;

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
        
        var signatures = _context.Signatures.Where(s => request.SignatureDefinitions.Contains(s.Id))
            .Include(p => p.Responsible)
            .ToList();

        var repoConfig = new ReportConfiguration()
        {
            Type = request.Type,
            RemarksLabelText = request.RemarksLabelText ?? "Remarks",
            Title = request.Title,
            FormName = request.FormName,
            SignatureDefinitions = PrepareForConfiguration(signatures),
            Footer = $@"<footer style=""padding-left: 20px; opacity: 0.5; font-size: 3.2em; display: flex;margin: 10px, 10px;flex-direction: column;color: grey;font-family: 'Times New Roman', Times, serif;"">
                                            <div class='' style='font-size: 3.2em; text-align: right;letter-spacing: 2px;'><label class='pageNumber'></label> | Page</div>
                                            <div class='footer'>
                                              <p style='line-height: 3px;font-size: 3.2em;'>FORM E1(CSE INTERNAL) INSPECTION REPORT FOR LICENSING LEW SINGLE USER PREMISE- REV #8
                                              </p><p style='line-height: 3px;font-size: 3.2em;'>ALL RIGHTS RESERVED TO CHENG SENG ELECTRIC CO PTE LTD</p>
                                            </div>
                                          </footer>
                                        ",
            MarginBottom = "80px",
            MarginTop = "20px",
            MarginLeft = "70px",
            MarginRight = "70px",
            PrintSectionId = request.PrintSectionId,
            TemplateName = request.TemplateName,
            UseNotes = request.UseNotes,
            UseCheckList = request.UseCheckList,
            UsePhotoRecord = request.UsePhotoRecord,
        };
        
        if (request.ChecksDefinition is {})
        {
            var checks = _context.Set<CheckList>()
                .Where(s => request.ChecksDefinition.Contains(s.Id))
                .Include(p => p.Checks)
                .ToList();

            repoConfig.ChecksDefinition = PrepareForConfiguration(checks);
        }

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
            result.Add(signature.PrepareForNewReportConfiguration());
        }
        return result;
    }
}
