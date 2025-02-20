using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Arch.TenantInfos.Dtos;
using Arch.Dto;

namespace Arch.TenantInfos
{
    public interface ITenantInfosAppService : IApplicationService
    {
        Task<PagedResultDto<GetTenantInfoForViewDto>> GetAll(GetAllTenantInfosInput input);

        Task<GetTenantInfoForEditOutput> GetTenantInfoForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditTenantInfoDto input);

        Task Delete(EntityDto<long> input);

    }
}