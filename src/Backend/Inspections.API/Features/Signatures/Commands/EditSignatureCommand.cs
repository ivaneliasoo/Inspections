using System;
using Inspections.Core.Domain.SignaturesAggregate;
using JetBrains.Annotations;
using MediatR;

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

        public int Id { get; }
        public string Title { get; } = default!;
        public string? Annotation { get; }
        public ResponsibleType ResponsibleType { get; }
        public string ResponsibleName { get; } = default!;
        public string? Designation { get; }
        public string? Remarks { get; }
        public DateTimeOffset Date { get; }
        public bool Principal { get; }
        public string? DrawnSign { get; }
        // ReSharper disable once MemberCanBePrivate.Global
        public short Order { [UsedImplicitly] get; }
    }
}
