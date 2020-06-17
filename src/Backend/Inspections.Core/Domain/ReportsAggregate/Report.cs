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
        public string Name { get; private set; }
        public string Address { get; private set; }
        public EMALicense License { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public bool IsClosed { get; private set; }

        public string Title { get; private set; }
        public string FormName { get; private set; }
        public string RemarksLabelText { get; private set; }

        private readonly List<Signature> signatures = new List<Signature>();
        public IReadOnlyCollection<Signature> Signatures => signatures;
        private readonly List<Note> notes = new List<Note>();
        public IReadOnlyCollection<Note> Notes => notes;

        private readonly List<CheckList> checkList = new List<CheckList>();
        public IReadOnlyCollection<CheckList> CheckList => checkList;

        private readonly List<PhotoRecord> photoRecords = new List<PhotoRecord>();

        internal Report()
        {

        }

        public Report(string name, string address, EMALicense license, DateTimeOffset date)
        {
            Name = name;
            Address = address;
            License = license;
            Date = date;
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
            CheckIsClosed();
            Name = name;
            Address = address;
            License = license;
            Date = date;
        }

        public IReadOnlyCollection<PhotoRecord> PhotoRecords => photoRecords;
        public bool Completed => !checkList.Any(c => c.Completed);

        public void AddNote(Note note)
        {
            CheckIsClosed();
            notes.Add(note);
        }

        private void CheckIsClosed()
        {
            if (IsClosed)
                throw new InvalidOperationException("Report is Closed. You Can't edit Closed Reports");
        }

        public void AddNote(IEnumerable<Note> note)
        {
            CheckIsClosed();
            notes.AddRange(note);
        }

        public void RemoveNote(Note note)
        {
            CheckIsClosed();
            notes.Remove(note);
        }

        public void AddPhoto(PhotoRecord photo)
        {
            CheckIsClosed();
            photoRecords.Add(photo);
        }

        public void AddPhoto(IEnumerable<PhotoRecord> photo)
        {
            CheckIsClosed();
            photoRecords.AddRange(photo);
        }

        public void RemovePhoto(PhotoRecord photo)
        {
            CheckIsClosed();
            photoRecords.Remove(photo);
        }

        internal void AddCheckList(IEnumerable<CheckList> checkList)
        {
            CheckIsClosed();
            foreach (var check in checkList)
            {
                this.checkList.Add(check.PreparteForNewReport());
            }
           
        }

        internal void RemoveCheckList(CheckList checkList)
        {
            CheckIsClosed();
            this.checkList.Remove(checkList);
        }

        internal void AddSignature(IEnumerable<Signature> signature)
        {
            CheckIsClosed();
            foreach (var sign in signature)
            {
                signatures.Add(sign.PreparteForNewReport());
            }
        }

        internal void RemoveSignature(Signature signature)
        {
            CheckIsClosed();
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
