using Entities;
using Entities.Models;
using Contracts;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public IEnumerable<Owner> GetAllOwners() => 
            FindAll()
            .OrderBy(ow => ow.Name)
            .ToList();
    }
}
