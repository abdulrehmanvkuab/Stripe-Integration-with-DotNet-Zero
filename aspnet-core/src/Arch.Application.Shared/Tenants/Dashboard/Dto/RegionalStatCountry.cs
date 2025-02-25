﻿using System.Collections.Generic;

namespace Arch.Tenants.Dashboard.Dto
{
    public class RegionalStatCountry
    {
        public string CountryName { get; set; }

        public decimal Sales { get; set; }

        public List<long> Change { get; set; }

        public decimal AveragePrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}