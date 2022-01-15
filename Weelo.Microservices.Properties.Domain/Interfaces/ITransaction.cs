
using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain.Interfaces
{
    public interface ITransaction
    {
        Task<int> saveAllChangesAsync();
    }
}
