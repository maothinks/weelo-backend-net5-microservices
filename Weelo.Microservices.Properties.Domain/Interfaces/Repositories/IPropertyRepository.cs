using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository<TEntity, TEntityId>
        : IAdd<TEntity>, IEdit<TEntity>, IDelete<TEntityId>, ITransaction
    { }
}
