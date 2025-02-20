using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.SendEmail
{
    public class SendEmailRequestModel
    {
        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Title { get; set; } = string.Empty;

        public string SubTitle { get; set; } = string.Empty;
        public List<IFormFile> File { get; set; }
    }
}
