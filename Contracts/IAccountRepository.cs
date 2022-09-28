using Entities.Models;
using Entities;

namespace Contracts
{
    public interface IAccountRepository 
    {
        IEnumerable<Account> AccountsByOwner(Guid ownerId);
    }
}
