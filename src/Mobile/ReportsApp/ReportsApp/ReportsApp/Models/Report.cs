using System;
using System.Collections.Generic;
using System.Text;

namespace ReportsApp.Models
{
    public class Report
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public License License { get; set; }
        public DateTime Date { get; set; }
        public bool IsClosed { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string RemarksLabelText { get; set; }
        public List<Signature> Signatures { get; set; }
        public List<Note> Notes { get; set; }
        public List<CheckList> CheckList { get; set; }
        public List<object> PhotoRecords { get; set; }
        public bool Completed { get; set; }
        public bool NotCompleted => !Completed;
        public bool IsNotClosed => !IsClosed;
        public int Id { get; set; }
    }
}
