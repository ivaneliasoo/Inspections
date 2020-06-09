using Inspections.API.Features.Signatures.Commands;
using Inspections.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.Signatures.Handlers
{
    public class DeleteSignatureCommandHandler : IRequestHandler<DeleteSignatureCommand, bool>
    {
        private readonly ISignaturesRepository _signaturesRepository;

        public DeleteSignatureCommandHandler(ISignaturesRepository signaturesRepository)
        {
            _signaturesRepository = signaturesRepository ?? throw new ArgumentNullException(nameof(signaturesRepository));
        }

        public async Task<bool> Handle(DeleteSignatureCommand request, CancellationToken cancellationToken)
        {
            var signature = await _signaturesRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

            await _signaturesRepository.DeleteAsync(signature).ConfigureAwait(false);

            return true;
        }
    }
}
