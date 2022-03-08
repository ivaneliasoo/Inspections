using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inspections.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inspections.API.Features.Forms.GetAllByReportId;

public record GetAllByReportIdQuery(int? ReportId) : IRequest<IEnumerable<FormDefinitionResponse>>;

public class GetAllByReportId : IRequestHandler<GetAllByReportIdQuery, IEnumerable<FormDefinitionResponse>>
{
    private readonly InspectionsContext _context;
    private readonly IMapper _mapper;

    public GetAllByReportId(InspectionsContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<IEnumerable<FormDefinitionResponse>> Handle(GetAllByReportIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reports
            .AsNoTracking()
            .Where(r => r.Id == request.ReportId)
            .SelectMany(r => r.AvailableForms, (report, form) =>
                new FormDefinitionResponse
                {
                   Id = form.Id,
                    Name = form.Name,
                   Title = form.Title,
                   Icon = form.Icon,
                   Fields = form.Fields,
                   Enabled = form.Enabled
                })
            .ToListAsync(cancellationToken);
    }
}

