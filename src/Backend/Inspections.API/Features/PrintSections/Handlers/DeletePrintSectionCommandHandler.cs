using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.PrintSections.Commands;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.PrintSections.Handlers
{

    public class DeletePrintSectionCommandHandler : IRequestHandler<DeletePrintSectionCommand, bool>
    {
        private readonly IPrintSectionRepository _printSectionRepository;

        public DeletePrintSectionCommandHandler(IPrintSectionRepository printSectionRepository)
        {
            _printSectionRepository = printSectionRepository ?? throw new ArgumentNullException(nameof(printSectionRepository));
        }

        public async Task<bool> Handle(DeletePrintSectionCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var signature = await _printSectionRepository.GetByIdAsync(request.Id).ConfigureAwait(false);

            await _printSectionRepository.DeleteAsync(signature).ConfigureAwait(false);

            return true;
        }
    }
}
