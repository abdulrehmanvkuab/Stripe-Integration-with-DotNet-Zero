using Abp.Application.Services.Dto;

namespace Arch.Notifications.Dto
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}