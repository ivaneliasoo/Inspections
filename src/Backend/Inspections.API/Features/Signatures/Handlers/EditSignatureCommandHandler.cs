using Ardalis.GuardClauses;
using Inspections.API.Features.Signatures.Commands;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.ApplicationServices;
using MediatR;

namespace Inspections.API.Features.Signatures.Handlers;

public class EditSignatureCommandHandler : IRequestHandler<EditSignatureCommand, bool>
{
    private readonly ISignaturesRepository _signaturesRepository;
    private readonly IUserNameResolver _userNameResolver;

    public EditSignatureCommandHandler(ISignaturesRepository signaturesRepository, IUserNameResolver userNameResolver)
    {
        _signaturesRepository = signaturesRepository ?? throw new ArgumentNullException(nameof(signaturesRepository));
        _userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));
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
        newSignature.DrawnSign = request.DrawnSign;
        newSignature.DefaultResponsibleType = request.DefaultResponsibleType;
        newSignature.UseLoggedInUserAsDefault = request.UseLoggedInUserAsDefault;
        newSignature.SetDefaultResponsible(_userNameResolver.FullName);

        await _signaturesRepository.UpdateAsync(newSignature).ConfigureAwait(false);

        return true;
    }
}
