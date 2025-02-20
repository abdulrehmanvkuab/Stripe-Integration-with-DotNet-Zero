using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.Dto
{

    public class ProcessActionDto
    {
        public long? Id { get; set; }
        public string ListofId { get; set; }
        public string PageType { get; set; }
        public string ProcessHeader { get; set; }
        public string StaticFilter { get; set; }
        public string ActionName { get; set; }

    }

   
}
