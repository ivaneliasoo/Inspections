using Ardalis.GuardClauses;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;

namespace Inspections.Core.Domain.ReportsAggregate;

public class Report : Entity<int>, IAggregateRoot
{
    public int ReportConfigurationId { get; set; }
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public int? EMALicenseId { get; set; }
    public EMALicense? License { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public bool IsClosed { get; private set; }
    public string Title { get; } = default!;
    public string FormName { get; } = default!;
    public string? RemarksLabelText { get; }
    public bool NeedsPhotoRecord { get; }
    private readonly List<Signature> signatures = new();
    public IReadOnlyCollection<Signature> Signatures => signatures;
    private readonly List<Note> notes = new();
    public IReadOnlyCollection<Note> Notes => notes;

    private readonly List<CheckList> checkList = new();
    public IReadOnlyCollection<CheckList> CheckList => checkList;

    private readonly List<PhotoRecord> photoRecords = new();
    public IReadOnlyCollection<PhotoRecord> PhotoRecords => photoRecords;

    private readonly List<ReportForm> _availableForms = new();
    public IReadOnlyCollection<ReportForm> AvailableForms => _availableForms;

    internal Report()
    {
    }

    public Report(string name, string address, EMALicense? license, DateTimeOffset date, int reportConfigurationId)
    {
        Name = name;
        Address = address;
        License = license;
        Date = date;
        ReportConfigurationId = reportConfigurationId;
    }

    internal void SetName(string name)
    {
        Name = name;
    }

    public Report(string title, string formName, string remarksLabelText, int reportConfigurationId,
        bool needsPhotoRecord)
        : this(string.Empty, string.Empty, null, DateTimeOffset.UtcNow, reportConfigurationId)
    {
        Title = title;
        FormName = formName;
        RemarksLabelText = remarksLabelText;
        NeedsPhotoRecord = needsPhotoRecord;
    }

    public void Edit(string name, string address, EMALicense? license, DateTimeOffset date)
    {
        Name = name;
        Address = address;
        License = license;
        EMALicenseId = license?.Id;
        Date = date;
    }

    public void ClearSignatures()
    {
        this.signatures.Clear();
    }

    public bool Completed => checkList.All(c => c.Completed);

    public void AddNote(Note note)
    {
        notes.Add(note);
    }

    public void AddNote(IEnumerable<Note> note)
    {
        notes.AddRange(note);
    }

    public void RemoveNote(Note note)
    {
        notes.Remove(note);
    }

    public void AddPhoto(PhotoRecord photo)
    {
        photoRecords.Add(photo);
    }

    public void AddPhoto(IEnumerable<PhotoRecord> photo)
    {
        photoRecords.AddRange(photo);
    }

    public void RemovePhoto(PhotoRecord photo)
    {
        photoRecords.Remove(photo);
    }

    internal void AddCheckList(IEnumerable<CheckList> newCheckList)
    {
        foreach (var check in newCheckList)
        {
            this.checkList.Add(check.CloneForReport());
        }
    }

    internal void RemoveCheckList(CheckList newCheckList)
    {
        this.checkList.Remove(newCheckList);
    }

    public void AddSignature(IEnumerable<Signature> signature, string userName = "")
    {
        foreach (var sign in signature)
        {
            signatures.Add(sign.PrepareForNewReport(userName));
        }
    }

    internal void RemoveSignature(Signature signature)
    {
        signatures.Remove(signature);
    }

    public void Close()
    {
        if (IsClosed)
            throw new InvalidOperationException("Report is already Closed");

        IsClosed = true;
    }

    public void AddForms(IReadOnlyCollection<ReportForm> configurationForms)
    {
        Guard.Against.Null(configurationForms, nameof(configurationForms));
        _availableForms.AddRange(configurationForms);
    }

    public IList<Signature> CloneSignatures()
    {
        return new List<Signature>(signatures);
    }
}
