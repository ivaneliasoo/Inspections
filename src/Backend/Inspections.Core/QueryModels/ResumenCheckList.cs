using System;
using System.Collections.Generic;
using System.Text;

namespace Inspections.Core.QueryModels
{
    public class ResumenCheckList
    {
        public int Id { get; set; }
        public string Text { get; private set; }
        public string Annotation { get; private set; }
        public int TotalItems { get; set; }
        public int TotalParams { get; set; }

    }
}
