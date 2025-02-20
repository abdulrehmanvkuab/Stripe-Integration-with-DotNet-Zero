using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Arch.TenantInfos
{
    [Table("TenantInfos")]
    [Audited]
    public class TenantInfo : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        /// <summary>
        /// Is this entity OrganizationalUnit?
        /// </summary>

        public virtual string OrganizationalUnitId { get; set; }

        public virtual string TenantName { get; set; }

        public virtual string BusinessName { get; set; }

        public virtual string Email { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual string Address { get; set; }

        public virtual string City { get; set; }

        public virtual string Country { get; set; }

        public virtual long ZipCode { get; set; }

        public virtual bool Status { get; set; }

    }
}