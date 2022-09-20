using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IAccountRepository _account;
        private IOwnerRepository _owner;
        private RepositoryContext _repositoryContext;
        public IAccountRepository Account { 
            get {
                return _account ?? new AccountRepository(_repositoryContext); 
                
            } }
        public IOwnerRepository Owner
        {
            get
            {
                return _owner ?? new OwnerRepository(_repositoryContext);
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext) =>
            _repositoryContext = repositoryContext;

        public void Save() => _repositoryContext.SaveChanges();
    }
}
