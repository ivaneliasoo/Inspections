using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Inspections.API.Features.Inspections.Commands
{
    public class Delete : IRequest<bool>
    {
        public Delete()
        {
        }
    }
}
