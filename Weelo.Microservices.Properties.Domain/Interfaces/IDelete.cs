

using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain.Interfaces
{
    public interface IDelete<TEntityId>
    {
        Task<bool> DeleteAsync(TEntityId entityId);

    }
}
