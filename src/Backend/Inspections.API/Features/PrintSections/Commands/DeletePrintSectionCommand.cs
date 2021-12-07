using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Inspections.API.Features.PrintSections.Commands
{

    public class DeletePrintSectionCommand : IRequest<bool>
    {
        public DeletePrintSectionCommand(int id)
        {
            Id = id;
        }

        private DeletePrintSectionCommand() { }

        public int Id { get; set; }
    }
}
