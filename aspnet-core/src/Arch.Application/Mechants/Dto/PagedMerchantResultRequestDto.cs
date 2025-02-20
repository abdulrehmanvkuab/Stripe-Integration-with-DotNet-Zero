using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Arch.Mechants.Dto
{
    public class PagedMerchantResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
