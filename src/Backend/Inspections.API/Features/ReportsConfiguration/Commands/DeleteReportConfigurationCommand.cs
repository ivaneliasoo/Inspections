using MediatR;

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
