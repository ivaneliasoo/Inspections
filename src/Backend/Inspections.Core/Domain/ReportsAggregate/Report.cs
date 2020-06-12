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
        public string Name { get; set; }
        public string Address { get; set; }
        public EMALicense License { get; set; }
        public DateTimeOffset Date { get; set; }

        private readonly List<Signature> signatures = new List<Signature>();
        public IReadOnlyCollection<Signature> Signatures => signatures;


        private readonly List<Note> notes = new List<Note>();
        public IReadOnlyCollection<Note> Notes => notes;

        private readonly List<CheckList> checkList = new List<CheckList>();
        public IReadOnlyCollection<CheckList> CheckList => checkList;

        private readonly List<PhotoRecord> photoRecords = new List<PhotoRecord>();
        public IReadOnlyCollection<PhotoRecord> PhotoRecords => photoRecords;
        public bool Completed => !checkList.Any(c => c.Completed);
    }
}
