﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Arch.Tenants.Dashboard.Dto;

namespace Arch.Tenants.Dashboard
{
    public static class DashboardRandomDataGenerator
    {
        private const string DateFormat = "yyyy-MM-dd";
        private static readonly Random Random;
        public static string[] CountryNames = { "South East", "South West", "North Central", "North East", "North West", "South South", "MidWest", "South Central", "North", "South"};

        static DashboardRandomDataGenerator()
        {
            Random = new Random();
        }

        public static long GetRandomInt(long min, long max)
        {
            return Random.Next((int) min, (int) max);
        }

        public static long[] GetRandomArray(long size, long min, long max)
        {
            var array = new long[size];
            for (var i = 0; i < size; i++)
            {
                array[i] = GetRandomInt(min, max);
            }

            return array;
        }

        public static long[] GetRandomPercentageArray(long size)
        {
            if (size == 1)
            {
                return new long[100];
            }

            var array = new long[size];
            long total = 0;
            for (var i = 0; i < size - 1; i++)
            {
                array[i] = GetRandomInt(0, 100 - total);
                total += array[i];
            }

            array[size - 1] = 100 - total;

            return array;
        }

        public static List<SalesSummaryData> GenerateSalesSummaryData(SalesSummaryDatePeriod inputSalesSummaryDatePeriod)
        {
            List<SalesSummaryData> data = null;


            switch (inputSalesSummaryDatePeriod)
            {
                case SalesSummaryDatePeriod.Daily:
                    data = new List<SalesSummaryData>
                    {
                        new SalesSummaryData(DateTime.Now.AddDays(-5).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddDays(-4).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddDays(-3).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddDays(-2).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddDays(-1).ToString(DateFormat), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                    };

                    break;
                case SalesSummaryDatePeriod.Weekly:
                    var lastYear = DateTime.Now.AddYears(-1).Year;
                    data = new List<SalesSummaryData>
                    {
                        new SalesSummaryData(lastYear + " W4", Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(lastYear + " W3", Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(lastYear + " W2", Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(lastYear + " W1", Random.Next(1000, 2000),
                            Random.Next(100, 999))
                    };

                    break;
                case SalesSummaryDatePeriod.Monthly:
                    data = new List<SalesSummaryData>
                    {
                        new SalesSummaryData(DateTime.Now.AddMonths(-4).ToString("yyyy-MM"), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddMonths(-3).ToString("yyyy-MM"), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddMonths(-2).ToString("yyyy-MM"), Random.Next(1000, 2000),
                            Random.Next(100, 999)),
                        new SalesSummaryData(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"), Random.Next(1000, 2000),
                            Random.Next(100, 999))
                    };

                    break;
            }

            return data;
        }

        public static List<MemberActivity> GenerateMemberActivities()
        {
            return new List<MemberActivity>
            {
                new MemberActivity("Femi", ArchConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%", "015-boy-6.svg"),

                new MemberActivity("Doris", ArchConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%","017-girl-8.svg"),

                new MemberActivity("Margaret", ArchConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%", "004-boy-1.svg"),

                new MemberActivity("Kate", ArchConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%", "039-girl-21.svg")
            };
        }

        public static List<RegionalStatCountry> GenerateRegionalStat()
        {
            var stats = new List<RegionalStatCountry>();
            for (var i = 0; i < 4; i++)
            {
                var countryIndex = GetRandomInt(0, CountryNames.Length);
                stats.Add(new RegionalStatCountry
                {
                    CountryName = CountryNames[countryIndex],
                    AveragePrice = GetRandomInt(10, 100),
                    Sales = GetRandomInt(10000, 100000),
                    TotalPrice = GetRandomInt(10000, 50000),
                    Change = new List<long>
                    {
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20)
                    }
                });
            }

            return stats;
        }
    }
}