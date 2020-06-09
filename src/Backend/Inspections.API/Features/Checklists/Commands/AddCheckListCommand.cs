using Inspections.API.Features.Checklists.Models;
using Inspections.Core.Domain.CheckListAggregate;
using MediatR;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Commands
{
    [DataContract]
    public class AddCheckListCommand : IRequest<bool>
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public List<CheckListParamDTO> TextParams { get; set; }
        [DataMember]
        public List<CheckListItemDTO> Items { get; set; }
        [DataMember]
        public string Annotation { get; set; }

        private AddCheckListCommand() { }

        public AddCheckListCommand(string text, string annotation, List<CheckListItemDTO> items, List<CheckListParamDTO> textParams)
        {
            Text = text;
            TextParams = textParams;
            Items = items;
            Annotation = annotation;
        }
    }
}
