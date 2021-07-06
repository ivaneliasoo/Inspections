using Ardalis.GuardClauses;
using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspections.Core.Domain.ReportsAggregate
{
    public class Report : Entity<int>, IAggregateRoot
    {
        public string Name { get; private set; } = default!;
        public string Address { get; private set; } = default!;

        public int? EMALicenseId { get; set; }
        public EMALicense? License { get; private set; }
        public int? OperationalReadingsId { get; private set; }
        public OperationalReadings? OperationalReadings { get; private set; }
        public DateTimeOffset Date { get; private set; } = default!;
        public bool IsClosed { get; private set; } = default!;

        public string Title { get; private set; } = default!;
        public string FormName { get; private set; } = default!;
        public string? RemarksLabelText { get; private set; }
        private readonly List<Signature> signatures = new();
        public IReadOnlyCollection<Signature> Signatures => signatures;
        private readonly List<Note> notes = new();
        public IReadOnlyCollection<Note> Notes => notes;

        private readonly List<CheckList> checkList = new();
        public IReadOnlyCollection<CheckList> CheckList => checkList;

        private readonly List<PhotoRecord> photoRecords = new();

        internal Report()
        {

        }

        public Report(string name, string address, EMALicense? license, DateTimeOffset date)
        {
            Name = name;
            Address = address;
            License = license;
            Date = date;
        }

        internal void SetName(string name)
        {
            //CheckIfClosed();
            Name = name;
        }

        public void AddOperationalReadings(OperationalReadings operationalReadings)
        {
            Guard.Against.Null(operationalReadings, nameof(operationalReadings));
            OperationalReadings = operationalReadings;
        }

        public Report(string title, string formName, string remarksLabelText)
            : this(string.Empty, string.Empty, null, DateTimeOffset.UtcNow)
        {
            Title = title;
            FormName = formName;
            RemarksLabelText = remarksLabelText;
        }

        public void Edit(string name, string address, EMALicense license, DateTimeOffset date)
        {
            //CheckIfClosed();
            Name = name;
            Address = address;
            License = license;
            EMALicenseId = license.Id;
            Date = date;
        }

        public IReadOnlyCollection<PhotoRecord> PhotoRecords => photoRecords;
        public bool Completed => !checkList.Any(c => !c.Completed);

        public void AddNote(Note note)
        {
            //CheckIfClosed();
            notes.Add(note);
        }

        //I still believe this is a must, but han pin wants to allow editing after report has been closed
        //private void CheckIfClosed()
        //{
        //    if (IsClosed)
        //        throw new InvalidOperationException("Report is Closed. You Can't edit Closed Reports");
        //}

        public void AddNote(IEnumerable<Note> note)
        {
            //CheckIfClosed();
            notes.AddRange(note);
        }

        public void RemoveNote(Note note)
        {
            //CheckIfClosed();
            notes.Remove(note);
        }

        public void AddPhoto(PhotoRecord photo)
        {
            //CheckIfClosed();
            photoRecords.Add(photo);
        }

        public void AddPhoto(IEnumerable<PhotoRecord> photo)
        {
            //CheckIfClosed();
            photoRecords.AddRange(photo);
        }

        public void RemovePhoto(PhotoRecord photo)
        {
            //CheckIfClosed();
            photoRecords.Remove(photo);
        }

        internal void AddCheckList(IEnumerable<CheckList> checkList)
        {
            //CheckIfClosed();
            foreach (var check in checkList)
            {
                this.checkList.Add(check.CloneForReport());
            }

        }

        internal void RemoveCheckList(CheckList checkList)
        {
            //CheckIfClosed();
            this.checkList.Remove(checkList);
        }

        internal void AddSignature(IEnumerable<Signature> signature, string userName = "")
        {
            //CheckIfClosed();
            foreach (var sign in signature)
            {
                signatures.Add(sign.PreparteForNewReport(userName));
            }
        }

        internal void RemoveSignature(Signature signature)
        {
            //CheckIfClosed();
            signatures.Remove(signature);
        }

        public void Close()
        {
            if (IsClosed)
                throw new InvalidOperationException("Report is already Closed");


            IsClosed = true;
        }
    }
}
