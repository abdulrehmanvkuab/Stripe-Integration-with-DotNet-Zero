using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Arch.Mechants.Dto;

namespace Arch.Mechants
{
    public interface IMerchantAppService : IAsyncCrudAppService<
           MerchantDto, int, PagedMerchantResultRequestDto, CreateMerchantDto, UpdateMerchantDto>
    {
        Task<List<MerchantDto>> GetAllMerchantsAsync();
    }
}
