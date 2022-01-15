using Weelo.Microservices.Properties.Domain.Interfaces;

namespace Weelo.Microservices.Properties.Application.Interfaces
{
    interface IBaseService<TPagination, TPaginationMetadata, TEntity, TEntityId>
        : IAdd<TEntity>, IEdit<TEntity>, IDelete<TEntityId>, IList<TPagination, TPaginationMetadata, TEntity, TEntityId>
    { }
}
