using Inspections.Core.Domain.CheckListAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Commands
{
    public class UpdateCheckListParamCommand : IRequest<bool>
    {
        public UpdateCheckListParamCommand(int id, string key, string value, CheckListParamType type)
        {
            Id = id;
            Key = key;
            Value = value;
            Type = type;
        }

        private UpdateCheckListParamCommand() { }

        public int? IdCheckList { get; set; }
        public int? IdCheckListItem { get; set; }

        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public CheckListParamType Type { get; set; }
    }
}
