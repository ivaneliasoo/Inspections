using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Inspections.API.Features.Inspections.Commands
{
    public class DeleteReportCommand : IRequest<bool>
    {
        public int Id { get; set; }
        private DeleteReportCommand()
        {
        }
        public DeleteReportCommand(int id)
        {
            Id = id;
        }
    }
}
