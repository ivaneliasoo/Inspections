using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Handlers
{
    public class DeleteCheckListParamCommandHandler : IRequestHandler<DeleteCheckListParamCommand, bool>
    {
        private readonly ICheckListsRepository _checkListsRepository;

        public DeleteCheckListParamCommandHandler(ICheckListsRepository checkListsRepository)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
        }

        public Task<bool> Handle(DeleteCheckListParamCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            return Task.FromResult(true);
        }
    }
}
