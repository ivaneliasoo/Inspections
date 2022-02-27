using MediatR;

namespace Inspections.API.Features.Reports.Commands;

public record UpdateOperationalReadingsCommand : IRequest<bool>
{
    public UpdateOperationalReadingsCommand(int id, int reportId, short voltageL1N, short voltageL2N, short voltageL3N, short voltageL1L2, short voltageL1L3, short voltageL2L3, short runningLoadL1, short runningLoadL2, short runningLoadL3, short mainBreakerAmp, short mainBreakerPoles, short mainBreakerCapacity, short overCurrentDtla, short overCurrentDtlSec, short overCurrentIdmtla, short overCurrentIdmtlTm, short earthFaultMa, short earthFaultElra, short earthFaultElrSec, short earthFaultA, short earthFaultSec, short mainBreakerRating, bool overCurrentDirectActingEnabled, short overCurrentDirectActing, bool overCurrentDtlEnabled, bool overCurrentIdtmlEnabled, bool earthFaultRoobEnabled, bool earthFaultEirEnabled, bool earthFaultEfEnabled)
    {
        Id = id;
        ReportId = reportId;
        VoltageL1N = voltageL1N;
        VoltageL2N = voltageL2N;
        VoltageL3N = voltageL3N;
        VoltageL1L2 = voltageL1L2;
        VoltageL1L3 = voltageL1L3;
        VoltageL2L3 = voltageL2L3;
        RunningLoadL1 = runningLoadL1;
        RunningLoadL2 = runningLoadL2;
        RunningLoadL3 = runningLoadL3;
        MainBreakerAmp = mainBreakerAmp;
        MainBreakerPoles = mainBreakerPoles;
        MainBreakerCapacity = mainBreakerCapacity;
        OverCurrentDTLA = overCurrentDtla;
        OverCurrentDTLSec = overCurrentDtlSec;
        OverCurrentIDMTLA = overCurrentIdmtla;
        OverCurrentIDMTLTm = overCurrentIdmtlTm;
        EarthFaultMA = earthFaultMa;
        EarthFaultELRA = earthFaultElra;
        EarthFaultELRSec = earthFaultElrSec;
        EarthFaultA = earthFaultA;
        EarthFaultSec = earthFaultSec;
        MainBreakerRating = mainBreakerRating;
        OverCurrentDirectActingEnabled = overCurrentDirectActingEnabled;
        OverCurrentDirectActing = overCurrentDirectActing;
        OverCurrentDTLEnabled = overCurrentDtlEnabled;
        OverCurrentIDTMLEnabled = overCurrentIdtmlEnabled;
        EarthFaultRoobEnabled = earthFaultRoobEnabled;
        EarthFaultEIREnabled = earthFaultEirEnabled;
        EarthFaultEFEnabled = earthFaultEfEnabled;
    }

    public int Id { get; }
    public int ReportId { get; }
    public short VoltageL1N { get; }
    public short VoltageL2N { get; }
    public short VoltageL3N { get; }
    public short VoltageL1L2 { get; }
    public short VoltageL1L3 { get; }
    public short VoltageL2L3 { get; }
    public short RunningLoadL1 { get; }
    public short RunningLoadL2 { get; }
    public short RunningLoadL3 { get; }
    public short MainBreakerAmp { get; }
    public short MainBreakerPoles { get; }
    public short MainBreakerCapacity { get; }
    public short OverCurrentDTLA { get; }
    public short OverCurrentDTLSec { get; }
    public short OverCurrentIDMTLA { get; }
    public short OverCurrentIDMTLTm { get; }
    public short EarthFaultMA { get; }
    public short EarthFaultELRA { get; }
    public short EarthFaultELRSec { get; }
    public short EarthFaultA { get; }
    public short EarthFaultSec { get; }
    public short MainBreakerRating { get; }
    public bool OverCurrentDirectActingEnabled { get; }
    public short OverCurrentDirectActing { get; }
    public bool OverCurrentDTLEnabled { get; }
    public bool OverCurrentIDTMLEnabled { get; }
    public bool EarthFaultRoobEnabled { get; }
    public bool EarthFaultEIREnabled { get; }
    public bool EarthFaultEFEnabled { get; }
}
