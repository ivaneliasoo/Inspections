using System.Text.Json;
using Ardalis.GuardClauses;
using Inspections.Core.Domain.Forms;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Infrastructure.ApplicationServices;
using Inspections.Infrastructure.Data;
using Inspections.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Forms.Create;

public class NewFormDefinitionCommand : IRequest<FormDefinitionResponse>
{
    public NewFormDefinitionCommand(int id, string name, string title, string icon, DynamicFields fields, JsonDocument? defaultValues, bool enabled, List<int>? reports, List<int>? reportConfigurations)
    {
        Id = id;
        Name = name;
        Title = title;
        Icon = icon;
        Fields = fields;
        DefaultValues = defaultValues;
        Enabled = enabled;
        if(reports is {})
            Reports = reports;
        if(reportConfigurations is {})
            ReportConfigurations = reportConfigurations;
    }

    public int Id { get; }
    public string Name { get; }
    public string Title { get; }
    public string Icon { get; }
    public DynamicFields Fields { get; }
    public JsonDocument? DefaultValues { get; }
    public bool Enabled { get; }
    public short Order { get; }
    public List<int>? Reports { get; }
    public List<int>? ReportConfigurations { get; }
}

public class NewFormDefinitionCommandHandler : IRequestHandler<NewFormDefinitionCommand, FormDefinitionResponse>
{
    private readonly InspectionsContext _context;
    private readonly IUserNameResolver _userNameResolver;

    public NewFormDefinitionCommandHandler(InspectionsContext context, IUserNameResolver userNameResolver)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));
    }
    
    public async Task<FormDefinitionResponse> Handle(NewFormDefinitionCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var newFormDefinition = request.ToEntity();
        var reportsConfigurationToAssociate = await _context.ReportConfigurations.Where(r => request.ReportConfigurations.Contains(r.Id)).ToListAsync(cancellationToken: cancellationToken);
        newFormDefinition.AssociateReportConfiguration(reportsConfigurationToAssociate);

        await _context.FormsDefinitions.AddAsync(newFormDefinition, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return FormDefinitionResponse.CreateFromEntity(newFormDefinition); 
    }
}
