using Inspections.Core.Domain.CheckListAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Models
{
    public class CheckListParamDTO
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public CheckListParamType Type { get; set; }
    }
}
