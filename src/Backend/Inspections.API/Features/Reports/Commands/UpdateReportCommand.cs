using System;
using JetBrains.Annotations;
using MediatR;

namespace Inspections.API.Features.Reports.Commands
{
    public class UpdateReportCommand : IRequest<bool>
    {
        public int Id { get;  }
        public string Name { [UsedImplicitly] get;  } = default!;
        public string Address { get;  } = default!;
        public string LicenseNumber { get;  } = default!;
        public DateTimeOffset Date { get;  }
        public bool IsClosed { get;  }
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
