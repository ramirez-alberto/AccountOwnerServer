using Entities;
using Entities.Models;
using Entities.Helpers;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        private ISortHelper<Owner> _sortHelper;
        public OwnerRepository(RepositoryContext repositoryContext, ISortHelper<Owner> sortHelper)
            : base(repositoryContext) { _sortHelper = sortHelper; }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync() =>
            await FindAll()
            .OrderBy(ow => ow.Name)
            .ToListAsync();

        public async Task<PagedList<Owner>> GetOwnersAsync(OwnerParameters ownerParameters)
        {
            var owners = FindByCondition(o => o.DateOfBirth.Year >= ownerParameters.MinYearOfBirth &&
                    o.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth);
            SearchByName(ref owners, ownerParameters.Name);
            var sortedOwners  = _sortHelper.ApplySort(owners, ownerParameters.OrderBy);

            return await PagedList<Owner>.ToPagedListAsync(sortedOwners
                , ownerParameters.PageNumber
                , ownerParameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Owner> owners, string ownerName)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(ownerName))
                return;

            owners = owners.Where(o => o.Name.ToLower().Trim().Contains(ownerName.Trim().ToLower()));
        }
        public async Task<Owner?> GetOwnerByIdAsync(Guid id) =>
            await FindByCondition(ow => ow.Id.Equals(id)).FirstOrDefaultAsync();

        public void CreateOwner(Owner owner) =>
            Create(owner);
    }
}
