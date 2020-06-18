using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Inspections.API.Features.Inspections.Commands
{
    public class UpdateReportCommand : IRequest<bool>
    {
        public UpdateReportCommand()
        {
        }
    }
}
