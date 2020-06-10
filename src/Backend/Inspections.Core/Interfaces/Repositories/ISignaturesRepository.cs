using Inspections.Core.Domain.SignaturesAggregate;
using Inspections.Shared;
using System.Threading.Tasks;

namespace Inspections.Core.Interfaces
{
    public interface ISignaturesRepository : IAsyncRepository<Signature>
    {
    }
}
