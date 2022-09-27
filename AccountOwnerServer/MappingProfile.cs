using Entities.Models;
using Entities.DataTransferObjects;
using AutoMapper;

namespace AccountOwnerServer
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerForCreateDto,Owner>();
            CreateMap<Account, AccountDto>();
            CreateMap<OwnerForUpdateDto, Owner>();
        }
    }
}
