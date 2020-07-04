using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Inspections.API.Features.Checklists.Commands
{
    [DataContract]
    public class UpdateCheckListCommand:IRequest<bool>
    {
        [DataMember]
        public int IdCheckList { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Annotation { get; set; }
        [DataMember]
        public bool IsConfiguration { get; set; }

        private UpdateCheckListCommand() { }

        public UpdateCheckListCommand(string text, string annotation, bool isConfiguration)
        {
            Text = text;
            Annotation = annotation;
            IsConfiguration = isConfiguration;
        }
    }
}
