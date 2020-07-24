using Inspections.Core.Domain.CheckListAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Models
{
    public class CheckListItemDTO
    {
        public int Id { get; set; }
        public int CheckListId { get; set; }
        public string Text { get; set; }
        public CheckValue Checked { get; set; }
        public bool Editable { get; set; }
        public bool Required { get; set; }
        public string Remarks { get; set; }
        public List<CheckListParamDTO> TextParams { get; set; } = new List<CheckListParamDTO>();
    }
}
