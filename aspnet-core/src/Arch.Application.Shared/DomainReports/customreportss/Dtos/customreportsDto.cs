using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Arch.Domain;

namespace Arch.CustomReportss.Dtos
{

    public class CustomReportsDto : AuditedEntity<long>
    {
        public string ReportID { get; set; }

        public string ReportName { get; set; }

        public string ReportSchema { get; set; }
        public bool IsDeleted { get; set; }



    }
}