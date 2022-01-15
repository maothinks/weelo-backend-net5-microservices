using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TPagination, TPaginationMetadata, TEntity, TEntityId>
        : IAdd<TEntity>, IEdit<TEntity>, IDelete<TEntityId>, IList<TPagination, TPaginationMetadata, TEntity, TEntityId>, ITransaction
    {
        Task<IList<TEntity>> GetAllByParentIdAsync(Guid id);   
    }
}
