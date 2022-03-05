using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Forms.GetAll;

public record GetAllQuery(string? Filter) : IRequest<IEnumerable<FormDefinitionResponse>>;

public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<FormDefinitionResponse>>
{
    private readonly InspectionsContext _context;
    private readonly IMapper _mapper;

    public GetAllHandler(InspectionsContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<IEnumerable<FormDefinitionResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = _context.FormsDefinitions
            .AsNoTracking()
            .Where(f => f.Enabled);

        if (request.Filter is not null)
        {
            result = result.Where(f => f.Name.Contains(request.Filter));
        }
                
        return await result.ProjectTo<FormDefinitionResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}

