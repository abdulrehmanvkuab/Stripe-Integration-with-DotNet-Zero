using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Arch.TenantInfos.Dtos;
using Arch.Dto;
using Abp.Application.Services.Dto;
using Arch.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Arch.Storage;

namespace Arch.TenantInfos
{
    [AbpAuthorize(AppPermissions.Pages_TenantInfos)]
    public class TenantInfosAppService : ArchAppServiceBase, ITenantInfosAppService
    {
        private readonly IRepository<TenantInfo, long> _tenantInfoRepository;

        public TenantInfosAppService(IRepository<TenantInfo, long> tenantInfoRepository)
        {
            _tenantInfoRepository = tenantInfoRepository;

        }

        public async Task<PagedResultDto<GetTenantInfoForViewDto>> GetAll(GetAllTenantInfosInput input)
        {

            var filteredTenantInfos = _tenantInfoRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TenantName.Contains(input.Filter) || e.BusinessName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.PhoneNumber.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Country.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TenantNameFilter), e => e.TenantName == input.TenantNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessName == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email == input.EmailFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNumberFilter), e => e.PhoneNumber == input.PhoneNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address == input.AddressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country == input.CountryFilter)
                        .WhereIf(input.MinZipCodeFilter != null, e => e.ZipCode >= input.MinZipCodeFilter)
                        .WhereIf(input.MaxZipCodeFilter != null, e => e.ZipCode <= input.MaxZipCodeFilter)
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status));

            var pagedAndFilteredTenantInfos = filteredTenantInfos
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var tenantInfos = from o in pagedAndFilteredTenantInfos
                              select new
                              {

                                  o.TenantName,
                                  o.BusinessName,
                                  o.Email,
                                  o.PhoneNumber,
                                  o.Address,
                                  o.City,
                                  o.Country,
                                  o.ZipCode,
                                  o.Status,
                                  Id = o.Id
                              };

            var totalCount = await filteredTenantInfos.CountAsync();

            var dbList = await tenantInfos.ToListAsync();
            var results = new List<GetTenantInfoForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTenantInfoForViewDto()
                {
                    TenantInfo = new TenantInfoDto
                    {

                        TenantName = o.TenantName,
                        BusinessName = o.BusinessName,
                        Email = o.Email,
                        PhoneNumber = o.PhoneNumber,
                        Address = o.Address,
                        City = o.City,
                        Country = o.Country,
                        ZipCode = o.ZipCode,
                        Status = o.Status,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTenantInfoForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_TenantInfos_Edit)]
        public async Task<GetTenantInfoForEditOutput> GetTenantInfoForEdit(EntityDto<long> input)
        {
            var tenantInfo = await _tenantInfoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantInfoForEditOutput { TenantInfo = ObjectMapper.Map<CreateOrEditTenantInfoDto>(tenantInfo) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantInfoDto input)
        {
            if (input.Id == null)
            {
                input.CreationTime = UtilityServices.TimeUtils.Now();
                await Create(input);
            }
            else
            {
                input.LastModificationTime = UtilityServices.TimeUtils.Now();
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TenantInfos_Create)]
        protected virtual async Task Create(CreateOrEditTenantInfoDto input)
        {
            var tenantInfo = ObjectMapper.Map<TenantInfo>(input);

            if (AbpSession.TenantId != null)
            {
                tenantInfo.TenantId = (int?)AbpSession.TenantId;
            }

            await _tenantInfoRepository.InsertAsync(tenantInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantInfos_Edit)]
        protected virtual async Task Update(CreateOrEditTenantInfoDto input)
        {
            var tenantInfo = await _tenantInfoRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, tenantInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_TenantInfos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _tenantInfoRepository.DeleteAsync(input.Id);
        }

    }
}