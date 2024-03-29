﻿using Ardalis.GuardClauses;
using Inspections.Core.Domain.Forms;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;

namespace Inspections.Core;

public class ReportsBuilder
{
    private Report _report;

    public ReportsBuilder(ReportConfiguration configuration, string userName)
    {
        Guard.Against.Null(configuration, nameof(configuration));

        _report = new Report(configuration.Title, configuration.FormName,
            string.IsNullOrWhiteSpace(configuration.RemarksLabelText) ? "Remarks" : configuration.RemarksLabelText,
            configuration.Id, configuration.UsePhotoRecord);
        
        this.WithDefaultNotes(configuration.UseNotes);

        if(configuration.ChecksDefinition is not null || configuration.UseCheckList)
            _report.AddCheckList(configuration.ChecksDefinition!);
        
        _report.AddSignature(configuration.SignatureDefinitions, userName);
    }
    
    public ReportsBuilder WithForms(IEnumerable<FormDefinition> forms)
    {
        _report.AddForms(forms.Select(f => new ReportForm(f.Name, f.Title, f.Fields, f.Icon, f.Enabled, f.Order))
            .ToList());

        return this;
    }

    public ReportsBuilder WithName(string name)
    {
        _report.SetName(name);

        return this;
    }

    private void WithDefaultNotes(bool addDefaultNotes)
    {
        if(!addDefaultNotes) return;
        var note = new Note()
        {
            Text = "Premise owner to rectify items marked \"not acceptable\"", NeedsCheck = true, Checked = true
        };

        _report.AddNote(note);
    }

    private void Reset()
    {
        this._report = new Report();
    }

    public Report Build()
    {
        Report result = this._report;
        this.Reset();
        return result;
    }
}
