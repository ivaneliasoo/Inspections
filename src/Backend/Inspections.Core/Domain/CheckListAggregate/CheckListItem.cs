﻿using Inspections.Shared;
using System.Collections.Generic;

namespace Inspections.Core.Domain.CheckListAggregate
{
    public class CheckListItem : Entity<int>
    {
        public string Text { get; set; }
        public List<CheckListParam> TextParams { get; set; }
        public CheckValue Checked { get; set; }
        public bool Required { get; set; }
        public string Remarks { get; set; }
    }
}
