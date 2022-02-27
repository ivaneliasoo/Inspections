using System.Text.Json;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.Forms;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Infrastructure.Data;
using Inspections.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Forms.Create;

public class NewFormDefinitionCommand : IRequest<FormDefinitionResponse>
{
    public NewFormDefinitionCommand(int id, string name, string title, DynamicFields fields, JsonDocument? defaultValues, bool enabled, List<int> reports, List<int> reportConfigurations)
    {
        Id = id;
        Name = name;
        Title = title;
        Fields = fields;
        DefaultValues = defaultValues;
        Enabled = enabled;
        Reports = reports;
        ReportConfigurations = reportConfigurations;
    }

    public int Id { get; }
    public string Name { get; }
    public string Title { get; }
    public DynamicFields Fields { get; }
    public JsonDocument? DefaultValues { get; }
    public bool Enabled { get; }
    public List<int> Reports { get; }
    public List<int> ReportConfigurations { get; }
}

public class NewFormDefinitionCommandHandler : IRequestHandler<NewFormDefinitionCommand, FormDefinitionResponse>
{
    private readonly InspectionsContext _context;

    public NewFormDefinitionCommandHandler(InspectionsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<FormDefinitionResponse> Handle(NewFormDefinitionCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var newFormDefinition = request.ToEntity();
        var reportsToAssociate = await _context.Reports.Where(r => request.Reports.Contains(r.Id)).ToListAsync(cancellationToken: cancellationToken);
        var reportsConfigurationToAssociate = await _context.ReportConfigurations.Where(r => request.Reports.Contains(r.Id)).ToListAsync(cancellationToken: cancellationToken);
        newFormDefinition.AssociateReport(reportsToAssociate);
        newFormDefinition.AssociateReportConfiguration(reportsConfigurationToAssociate);

        if (!await _context.FormsDefinitions.AnyAsync(fd => fd.Id == newFormDefinition.Id, cancellationToken))
            throw new NotFoundException(newFormDefinition.Id.ToString(), nameof(FormDefinition));

        await _context.FormsDefinitions.AddAsync(newFormDefinition, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return FormDefinitionResponse.CreateFromEntity(newFormDefinition); 
    }
}
