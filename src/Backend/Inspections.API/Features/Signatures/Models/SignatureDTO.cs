using Ardalis.GuardClauses;
using Inspections.Core.Domain.SignaturesAggregate;
using System;

namespace Inspections.API.Features.Signatures.Models
{
    public class SignatureDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Annotation { get; set; }
        public ResponsibleType ResponsableType { get; set; }
        public string? ResponsableName { get; set; } = default!; 
        public string? Designation { get; set; }
        public string? Remarks { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool Principal { get; set; }
        public int? ReportId { get; set; }
        public int? ReportConfigurationId { get; set; }
        public short Order { get; set; }

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
            if (!signature.IsConfiguration && signature.Responsible != null)
            {
                ResponsableType = signature.Responsible.Type;
                ResponsableName = signature.Responsible.Name;
            }
            Order = signature.Order;
            ReportConfigurationId = signature.ReportConfigurationId;
            ReportId = signature.ReportId;
        }
    }
}
