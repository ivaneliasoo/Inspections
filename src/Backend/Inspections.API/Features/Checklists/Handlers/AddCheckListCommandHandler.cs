using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.API.Features.Checklists.Mapping;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces.Repositories;
using MediatR;

namespace Inspections.API.Features.Checklists.Handlers
{
    public class AddCheckListCommandHandler : IRequestHandler<AddCheckListCommand, bool>
    {
        private readonly ICheckListsRepository _checkListsRepository;

        public AddCheckListCommandHandler(ICheckListsRepository checkListsRepository)
        {
            _checkListsRepository = checkListsRepository ?? throw new ArgumentNullException(nameof(checkListsRepository));
        }

        public async Task<bool> Handle(AddCheckListCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var checkList = new CheckList(request.Text,
                                          request.Annotation,
                                          request.IsConfiguration);
            if (request.Items is not null)
                checkList.AddCheckItems(CheckListMappingHelper.MapItems(request.Items));

            var result = await _checkListsRepository.AddAsync(checkList).ConfigureAwait(false);

            if (result == null)
                return false;

            return true;
        }
    }
}
