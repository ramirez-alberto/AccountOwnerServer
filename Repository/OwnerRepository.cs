using Entities;
using Entities.Models;
using Entities.Helpers;
using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;

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

        public async Task<PagedList<Owner>> GetOwnersAsync(OwnerParameters ownerParameters)
        {
            var owners = FindByCondition(o => o.DateOfBirth.Year >= ownerParameters.MinYearOfBirth &&
                    o.DateOfBirth.Year <= ownerParameters.MaxYearOfBirth);
            SearchByName(ref owners, ownerParameters.Name);
            ApplySort(ref owners, ownerParameters.OrderBy);

            return await PagedList<Owner>.ToPagedListAsync(owners
                , ownerParameters.PageNumber
                , ownerParameters.PageSize);
        }

        private void SearchByName(ref IQueryable<Owner> owners, string ownerName)
        {
            if (!owners.Any() || string.IsNullOrWhiteSpace(ownerName))
                return;

            owners = owners.Where(o => o.Name.ToLower().Trim().Contains(ownerName.Trim().ToLower()));
        }

        private void ApplySort(ref IQueryable<Owner> owners, string orderByQueryString)
        {
            if (!owners.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                owners = owners.OrderBy(o => o.Name);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(",");
            var propertyInfos = typeof(Owner).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new System.Text.StringBuilder();

            foreach (string param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(x => x.Name.Equals(propertyFromQueryName));
                if (objectProperty == null)
                    continue;
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',',' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                owners = owners.OrderBy(o => o.Name);
                return;
            }
            owners = owners.OrderBy(orderQuery);
        }

        public async Task<Owner?> GetOwnerByIdAsync(Guid id) =>
            await FindByCondition(ow => ow.Id.Equals(id)).FirstOrDefaultAsync();

        public void CreateOwner(Owner owner) =>
            Create(owner);
    }
}
