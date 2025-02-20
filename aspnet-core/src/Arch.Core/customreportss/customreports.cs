using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Arch.CustomReportss
{
    [Table("CustomReports")]
    public class CustomReports : AuditedEntity<long>, IMustHaveTenant
    {

        public  int TenantId { get; set; }
        public  string OrganizationalUnitId { get; set; }

     
        public virtual string ReportID { get; set; }

     
        public virtual string ReportName { get; set; }


        public string ReportSchema { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? LastModificationTime { get; set; }

        public virtual long? LastModifierUserId { get; set; }

        public virtual long? CreatorUserId { get; set; }

        public virtual DateTime CreationTime { get; set; }

    }
}