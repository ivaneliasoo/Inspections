using Ardalis.GuardClauses;
using Inspections.API.Features.Signatures.Commands;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Core.Interfaces.Repositories;
using Inspections.Infrastructure.ApplicationServices;
using MediatR;

namespace Inspections.API.Features.Signatures.Handlers;

public class AddSignatureCommandHandler : IRequestHandler<AddSignatureCommand, bool>
{
    private readonly ISignaturesRepository _signaturesRepository;
    private readonly IUserNameResolver _userNameResolver;

    public AddSignatureCommandHandler(ISignaturesRepository signaturesRepository, IUserNameResolver userNameResolver)
    {
        _signaturesRepository = signaturesRepository ?? throw new ArgumentNullException(nameof(signaturesRepository));
        _userNameResolver = userNameResolver ?? throw new ArgumentNullException(nameof(userNameResolver));
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
            Date = request.Date ?? default,
            Principal = request.Principal,
            IsConfiguration = request.ReportConfigurationId > 0,
            ReportConfigurationId = request.ReportConfigurationId == 0 ? default : request.ReportConfigurationId,
            ReportId = request.ReportId == 0 ? default : request.ReportId,
            DefaultResponsibleType = request.DefaultResponsibleType,
            UseLoggedInUserAsDefault = request.UseLoggedInUserAsDefault,
        };
        
        newSignature.SetDefaultResponsible(_userNameResolver.FullName);

        var result = await _signaturesRepository.AddAsync(newSignature).ConfigureAwait(false);

        return result.Id > 0;
    }
}
