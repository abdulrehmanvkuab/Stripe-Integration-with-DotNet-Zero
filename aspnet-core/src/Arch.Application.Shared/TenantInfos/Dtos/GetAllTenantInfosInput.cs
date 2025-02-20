using Abp.Application.Services.Dto;
using System;

namespace Arch.TenantInfos.Dtos
{
    public class GetAllTenantInfosInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TenantNameFilter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string EmailFilter { get; set; }

        public string PhoneNumberFilter { get; set; }

        public string AddressFilter { get; set; }

        public string CityFilter { get; set; }

        public string CountryFilter { get; set; }

        public long? MaxZipCodeFilter { get; set; }
        public long? MinZipCodeFilter { get; set; }

        public int? StatusFilter { get; set; }

    }
}