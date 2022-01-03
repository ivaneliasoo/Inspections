﻿using Ardalis.GuardClauses;
using Inspections.API.Features.Signatures.Commands;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.Signatures.Handlers
{
    public class EditSignatureCommandHandler : IRequestHandler<EditSignatureCommand, bool>
    {
        private readonly ISignaturesRepository _signaturesRepository;

        public EditSignatureCommandHandler(ISignaturesRepository signaturesRepository)
        {
            _signaturesRepository = signaturesRepository ?? throw new ArgumentNullException(nameof(signaturesRepository));
        }

        public async Task<bool> Handle(EditSignatureCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var newSignature = await _signaturesRepository.GetByIdAsync(request.Id).ConfigureAwait(false);
            newSignature.Title = request.Title;
            newSignature.Annotation = request.Annotation;
            newSignature.Remarks = request.Remarks;
            newSignature.Designation = request.Designation;
            newSignature.Date = request.Date;
            newSignature.Principal = request.Principal;
            var determinedResponsableType = request.Title.Contains("LEW", StringComparison.OrdinalIgnoreCase) ? ResponsibleType.LEW : request.ResponsibleType;
            newSignature.Responsible = new Responsible() { Name = request.ResponsibleName, Type = determinedResponsableType };
            newSignature.IsConfiguration = false;
            newSignature.DrawnSign = request.DrawnSign;

            await _signaturesRepository.UpdateAsync(newSignature).ConfigureAwait(false);

            return true;
        }
    }
}
