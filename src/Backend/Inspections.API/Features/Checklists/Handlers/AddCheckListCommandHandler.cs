using Ardalis.GuardClauses;
using Inspections.API.Features.Checklists.Commands;
using Inspections.API.Features.Checklists.Mapping;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                                          CheckListMappingHelper.MapParams(request.TextParams),
                                          request.Annotation,
                                          false);

            checkList.AddCheckItems(CheckListMappingHelper.MapItems(request.Items));

            var result = await _checkListsRepository.AddAsync(checkList).ConfigureAwait(false);

            if (result == null)
                return false;

            return true;
        }
    }
}
