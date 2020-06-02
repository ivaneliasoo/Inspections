using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Inspections.API.Features.Inspections.Commands
{
    public class Update : IRequest<bool>
    {
        public Update()
        {
        }
    }
}
