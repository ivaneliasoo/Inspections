using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Signatures.Commands;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces;
using MediatR;

namespace Inspections.API.Features.Signatures.Handlers
{
    public class AddSignatureCommandHandler : IRequestHandler<AddSignatureCommand, bool>
    {
        private readonly ISignaturesRepository _signaturesRepository;

        public AddSignatureCommandHandler(ISignaturesRepository signaturesRepository)
        {
            _signaturesRepository = signaturesRepository ?? throw new ArgumentNullException(nameof(signaturesRepository));
        }

        public async Task<bool> Handle(AddSignatureCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var newSignature = new Signature()
            {
                Title = request.Title,
                Annotation = request.Annotation,
                Remarks = request.Remarks,
                Designation = request.Designation,
                Date = request.Date,
                Principal = request.Principal,
                Responsible = new Responsible() { Name = request.ResponsibleName, Type = request.Title.Contains("LEW", StringComparison.OrdinalIgnoreCase) ? ResponsibleType.LEW : request.ResponsableType },
                IsConfiguration = request.ReportConfigurationId > 0,
                ReportConfigurationId = request.ReportConfigurationId == 0 ? default : request.ReportConfigurationId,
                ReportId = request.ReportId == 0 ? default : request.ReportId
            };

            var result = await _signaturesRepository.AddAsync(newSignature).ConfigureAwait(false);

            return result.Id > 0;
        }
    }
}
