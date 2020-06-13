using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Features.ReportsConfiguration.Commands
{
    public class DeleteReportConfigurationCommand : IRequest<bool>
    {
        public int Id { get; set; }
        private DeleteReportConfigurationCommand()
        {

        }
        public DeleteReportConfigurationCommand(int id)
        {
            Id = id;
        }
    }
}
