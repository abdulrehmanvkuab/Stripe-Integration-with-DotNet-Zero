using System.Collections.Generic;

namespace Arch.Tenants.Dashboard.Dto
{
    public class GetSalesSummaryOutput
    {
        public GetSalesSummaryOutput(List<SalesSummaryData> salesSummary)
        {
            SalesSummary = salesSummary;
        }


        public long TotalSales { get; set; }

        public long Revenue { get; set; }

        public long Expenses { get; set; }

        public long Growth { get; set; }

        public List<SalesSummaryData> SalesSummary { get; set; }

    }
}