using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.Mechants.Dto
{
    public class CreateMerchantDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
