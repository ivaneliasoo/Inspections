using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public record UpdateOperationalReadingsCommand : IRequest<bool>
    {
        public int Id { get; init; }
        public int ReportId { get; init; }
        public short VoltageL1N { get; init; }
        public short VoltageL2N { get; init; }
        public short VoltageL3N { get; init; }
        public short VoltageL1L2 { get; init; }
        public short VoltageL1L3 { get; init; }
        public short VoltageL2L3 { get; init; }
        public short RunningLoadL1 { get; init; }
        public short RunningLoadL2 { get; init; }
        public short RunningLoadL3 { get; init; }
        public short MainBreakerAmp { get; init; }
        public short MainBreakerPoles { get; init; }
        public short MainBreakerCapacity { get; init; }
        public short OverCurrentDTLA { get; init; }
        public short OverCurrentDTLSec { get; init; }
        public short OverCurrentIDMTLA { get; init; }
        public short OverCurrentIDMTLTm { get; init; }
        public short EarthFaultMA { get; init; }
        public short EarthFaultELRA { get; init; }
        public short EarthFaultELRSec { get; init; }
        public short EarthFaultA { get; init; }
        public short EarthFaultSec { get; init; }
        public short MainBreakerRating { get; init; }
        public bool OverCurrentDirectActingEnabled { get; init; }
        public short OverCurrentDirectActing { get; init; }
        public bool OverCurrentDTLEnabled { get; init; }
        public bool OverCurrentIDTMLEnabled { get; init; }
        public bool EarthFaultRoobEnabled { get; init; }
        public bool EarthFaultEIREnabled { get; init; }
        public bool EarthFaultEFEnabled { get; init; }
    }
}
