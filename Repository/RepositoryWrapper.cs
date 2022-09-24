using Contracts;
using Entities;
using Entities.Models;
using Entities.Helpers;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IAccountRepository _account;
        private ISortHelper<Owner> _sortHelper;
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
                return _owner ?? new OwnerRepository(_repositoryContext, _sortHelper);
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext,ISortHelper<Owner> sortHelper)
        {
            _repositoryContext = repositoryContext;
            _sortHelper = sortHelper;
        }

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
