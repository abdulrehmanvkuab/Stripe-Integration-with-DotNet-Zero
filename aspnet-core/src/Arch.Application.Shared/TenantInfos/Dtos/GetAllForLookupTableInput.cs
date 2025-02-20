using Abp.Application.Services.Dto;

namespace Arch.TenantInfos.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}