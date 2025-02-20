using Abp.Application.Services.Dto;

namespace Arch.CustomReportss.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public string ExtraFilter { get; set; }
        public string AutoFilter { get; set; }
    }
}