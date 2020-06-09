using Inspections.Core.Domain.SignaturesAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Signatures.Commands
{
    public class AddSignatureCommand : IRequest<bool>
    {
        public AddSignatureCommand(string title,
                                   string annotation,
                                   ResponsableType responsableType,
                                   string responsableName,
                                   string designation,
                                   string remarks,
                                   DateTimeOffset date,
                                   bool principal)
        {
            Title = title;
            Annotation = annotation;
            ResponsableType = responsableType;
            ResponsableName = responsableName;
            Designation = designation;
            Remarks = remarks;
            Date = date;
            Principal = principal;
        }

        private AddSignatureCommand() { }

        public string Title { get; set; }
        public string Annotation { get; set; }
        public ResponsableType ResponsableType { get; set; }
        public string ResponsableName { get; set; }
        public string Designation { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Principal { get; set; }
    }
}
