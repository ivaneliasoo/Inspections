using MediatR;
using System.Runtime.Serialization;

namespace Inspections.API.Features.Checklists.Commands
{
    [DataContract]
    public class UpdateCheckListCommand:IRequest<bool>
    {
        [DataMember]
        public int IdCheckList { get; set; }
        [DataMember]
        public string Text { get; set; } = default!;
        [DataMember]
        public string? Annotation { get; set; }
        [DataMember]
        public bool IsConfiguration { get; set; }

        private UpdateCheckListCommand() { }

        public UpdateCheckListCommand(string text, string? annotation, bool isConfiguration)
        {
            Text = text;
            Annotation = annotation;
            IsConfiguration = isConfiguration;
        }
    }
}
