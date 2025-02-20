using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Arch.Merchants
{
    public class Merchant : FullAuditedEntity<int>
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
