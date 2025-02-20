using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Arch.EmailHandlers
{
    [Table("EmailHandlers")]
    public class EmailHandler : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        /// <summary>
        /// Is this entity OrganizationalUnit?
        /// </summary>

        public virtual string OrganizationalUnitId { get; set; }

        public virtual string Content { get; set; }

        [Required]
        public virtual string EmailAddress { get; set; }

        public virtual bool IsSent { get; set; }

        public virtual bool IsWaiting { get; set; }

        public virtual int? RetryCount { get; set; }

        public virtual long? SentOn { get; set; }

        public virtual string Subject { get; set; }

        public virtual string FileName { get; set; }

        public virtual string CcComaSeperated { get; set; }

        public virtual string BccComaSeperated { get; set; }

        public virtual DateTime? EmailSentDate { get; set; }

    }
}