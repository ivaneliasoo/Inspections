using Inspections.Core.Domain.InspectionsAggregate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Inspections.API.Features.Inspections.Commands
{
    public class Create : IRequest<bool>
    {
        public Create()
        {
        }
    }
}
