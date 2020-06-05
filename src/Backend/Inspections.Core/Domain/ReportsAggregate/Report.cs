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

        private readonly List<Signature> _signatures = new List<Signature>();
        public IReadOnlyList<Signature> Signatures => _signatures.AsReadOnly();


        private readonly List<Note> _notes = new List<Note>();
        public IReadOnlyList<Note> Notes => _notes.AsReadOnly();

        private readonly List<CheckList> _checkList = new List<CheckList>();
        public IReadOnlyList<CheckList> CheckList => _checkList.AsReadOnly();

        private readonly List<PhotoRecord> _photoRecord = new List<PhotoRecord>();
        public IReadOnlyList<PhotoRecord> PhotoRecords => _photoRecord.AsReadOnly();
        public bool Completed => !_checkList.Any(c => c.Completed);
    }
}
