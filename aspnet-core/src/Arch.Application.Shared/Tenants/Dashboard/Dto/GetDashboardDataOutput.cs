using System.Collections.Generic;

namespace Arch.Tenants.Dashboard.Dto
{
    public class GetDashboardDataOutput
    {
        public long TotalProfit { get; set; }

        public long NewFeedbacks { get; set; }

        public long NewOrders { get; set; }

        public long NewUsers { get; set; }

        public List<SalesSummaryData> SalesSummary { get; set; }

        public long TotalSales { get; set; }

        public long Revenue { get; set; }

        public long Expenses { get; set; }

        public long Growth { get; set; }

        public long TransactionPercent { get; set; }


        public long NewVisitPercent { get; set; }

        public long BouncePercent { get; set; }

        public long[] DailySales { get; set; }

        public long[] ProfitShares { get; set; }
        public string[] ProfitLabels { get; set; }
    }
}