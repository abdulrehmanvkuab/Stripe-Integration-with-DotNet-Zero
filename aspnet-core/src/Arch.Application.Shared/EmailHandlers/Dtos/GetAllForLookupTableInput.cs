using Abp.Application.Services.Dto;

namespace Arch.EmailHandlers.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}