using System;
using Abp.Application.Services.Dto;

namespace Arch.TenantInfos.Dtos
{
    public class TenantInfoDto : EntityDto<long>
    {
        public string TenantName { get; set; }

        public string BusinessName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public long ZipCode { get; set; }

        public bool Status { get; set; }

    }
}