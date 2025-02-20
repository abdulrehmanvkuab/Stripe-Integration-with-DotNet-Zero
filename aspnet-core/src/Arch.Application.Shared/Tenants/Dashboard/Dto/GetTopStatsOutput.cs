using System;
using System.Collections.Generic;
using System.Text;

namespace Arch.Tenants.Dashboard.Dto
{
    public class GetTopStatsOutput
    {
        public long TotalProfit { get; set; }

        public long NewFeedbacks { get; set; }

        public long NewOrders { get; set; }

        public long NewUsers { get; set; }
    }
}
