using System.Collections.Generic;
using System.Runtime.Serialization;
using Inspections.API.Features.Checklists.Models;
using MediatR;

namespace Inspections.API.Features.Checklists.Commands
{
    [DataContract]
    public class AddCheckListCommand : IRequest<bool>
    {
        [DataMember]
        public string Text { get; set; } = default!;

        [DataMember]
        public List<CheckListItemDTO>? Items { get; set; }
        [DataMember]
        public string? Annotation { get; set; }
        public bool IsConfiguration { get; set; }

        private AddCheckListCommand() { }

        public AddCheckListCommand(string text, string? annotation, List<CheckListItemDTO>? items, bool isConfiguration)
        {
            Text = text;
            Items = items;
            Annotation = annotation;
            IsConfiguration = isConfiguration;
        }
    }
}
