using System;

namespace DQF.Platform.Extensions
{
    public static class DateTimeExt
    {
        public static int YearMonthHash(this DateTime date)
        {
            return date.Month  + date.Year * 100;
        }

        public static int GetDateHash(this DateTime date)
        {
            return date.Day +  date.Month * 100  + date.Year * 100 * 100;
        } 

        public static string ToShortDateString(this DateTime? date, string emptyValue = "")
        {
            return date.HasValue ? date.Value.ToShortDateString() : emptyValue;
        }
    }
}