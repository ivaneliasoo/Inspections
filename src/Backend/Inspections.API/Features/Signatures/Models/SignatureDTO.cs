using Ardalis.GuardClauses;
using Inspections.Core.Domain.SignaturesAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.Signatures.Models
{
    public class SignatureDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Annotation { get; set; }
        public ResponsableType ResponsableType { get; set; }
        public string ResponsableName { get; set; }
        public string Designation { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Principal { get; set; }

        public SignatureDTO(Signature signature)
        {
            Guard.Against.Null(signature, nameof(signature));

            Id = signature.Id;
            Title = signature.Title;
            Annotation = signature.Annotation;
            Designation = signature.Designation;
            Remarks = signature.Remarks;
            Date = signature.Date;
            Principal = signature.Principal;
            if (!signature.IsConfiguration && signature.Responsable != null)
            {
                ResponsableType = signature.Responsable.Type;
                ResponsableName = signature.Responsable.Name;
            }
        }
    }
}
