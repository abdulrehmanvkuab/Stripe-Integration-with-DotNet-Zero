namespace Arch.Tenants.Dashboard.Dto
{
    public class SalesSummaryData
    {
        public string Period { get; set; }
        public long Sales { get; set; }
        public long Profit { get; set; }

        public SalesSummaryData(string period, long sales, long profit)
        {
            Period = period;
            Sales = sales;
            Profit = profit;
        }
    }
}