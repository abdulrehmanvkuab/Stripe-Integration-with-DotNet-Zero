﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.Authorization.Users
{
    public class UserDemoGraphicModel
    {
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public long? TeamUserId { get; set; }
        public bool IsMobile { get; set; }
        public long? SupervisorId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }

        public bool AllowSmsFeature { get; set; }

    }
}
