using Ardalis.GuardClauses;
using Inspections.Infrastructure.Data;
using MediatR;

namespace Inspections.API.Features.Forms.Delete;

public record DeleteFormDefinitionCommand(int Id) : IRequest<bool>;

public class DeleteFormDefinitionCommandHandler : IRequestHandler<DeleteFormDefinitionCommand, bool>
{
    private readonly InspectionsContext _context;

    public DeleteFormDefinitionCommandHandler(InspectionsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<bool> Handle(DeleteFormDefinitionCommand request, CancellationToken cancellationToken)
    {
        var formDef = await _context.FormsDefinitions.FindAsync(request.Id).ConfigureAwait(false);
        if (formDef == null)
        {
            throw new NotFoundException(request.Id.ToString(), nameof(formDef));
        }

        _context.FormsDefinitions.Remove(formDef);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }
}
