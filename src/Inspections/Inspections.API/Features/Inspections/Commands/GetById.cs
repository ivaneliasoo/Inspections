using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Inspections.API.Features.Inspections.Commands
{
    public class GetById : IRequest<bool>
    {
        public GetById()
        {
        }
    }
}
