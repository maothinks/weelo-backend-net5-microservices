using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain.Interfaces
{
    public interface IList<TParams, TPaginationMetadata, TEntity, TEntityId>
    {
        Task<IList<TEntity>> GetAllAsync(TParams paramsDto);

        Task<TPaginationMetadata> GetAllMetadataAsync(TParams paramsDto);

        Task<TEntity> GetByIdAsync(Guid entityId);
    }
}
