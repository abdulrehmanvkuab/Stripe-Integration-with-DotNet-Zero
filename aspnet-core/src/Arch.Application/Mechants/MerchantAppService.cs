using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Arch.Mechants.Dto;
using Arch.Merchants;
using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Arch.Mechants
{
    public class MerchantAppService :AsyncCrudAppService<
         Merchant, MerchantDto, int, PagedMerchantResultRequestDto, CreateMerchantDto, UpdateMerchantDto>, IMerchantAppService
    {
        private readonly IRepository<Merchant, int> _merchantRepository;

        public MerchantAppService(IRepository<Merchant, int> merchantRepository)
            : base(merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task<List<MerchantDto>> GetAllMerchantsAsync()
        {
            var merchants = await _merchantRepository.GetAll().ToListAsync();
            return ObjectMapper.Map<List<MerchantDto>>(merchants);
        }
    }



   
}
