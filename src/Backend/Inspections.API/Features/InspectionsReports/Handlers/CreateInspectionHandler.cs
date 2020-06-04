using Inspections.API.Features.Inspections.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.Inspections.Handlers
{
    public class CreateInspectionHandler : IRequestHandler<Create, bool>
    {
        public Task<bool> Handle(Create request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
