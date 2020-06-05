﻿using Inspections.Core.Domain.CheckListAggregate;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inspections.Core.Domain.InspectionReportAggregate
{
    public class InspectionReport : Entity<Guid>, IAggregateRoot
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public EMALicense License { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<Signature> Signatures { get; set; }

        public bool Completed => !_checkList.Any(c => c.Completed);

        private readonly List<Note> _notes = new List<Note>();
        public IReadOnlyList<Note> Notes => _notes.AsReadOnly();

        private readonly List<CheckList> _checkList = new List<CheckList>();
        public IReadOnlyList<CheckList> CheckList => _checkList.AsReadOnly();
    }
}