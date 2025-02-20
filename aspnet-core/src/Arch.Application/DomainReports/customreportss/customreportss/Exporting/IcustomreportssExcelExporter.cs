using System.Collections.Generic;
using Arch.CustomReportss.Dtos;
using Arch.Dto;

namespace Arch.CustomReportss.Exporting
{
    public interface ICustomReportssExcelExporter
    {
        FileDto ExportToFile(List<GetCustomReportsForViewDto> CustomReportss);
    }
}