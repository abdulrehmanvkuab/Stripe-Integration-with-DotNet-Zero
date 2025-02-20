using System;
using System.Collections.Generic;
using Arch.Tenants.Dashboard.Dto;

namespace Arch.Tenants.Dashboard
{
    public static class DashboardDataGenerator
    {
        private const string DateFormat = "yyyy-MM-dd";
        private static readonly Random Random;
        public static string[] CountryNames = { "Argentina", "China", "France", "Italy", "Japan", "Netherlands", "Russia", "Spain", "Turkey", "United States" };

        static DashboardDataGenerator()
        {
            Random = new Random();
        }

        public static int GetRandomInt(int min, int max)
        {
            return Random.Next(min, max);
        }

        public static int[] GetRandomArray(int size, int min, int max)
        {
            var array = new int[size];
            for (var i = 0; i < size; i++)
            {
                array[i] = GetRandomInt(min, max);
            }

            return array;
        }

        public static int[] GetRandomPercentageArray(int size)
        {
            if (size == 1)
            {
                return new int[100];
            }

            var array = new int[size];
            var total = 0;
            for (var i = 0; i < size - 1; i++)
            {
                array[i] = GetRandomInt(0, 100 - total);
                total += array[i];
            }

            array[size - 1] = 100 - total;

            return array;
        }

        // public static List<SalesSummaryData> GenerateSalesSummaryData(SalesSummaryDatePeriod inputSalesSummaryDatePeriod, List<Dictionary<string, object>> table)
        public static List<SalesSummaryData> GenerateSalesSummaryData(List<Dictionary<string, object>> table)
        {
            //List<SalesSummaryData> data = null;

            var members = new List<SalesSummaryData>();
            foreach (var row in table)
            {
                var period = new object();
                row.TryGetValue("period", out period);

                var sales = new object();
                row.TryGetValue("Sales", out sales);

                var profit = new object();
                row.TryGetValue("Profit", out profit);



                var member = new SalesSummaryData(period.ToString(), Convert.ToInt64(sales.ToString()), Convert.ToInt64(profit.ToString()));
                members.Add(member);

            }


            return members;


        }




        public static List<MemberActivity> GenerateMemberActivities(List<Dictionary<string, object>> table)
        {


            var members = new List<MemberActivity>();
            foreach (var row in table)
            {
                var name = new object();
                row.TryGetValue("name", out name);

                var Earnings = new object();
                row.TryGetValue("Earnings", out Earnings);

                var Cases = new object();
                row.TryGetValue("Cases", out Cases);

                var rate = new object();
                row.TryGetValue("rate", out rate);

                var closed = new object();
                row.TryGetValue("closed", out closed);

                var member = new MemberActivity(name.ToString(), (Earnings.ToString()), Convert.ToInt64(Cases.ToString()), Convert.ToInt64(closed.ToString()), (rate.ToString()), "");
                members.Add(member);

            }

            //for (int i = members.ToArray().Length; i < 10; i++)
            //{
            //    var member = new MemberActivity("", "",0, 0, "");
            //    members.Add(member);
            //}

            return members;
            //  }
        }

        public static List<RegionalStatCountry> GenerateRegionalStat(List<Dictionary<string, object>> table)
        {


            var members = new List<RegionalStatCountry>();
            foreach (var row in table)
            {
                var Local_Region = new object();
                row.TryGetValue("Local_Region", out Local_Region);

                var averagePrice = new object();
                row.TryGetValue("AveragePrice", out averagePrice);

                var sales = new object();
                row.TryGetValue("Sales", out sales);

                var totalPrice = new object();
                row.TryGetValue("TotalPrice", out totalPrice);

                //var closed = new object();
                //row.TryGetValue("closed", out closed);
                var change = new List<long>();


                //   var member = new RegionalStatCountry(Local_Region.ToString(), Convert.ToInt64(averagePrice.ToString()), Convert.ToInt64(sales.ToString()), Convert.ToInt64(totalPrice.ToString()), change);
                members.Add(new RegionalStatCountry
                {
                    CountryName = Local_Region.ToString(),
                    AveragePrice = Convert.ToDecimal(averagePrice.ToString()),
                    Sales = Convert.ToDecimal(sales.ToString()),
                    TotalPrice = Convert.ToDecimal(totalPrice.ToString()),
                    Change = change
                    //{
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20),
                    //    GetRandomInt(-20, 20)
                    //}
                });

            }

            return members;

        }


    }
}