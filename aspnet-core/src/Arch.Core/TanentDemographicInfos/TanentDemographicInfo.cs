using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Arch.TanentDemographicInfos
{
    [Table("TanentDemographicInfos")]
    public class TanentDemographicInfo : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        /// <summary>
        /// Is this entity OrganizationalUnit?
        /// </summary>

        public virtual string OrganizationalUnitId { get; set; }

        public virtual string Address { get; set; }

        public virtual string City { get; set; }

        public virtual string State { get; set; }

        public virtual string Country { get; set; }

        public virtual string Zip { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual string FaxNumber { get; set; }

    }
}