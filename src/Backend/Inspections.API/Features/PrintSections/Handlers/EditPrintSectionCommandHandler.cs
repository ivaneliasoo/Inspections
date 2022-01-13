using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.PrintSections.Commands;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.PrintSections.Handlers
{

    public class EditPrintSectionCommandHandler : IRequestHandler<EditPrintSectionCommand, bool>
    {
        private readonly IPrintSectionRepository _printSectionRepository;


        public EditPrintSectionCommandHandler(IPrintSectionRepository printSectionRepository)
        {
            _printSectionRepository = printSectionRepository ?? throw new ArgumentNullException(nameof(printSectionRepository));
        }

        public async Task<bool> Handle(EditPrintSectionCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var editPrintSection = await _printSectionRepository.GetByIdAsync(request.Id).ConfigureAwait(false);
            {
                editPrintSection.Code = request.Code;
                editPrintSection.Description = request.Description;
                editPrintSection.Content = request.Content;
                editPrintSection.IsMainReport = request.IsMainReport;
                editPrintSection.Status = request.Status;
            };


            await _printSectionRepository.UpdateAsync(editPrintSection).ConfigureAwait(false);

            return true;
        }
    }
}
