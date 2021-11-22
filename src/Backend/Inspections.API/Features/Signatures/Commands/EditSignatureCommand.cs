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
                                   ResponsibleType responsibleType,
                                   string responsibleName,
                                   string designation,
                                   string remarks,
                                   DateTimeOffset date,
                                   bool principal,
                                   string drawnSign,
                                   short order)
        {
            Id = id;
            Title = title;
            Annotation = annotation;
            ResponsibleType = responsibleType;
            ResponsibleName = responsibleName;
            Designation = designation;
            Remarks = remarks;
            Date = date;
            Principal = principal;
            DrawnSign = drawnSign;
            Order = order;
        }

        private EditSignatureCommand() { }

        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Annotation { get; set; }
        public ResponsibleType ResponsibleType { get; set; }
        public string ResponsibleName { get; set; } = default!;
        public string? Designation { get; set; }
        public string? Remarks { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Principal { get; set; }
        public string? DrawnSign { get; set; }
        public short Order { get; }
    }
}
