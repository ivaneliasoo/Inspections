using MediatR;

namespace Inspections.API.Features.Signatures.Commands
{
    public class DeleteSignatureCommand : IRequest<bool>
    {
        public DeleteSignatureCommand(int id)
        {
            Id = id;
        }

        private DeleteSignatureCommand() { }

        public int Id { get; }
    }
}
