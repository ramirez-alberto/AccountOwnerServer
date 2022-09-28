using Entities;
using Entities.Models;
using Contracts;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public IEnumerable<Account> AccountsByOwner(Guid ownerId)
        {
                return FindByCondition(a => a.OwnerId.Equals(ownerId)).ToList();
        }
        public AccountRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
    }
}
