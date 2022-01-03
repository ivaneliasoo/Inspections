using MediatR;
using System;

namespace Inspections.API.Features.Inspections.Commands
{
    public class UpdateReportCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get;  set; } = default!;
        public string Address { get; set; } = default!;
        public string LicenseNumber { get; set; } = default!;
        public DateTimeOffset Date { get;  set; }
        public bool IsClosed { get;  set; }
        internal UpdateReportCommand()
        {

        }

        public UpdateReportCommand(int id, string name, string address, string licenseNumber, DateTimeOffset date, bool isClosed)
        {
            Name = name;
            Address = address;
            LicenseNumber = licenseNumber;
            Date = date;
            IsClosed = isClosed;
            Id = id;
        }
    }
}
