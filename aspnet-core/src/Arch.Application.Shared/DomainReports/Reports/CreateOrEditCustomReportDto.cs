using Abp.Application.Services.Dto;

namespace Arch.Reports
{
    public class CreateOrEditCustomReportDto : EntityDto
    {
        public string ReportName { get; set; }

        public string ReportSchema { get; set; }
    }
}
