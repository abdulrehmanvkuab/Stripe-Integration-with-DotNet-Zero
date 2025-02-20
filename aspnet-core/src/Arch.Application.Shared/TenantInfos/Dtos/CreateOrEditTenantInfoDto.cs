using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Arch.TenantInfos.Dtos
{
    public class CreateOrEditTenantInfoDto : AuditedEntityDto<long?>
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