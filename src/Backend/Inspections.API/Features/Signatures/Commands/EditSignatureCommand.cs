using Inspections.Core.Domain.SignaturesAggregate;
using MediatR;
using System;

namespace Inspections.API.Features.Signatures.Commands
{
    public class EditSignatureCommand : IRequest<bool>
    {
        public EditSignatureCommand(int id, 
                                   string title,
                                   string annotation,
                                   ResponsableType responsableType,
                                   string responsableName,
                                   string designation,
                                   string remarks,
                                   DateTimeOffset date,
                                   bool principal,
                                   string drawedSign)
        {
            Id = id;
            Title = title;
            Annotation = annotation;
            ResponsableType = responsableType;
            ResponsableName = responsableName;
            Designation = designation;
            Remarks = remarks;
            Date = date;
            Principal = principal;
            DrawedSign = drawedSign;
        }

        private EditSignatureCommand() { }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Annotation { get; set; }
        public ResponsableType ResponsableType { get; set; }
        public string ResponsableName { get; set; }
        public string Designation { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Principal { get; set; }
        public string DrawedSign { get; set; }

    }
}
