using System.Collections.Generic;
using Inspections.API.Features.Checklists.Models;
using Inspections.Core.Domain.CheckListAggregate;

namespace Inspections.API.Features.Checklists.Mapping
{
    internal static class CheckListMappingHelper
    {
        internal static List<CheckListItem> MapItems(List<CheckListItemDTO> checklistItems)
        {
            List<CheckListItem> result = new List<CheckListItem>();
            foreach (var item in checklistItems)
            {
                var newItem = new CheckListItem
                (
                   item.CheckListId,
                   item.Text,
                   item.Checked,
                   item.Editable,
                   item.Required,
                   item.Remarks
                );
                result.Add(newItem);
            }
            return result;
        }
    }
}
