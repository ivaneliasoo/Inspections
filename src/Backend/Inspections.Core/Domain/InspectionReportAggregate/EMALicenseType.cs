using Inspections.Shared;

namespace Inspections.Core.Domain.InspectionReportAggregate
{
    public class EMALIcenseType : Enumeration
    {
        public static EMALIcenseType Electrician = new EMALIcenseType(1, "Electrician", "E");
        public static EMALIcenseType ElectricalTechnician = new EMALIcenseType(2, "Electrical Technician", "ET");
        public static EMALIcenseType ElectricalEngineer = new EMALIcenseType(3, "Electrical Engeneer", "EE");

        public EMALIcenseType(int id, string name, string code)
            : base(id, name)
        {
            Code = code;
        }

        public string Code { get; }
    }
}