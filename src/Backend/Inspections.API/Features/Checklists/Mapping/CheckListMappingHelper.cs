using Inspections.API.Features.Checklists.Models;
using Inspections.Core.Domain.CheckListAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Mapping
{
    internal static class CheckListMappingHelper
    {
        internal static List<CheckListParam> MapParams(List<CheckListParamDTO> checklistParams)
        {
            List<CheckListParam> result = new List<CheckListParam>();
            foreach (var param in checklistParams)
            {
                var newParam = new CheckListParam
                (
                    null,
                    0,
                    param.Key,
                    param.Value,
                    param.Type
                );
                result.Add(newParam);
            }
            return result;
        }

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
