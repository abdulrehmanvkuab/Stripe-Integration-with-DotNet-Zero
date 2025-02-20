using AutoMapper;
using Arch.Merchants;

using Arch.Mechants.Dto;

public class MerchantMappingProfile : Profile
{
    public MerchantMappingProfile()
    {
        // Entity to DTO Mapping
        CreateMap<Merchant, MerchantDto>();

        // DTO to Entity Mapping
        CreateMap<CreateMerchantDto, Merchant>();
        CreateMap<UpdateMerchantDto, Merchant>();
    }
}
