using Microsoft.AspNetCore.Mvc;
using MediatR;
using Inspections.Core.Domain.ReportsAggregate;
using System;

namespace Inspections.API.Features.Inspections.Commands
{
    public class UpdateReportCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get;  set; }
        public string Address { get;  set; }
        public EMALicenseType LicenseType { get; set; }
        public string LicenseNumber { get; set; }
        public DateTimeOffset Date { get;  set; }
        public bool IsClosed { get;  set; }
        internal UpdateReportCommand()
        {

        }

        public UpdateReportCommand(int id, string name, string address, EMALicenseType licenseType, string licenseNumber, DateTimeOffset date, bool isClosed)
        {
            Name = name;
            Address = address;
            LicenseType = licenseType;
            LicenseNumber = licenseNumber;
            Date = date;
            IsClosed = isClosed;
            Id = id;
        }
    }
}
