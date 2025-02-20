using Abp.Application.Services.Dto;
using System;
using Arch.Dto;

namespace Arch.CustomReportss.Dtos
{
    public class GetAllCustomReportssInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public ProcessActionDto ProcessAction { get; set; }

        public string ReportID { get; set; }

        public string ReportName { get; set; }

        public string ReportSchema { get; set; }
        public int? IsDeleted { get; set; }

        public DateTime? MaxLastModificationTimeFilter { get; set; }
        public DateTime? MinLastModificationTimeFilter { get; set; }

        public long? MaxLastModifierUserIdFilter { get; set; }
        public long? MinLastModifierUserIdFilter { get; set; }

        public long? MaxCreatorUserIdFilter { get; set; }
        public long? MinCreatorUserIdFilter { get; set; }

        public DateTime? MaxCreationTimeFilter { get; set; }
        public DateTime? MinCreationTimeFilter { get; set; }

    }
}