using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Arch.CustomReportss.Dtos
{
    public class GetCustomReportsForEditOutput
    {
        public CreateOrEditCustomReportsDto CustomReports { get; set; }

    }
}