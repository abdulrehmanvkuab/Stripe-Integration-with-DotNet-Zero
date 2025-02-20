using Arch.UtilityServices.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.UtilityServices
{
    public class TimeUtils
    {
        public TimeUtils()
        {

        }

        public static long ConvertMilliseconds(DateTime? dateTime)
        {
            DateTimeOffset dateTimeOffSet = (DateTimeOffset)dateTime;
            long unixMilliseconds = dateTimeOffSet.ToUnixTimeMilliseconds();
            return unixMilliseconds;
        }

        public static StartAndEndTimeModel GetDayStartEnd(DateTime dateTime)
        {
            dateTime = GetDateTimeUtc(dateTime);

            var todayStartTimeStamp = dateTime.Date;
            var todayEndTimeStamp = todayStartTimeStamp.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
            StartAndEndTimeModel startAndEndTimeModel = new StartAndEndTimeModel(todayStartTimeStamp, todayEndTimeStamp);
            return startAndEndTimeModel;

        }


        public static DateTime Now() { 
        
            return GetDateTimeUtc(DateTime.Now);

        }

        public static StartAndEndTimeModel GetTodayStartEnd()
        {
            return GetDayStartEnd(DateTime.Now.Date);
        }

        public static DateTime GetDateTimeUtc(DateTime dateTime)
        {
            //var timeZoneId = TimeZoneInfo.FindSystemTimeZoneById("UTC");

            var kind = dateTime.Kind.ToString();
            if(kind == "Utc")
            {
                return dateTime;
            }

            var startKind = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

            dateTime = TimeZoneInfo.ConvertTimeToUtc(startKind);

            return dateTime;
        }

    }
}
