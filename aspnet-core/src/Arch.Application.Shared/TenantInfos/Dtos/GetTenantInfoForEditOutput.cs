using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Arch.TenantInfos.Dtos
{
    public class GetTenantInfoForEditOutput
    {
        public CreateOrEditTenantInfoDto TenantInfo { get; set; }

    }
}