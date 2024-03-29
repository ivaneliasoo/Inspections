﻿using Inspections.Shared;

namespace Inspections.Core.Domain.SignaturesAggregate;

public class Signature : Entity<int>, IAggregateRoot
{
    public string Title { get; set; } = default!;
    public string? Annotation { get; set; }
    public Responsible Responsible { get; set; } = new Responsible();
    public string? ResponsibleName => Responsible.Name;
    public string? Designation { get; set; }
    public string? Remarks { get; set; }
    public DateTimeOffset Date { get; set; }
    public bool Principal { get; set; }
    public bool IsConfiguration { get; set; }
    public int? ReportId { get; set; }
    public int? ReportConfigurationId { get; set; }
    public string? DrawnSign { get; set; }
    public short Order { get; set; }
    public ResponsibleType? DefaultResponsibleType { get; set; }
    public bool UseLoggedInUserAsDefault { get; set; }

    public void SetDefaultResponsible(string userName)
    {
        this.Responsible = new Responsible
        {
            Name = this.UseLoggedInUserAsDefault ? userName : string.Empty,
            Type = this.DefaultResponsibleType ?? ResponsibleType.Inspector
        };
    }

    public Signature PrepareForNewReport(string userName)
    {
        Signature newSignature = (Signature)this.MemberwiseClone();
        if (newSignature is null) throw new Exception("error trying to add signatures to the report");
        newSignature.Id = 0;
        newSignature.ReportId = 0;
        newSignature.ReportConfigurationId = null;
        newSignature.Date = DateTimeOffset.Now;
        newSignature.IsConfiguration = false;

        SetDefaultResponsible(userName);

        return newSignature;
    }

    public Signature PrepareForNewReportConfiguration()
    {
        var newSignature = (Signature)this.MemberwiseClone();
        if (newSignature is null) throw new Exception("error trying to add signatures to the report");
        newSignature.Id = 0;
        newSignature.ReportId = null;
        newSignature.ReportConfigurationId = 0;
        newSignature.IsConfiguration = true;
        newSignature.Responsible = new Responsible {Name = string.Empty, Type = ResponsibleType.Inspector};

        return newSignature;
    }
}
