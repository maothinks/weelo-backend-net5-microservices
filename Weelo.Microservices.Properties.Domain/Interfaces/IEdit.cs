
using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain.Interfaces
{
    public interface IEdit<TEntity>
    {
        Task<bool> EditAsync(TEntity entity);
    }
}
