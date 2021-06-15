using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;

namespace Inspections.Core.QueryModels
{
    public class ReportQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public DateTimeOffset Date { get; set; }
        public string LicenseNumber { get; set; } = default!;
        public string LicenseName { get; set; } = default!;
        public decimal LicenseKVA { get; set; }
        public decimal LicenseVolt { get; set; }
        public decimal LicenseAmp { get; set; }
        public DateTimeRange LicenseValidity { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string FormName { get; set; } = default!;
        public string? RemarksLabelText { get; set; }
        public int OperationalReadingsId { get; set; }
        public short OperationalReadingsVoltageL1N { get; set; }
        public short OperationalReadingsVoltageL2N { get; set; }
        public short OperationalReadingsVoltageL3N { get; set; }
        public short OperationalReadingsVoltageL1L2 { get; set; }
        public short OperationalReadingsVoltageL1L3 { get; set; }
        public short OperationalReadingsVoltageL2L3 { get; set; }
        public short OperationalReadingsRunningLoadL1 { get; set; }
        public short OperationalReadingsRunningLoadL2 { get; set; }
        public short OperationalReadingsRunningLoadL3 { get; set; }
        public short OperationalReadingsMainBreakerAmp { get; set; }
        public short OperationalReadingsMainBreakerPoles { get; set; }
        public short OperationalReadingsMainBreakerCapacity { get; set; }
        public bool OperationalReadingsOverCurrentByMainBreaker { get; set; }
        public short OperationalReadingsOverCurrentDTLA { get; set; }
        public short OperationalReadingsOverCurrentDTLSec { get; set; }
        public short OperationalReadingsOverCurrentIDMTLA { get; set; }
        public short OperationalReadingsOverCurrentIDMTLTm { get; set; }
        public short OperationalReadingsEarthFaultMA { get; set; }
        public short OperationalReadingsEarthFaultELRA { get; set; }
        public short OperationalReadingsEarthFaultELRSec { get; set; }
        public short OperationalReadingsEarthFaultA { get; set; }
        public short OperationalReadingsEarthFaultSec { get; set; }
        public IEnumerable<SignatureQueryResult> Signatures { get; set; } = default!;
        public IEnumerable<CheckListQueryResult> CheckLists { get; set; } = default!;
    }
}
