using Entities;
using Entities.Models;
using Entities.Helpers;
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

        public async Task<PagedList<Owner>> GetOwnersAsync(OwnerParameters ownerParameters) =>
            await PagedList<Owner>.ToPagedListAsync(
                FindByCondition(o => o.DateOfBirth.Year >= ownerParameters.MinYearOfBirth &&
                    o.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth)
                .OrderBy(ow => ow.Name)
                ,ownerParameters.PageNumber
                ,ownerParameters.PageSize);

        public async Task<Owner?> GetOwnerByIdAsync(Guid id) =>
            await FindByCondition(ow => ow.Id.Equals(id)).FirstOrDefaultAsync();

        public void CreateOwner(Owner owner) =>
            Create(owner);
    }
}
