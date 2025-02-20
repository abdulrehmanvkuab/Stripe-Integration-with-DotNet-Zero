using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.Website
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Abp.Domain.Entities.Auditing;

    namespace Acme.PhoneBookDemo.PhoneBook
    {
        [Table("Persons")]
        public class Person : FullAuditedEntity
        {
            public const int MaxNameLength = 32;
            public const int MaxSurnameLength = 32;
            public const int MaxEmailAddressLength = 255;

            [Required]
            [MaxLength(MaxNameLength)]
            public virtual string Name { get; set; }

            [Required]
            [MaxLength(MaxSurnameLength)]
            public virtual string Surname { get; set; }

            [MaxLength(MaxEmailAddressLength)]
            public virtual string EmailAddress { get; set; }
        }
    }

}
