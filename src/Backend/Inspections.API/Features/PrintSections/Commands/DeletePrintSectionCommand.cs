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
