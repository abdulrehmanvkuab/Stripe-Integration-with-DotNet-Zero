using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Controllers;
using Arch.Mechants.Dto;
using Arch.Mechants;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Arch.Web.Controllers
{
  
        [Route("api/[controller]")]
        [ApiController]
        public class MerchantController : AbpController
        {
            private readonly IMerchantAppService _merchantAppService;

            public MerchantController(IMerchantAppService merchantAppService)
            {
                _merchantAppService = merchantAppService;
            }

            [HttpGet]
            public async Task<List<MerchantDto>> GetAll()
            {
                return await _merchantAppService.GetAllMerchantsAsync();
            }

            [HttpPost]
            public async Task<MerchantDto> Create([FromBody] CreateMerchantDto input)
            {
                return await _merchantAppService.CreateAsync(input);
            }

            [HttpPut("{id}")]
            public async Task<MerchantDto> Update(int id, [FromBody] UpdateMerchantDto input)
            {
                input.Id = id;
                return await _merchantAppService.UpdateAsync(input);
            }

            [HttpDelete("{id}")]
            public async Task Delete(int id)
            {
                await _merchantAppService.DeleteAsync(new EntityDto<int> { Id = id });
            }
        
    }
}
