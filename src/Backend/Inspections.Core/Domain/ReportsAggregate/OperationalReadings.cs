using System;
using System.Collections.Generic;
using System.Text;
using Inspections.Shared;

namespace Inspections.Core.Domain.ReportsAggregate
{
    public class OperationalReadings : Entity<int>
    {
        public short VoltageL1N { get; set; }
        public short VoltageL2N { get; set; }
        public short VoltageL3N { get; set; }
        public short VoltageL1L2 { get; set; }
        public short VoltageL1L3 { get; set; }
        public short VoltageL2L3 { get; set; }
        public short RunningLoadL1 { get; set; }
        public short RunningLoadL2 { get; set; }
        public short RunningLoadL3 { get; set; }
        public short MainBreakerAmp { get; set; }
        public short MainBreakerPoles { get; set; }
        public short MainBreakerCapacity { get; set; }
        public bool OverCurrentByMainBreaker { get; set; }
        public short OverCurrentDTLA { get; set; }
        public short OverCurrentDTLSec { get; set; }
        public short OverCurrentIDMTLA { get; set; }
        public short OverCurrentIDMTLTm { get; set; }
        public short EarthFaultMA { get; set; }
        public short EarthFaultELRA { get; set; }
        public short EarthFaultELRSec { get; set; }
        public short EarthFaultA { get; set; }
        public short EarthFaultSec { get; set; }

        public OperationalReadings()
        {

        }
    }
}
