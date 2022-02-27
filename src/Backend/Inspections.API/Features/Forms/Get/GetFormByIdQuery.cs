using Ardalis.GuardClauses;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Forms.Get;

public record GetFormByIdQuery : IRequest<FormDefinitionResponse>
{
    public int Id { get; }

    public GetFormByIdQuery(int id)
    {
        Guard.Against.NegativeOrZero(id, nameof(id));
        
        Id = id;
    }
}

public class GetFormByIdHandler : IRequestHandler<GetFormByIdQuery, FormDefinitionResponse>
{
    private readonly InspectionsContext _context;

    public GetFormByIdHandler(InspectionsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<FormDefinitionResponse> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.FormsDefinitions
            .AsNoTracking()
            .Select(fd => FormDefinitionResponse.CreateFromEntity(fd)).SingleAsync(f => f.Id == request.Id, cancellationToken);
    }
}

