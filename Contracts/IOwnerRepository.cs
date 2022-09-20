using Entities.Models;
namespace Contracts
{
    public interface IOwnerRepository
    {
        IEnumerable<Owner> GetAllOwners();
    }
}
