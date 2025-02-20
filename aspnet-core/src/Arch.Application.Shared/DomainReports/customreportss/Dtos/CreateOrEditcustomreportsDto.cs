using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using Arch.Domain;

namespace Arch.CustomReportss.Dtos
{
    public class CreateOrEditCustomReportsDto : EntityDto<long?>
    {

        public string ReportID { get; set; }

       
        public string ReportName { get; set; }

        public string ReportSchema { get; set; }
        public string ProcessHeader { get; set; }
    }
}