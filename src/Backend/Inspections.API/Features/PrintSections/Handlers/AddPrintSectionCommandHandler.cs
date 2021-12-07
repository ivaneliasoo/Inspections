using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.PrintSections.Commands;
using Inspections.Core.Domain.PrintSectionsAggregate;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.PrintSections.Handlers
{

    public class AddPrintSectionCommandHandler : IRequestHandler<AddPrintSectionCommand, bool>
    {
        private readonly IPrintSectionRepository _printSectionRepository;

        public AddPrintSectionCommandHandler(IPrintSectionRepository printSectionRepository)
        {
            _printSectionRepository = printSectionRepository ?? throw new ArgumentNullException(nameof(printSectionRepository));
        }

        public async Task<bool> Handle(AddPrintSectionCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var newPrintSection = new PrintSection()
            {
                Code = request.Code,
                Content = request.Content,
                IsMainReport = request.IsMainReport,
                Status = request.Status,
            };

            var result = await _printSectionRepository.AddAsync(newPrintSection).ConfigureAwait(false);

            return result.Id > 0;
        }
    }
}
