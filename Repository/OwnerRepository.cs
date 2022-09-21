using Entities;
using Entities.Models;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync() => 
            await FindAll()
            .OrderBy(ow => ow.Name)
            .ToListAsync();
    }
}
