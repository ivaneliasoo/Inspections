using Ardalis.GuardClauses;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Inspections.Core.Domain.ReportsAggregate;

namespace Inspections.Core;

public class ReportsBuilder : IReportsBuilder
{
    private Report _report;

    public ReportsBuilder(ReportConfiguration configuration, string userName)
    {
        Guard.Against.Null(configuration, nameof(configuration));

        Configuration = configuration;

        _report = new Report(configuration.Title, configuration.FormName, configuration.RemarksLabelText ?? "Remarks", configuration.Id);

        _report.AddCheckList(configuration.ChecksDefinition);
        _report.AddSignature(configuration.SignatureDefinitions, userName);

    }

    internal ReportConfiguration Configuration { get; private set; }

    public ReportsBuilder WithOperationalReadings()
    {
        var or = new OperationalReadings
        {
            // TODO: this values should come from configuration
            VoltageL1N = 230,
            VoltageL2N = 230,
            VoltageL3N = 230,
            VoltageL1L2 = 400,
            VoltageL1L3 = 400,
            VoltageL2L3 = 400,
            RunningLoadL1 = 10,
            RunningLoadL2 = 10,
            RunningLoadL3 = 10,
            MainBreakerCapacity = 0,
            OverCurrentDirectActing = 0,
            MainBreakerRating = 100,
        };

        _report.AddOperationalReadings(or);
        return this;
    }

    public ReportsBuilder AddChecklists(int[] checklistsIds)
    {
        var checkListsToAdd = Configuration.ChecksDefinition.Where(s => checklistsIds.Contains(s.Id));

        return this;
    }

    public ReportsBuilder AddSignatures(int[] signaturesIds)
    {
        var signatureToAdd = Configuration.SignatureDefinitions.Where(s => signaturesIds.Contains(s.Id))
            .OrderByDescending(s => s.Order);
        _report.AddSignature(signatureToAdd.AsEnumerable());

        return this;
    }

    public ReportsBuilder WithName(string name)
    {
        _report.SetName(name);

        return this;
    }

    public ReportsBuilder WithDefaultNotes(bool addDefaultNOtes)
    {
        var note = new Note()
        {
            Text = "Premise owner to rectify items marked \"not acceptable\"",
            NeedsCheck = true,
            Checked = true
        };

        _report.AddNote(note);

        return this;
    }

    internal void Reset()
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
