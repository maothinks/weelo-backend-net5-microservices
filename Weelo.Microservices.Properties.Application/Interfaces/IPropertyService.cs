﻿using Weelo.Microservices.Properties.Domain.Interfaces;

namespace Weelo.Microservices.Properties.Application.Interfaces
{
    public interface IPropertyService<TPagination, TPaginationMetadata, TEntity, TEntityId>
        : IAdd<TEntity>, IList<TPagination, TPaginationMetadata, TEntity, TEntityId>, IDelete<TEntityId>, IEdit<TEntity>
    { }
}
