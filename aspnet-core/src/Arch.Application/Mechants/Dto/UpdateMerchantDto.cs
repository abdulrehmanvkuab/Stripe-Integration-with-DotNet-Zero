using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Arch.Mechants.Dto
{
    public class UpdateMerchantDto : IEntityDto<int>  // ✅ Add IEntityDto<int>
    {
        public int Id { get; set; }  // ✅ Required for IEntityDto<int>
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        // Add other properties as needed
    }
}
