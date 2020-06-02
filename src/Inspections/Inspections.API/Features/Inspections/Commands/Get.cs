using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Inspections.API.Features.Inspections.Commands
{
    public class Get : IRequest<bool>
    {
        public Get()
        {
        }
    }
}
