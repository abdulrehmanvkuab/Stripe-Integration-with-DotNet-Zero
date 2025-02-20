using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Arch.Mechants.Dto
{
    public class MerchantDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
