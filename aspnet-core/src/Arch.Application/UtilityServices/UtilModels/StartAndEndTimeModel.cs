﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.UtilityServices.UtilModels
{
    public class StartAndEndTimeModel
    {
        public StartAndEndTimeModel(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
