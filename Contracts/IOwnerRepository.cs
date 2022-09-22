using Entities.Models;
using Entities.Helpers;

namespace Contracts
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllOwnersAsync();
        //Task<IEnumerable<Owner>> GetOwnersAsync(OwnerParameters ownerParameters);
        Task<PagedList<Owner>> GetOwnersAsync(OwnerParameters ownerParameters);
        Task<Owner?> GetOwnerByIdAsync(Guid id);  
        void CreateOwner(Owner owner);
    }
}
