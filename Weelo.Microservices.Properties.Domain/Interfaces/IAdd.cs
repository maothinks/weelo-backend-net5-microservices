using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain.Interfaces
{
    public interface IAdd<TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity);
    }
}
