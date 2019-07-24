using System;
using System.Globalization;
using System.Text.RegularExpressions;
using ComboBoxUnoSandbox.Shared.Helpers.Extensions;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public static class DateTimeEx
    {
        public const int SecondsInDay = 60 * 60 * 24;
        static bool currentCultureMonthFirst;
        public static readonly DateTime MinSqlDate = new DateTime(1900, 1, 1); // a bit more sensible than 1753

        static DateTimeEx()
        {
            var shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            var monthIndex = shortDatePattern.IndexOf("M", StringComparison.Ordinal);
            var dayIndex = shortDatePattern.IndexOf("d", StringComparison.Ordinal);

            currentCultureMonthFirst = monthIndex >= 0 && monthIndex < dayIndex;
        }

        public static bool Between(int startDow, int endDow, int dow)
        {
            if (dow == startDow || dow == endDow) return true;

            if (endDow > startDow) return dow >= startDow && dow <= endDow;

            return dow >= endDow || dow <= startDow;
        }

        public static void SetUpForTest(bool currentCultureMonthFirst)
        {
            DateTimeEx.currentCultureMonthFirst = currentCultureMonthFirst;
        }

        public static DateTime Min(DateTime d1, DateTime d2)
        {
            return d1 < d2 ? d1 : d2;
        }

        public static DateTime Max(DateTime d1, DateTime d2)
        {
            return d1 > d2 ? d1 : d2;
        }

        public static DateTime? ParseNullable(string dateTimeString)
        {
            if (String.IsNullOrEmpty(dateTimeString)) return null;

            return DateTime.Parse(dateTimeString);
        }

        static readonly Regex ShortDateTime = new Regex(@"^ *(\d{1,2})/(\d{1,2})( +(\d{1,2})?:?(\d{2})?)? *$");
        static readonly Regex ShortYearDateTimeNoSlashUs = new Regex(@"^ *([01]?\d)([0123]?\d)(\d{2})?( +(\d{1,2})?:?(\d{2})?)? *$", RegexOptions.RightToLeft);  // right to left to favour 2 digits in the day
        static readonly Regex ShortDateTimeNoSlashUs = new Regex(@"^ *([01]?\d)([0123]?\d)( +(\d{1,2})?:?(\d{2})?)? *$", RegexOptions.RightToLeft);
        static readonly Regex ShortYearDateTimeNoSlashEu = new Regex(@"^ *([0123]?\d)([01]?\d)(\d{2})?( +(\d{1,2})?:?(\d{2})?)? *$"); // left to right to favour 2 digits in the day
        static readonly Regex ShortDateTimeNoSlashEu = new Regex(@"^ *([0123]?\d)([01]?\d)( +(\d{1,2})?:?(\d{2})?)? *$");
        static readonly Regex ShortTime = new Regex(@"^ *(\d{1,2})?:?(\d{2})? *$");
        static readonly Regex ShortTimeWithDate = new Regex(@"^[0-9/]+ *(\d{1,2})?:?(\d{2})?(:(\d{2}))? *$");

        public static bool TryParseTime(string input, out TimeSpan retVal)
        {
            retVal = default(TimeSpan);
            if (input == null) return false;
            var match = ShortTime.Match(input);
            if (match.Success)
            {
                var hour = match.Groups[1].Success ? Int32.Parse(match.Groups[1].Value) : 0;
                var min = match.Groups[2].Success ? Int32.Parse(match.Groups[2].Value) : 0;
                try
                {
                    retVal = new TimeSpan(hour, min, 0);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            match = ShortTimeWithDate.Match(input);
            if (match.Success)
            {
                var hour = match.Groups[1].Success ? Int32.Parse(match.Groups[1].Value) : 0;
                var min = match.Groups[2].Success ? Int32.Parse(match.Groups[2].Value) : 0;
                var sec = match.Groups[4].Success ? Int32.Parse(match.Groups[4].Value) : 0;
                try
                {
                    retVal = new TimeSpan(hour, min, sec);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            DateTime parsed;
            if (DateTime.TryParse(input, out parsed))
            {
                retVal = new TimeSpan(parsed.Hour, parsed.Minute, parsed.Second);
                return true;
            }

            retVal = TimeSpan.Zero;
            return false;
        }

        public static bool TryShortParseWithTime(string input, out bool parsed, out DateTime retVal, DateTime? referenceDate = null)
        {
            return TryShortParseWithTime(input, currentCultureMonthFirst, referenceDate, out parsed, out retVal);
        }

        static bool TryShortParseWithTime(string input, bool monthFirst, DateTime? referenceDate, out bool parsed, out DateTime retVal)
        {
            if (monthFirst)
            {
                if (MatchNoSlashUs(input, referenceDate, out parsed, out retVal))
                {
                    return true;
                }
            }
            else
            {
                if (MatchNoSlashEu(input, referenceDate, out parsed, out retVal))
                {
                    return true;
                }
            }
            var match = ShortDateTime.Match(input);
            if (match.Success)
            {
                var month = Int32.Parse(match.Groups[monthFirst ? 1 : 2].Value);
                var day = Int32.Parse(match.Groups[monthFirst ? 2 : 1].Value);
                var hour = match.Groups[4].Success ? Int32.Parse(match.Groups[4].Value) : 0;
                var min = match.Groups[5].Success ? Int32.Parse(match.Groups[5].Value) : 0;
                var closestYear = GetClosestYear(month, referenceDate);
                try
                {
                    retVal = new DateTime(closestYear, month, day, hour, min, 0);
                    parsed = true;
                    return true;
                }
                catch
                {
                    parsed = false;
                    return false;
                }
            }
            return false;
        }

        static bool MatchNoSlashUs(string input, DateTime? referenceDate, out bool parseSuccess, out DateTime dateTime)
        {
            dateTime = DateTime.Now;
            parseSuccess = true;
            var match = ShortDateTimeNoSlashUs.Match(input);
            if (match.Success)
            {
                var month = Int32.Parse(match.Groups[1].Value);
                var day = Int32.Parse(match.Groups[2].Value);
                var hour = match.Groups[4].Success ? Int32.Parse(match.Groups[4].Value) : 0;
                var min = match.Groups[5].Success ? Int32.Parse(match.Groups[5].Value) : 0;
                var closestYear = GetClosestYear(month, referenceDate);
                try
                {
                    dateTime = new DateTime(closestYear, month, day, hour, min, 0);
                    return true;
                }
                catch
                {
                    parseSuccess = false;
                    return false;
                }
            }
            match = ShortYearDateTimeNoSlashUs.Match(input);
            if (match.Success)
            {
                var month = Int32.Parse(match.Groups[1].Value);
                var day = Int32.Parse(match.Groups[2].Value);
                var year = CorrectCentury(Int32.Parse(match.Groups[3].Value));
                var hour = match.Groups[5].Success ? Int32.Parse(match.Groups[5].Value) : 0;
                var min = match.Groups[6].Success ? Int32.Parse(match.Groups[6].Value) : 0;
                try
                {
                    dateTime = new DateTime(year, month, day, hour, min, 0);
                    return true;
                }
                catch
                {
                    parseSuccess = false;
                    return false;
                }
            }
            return false;
        }

        static bool MatchNoSlashEu(string input, DateTime? referenceDate, out bool parseSuccess, out DateTime dateTime)
        {
            dateTime = DateTime.Now;
            parseSuccess = true;
            var match = ShortDateTimeNoSlashEu.Match(input);
            if (match.Success)
            {
                var day = Int32.Parse(match.Groups[1].Value);
                var month = Int32.Parse(match.Groups[2].Value);
                var hour = match.Groups[4].Success ? Int32.Parse(match.Groups[4].Value) : 0;
                var min = match.Groups[5].Success ? Int32.Parse(match.Groups[5].Value) : 0;
                var closestYear = GetClosestYear(month, referenceDate);
                try
                {
                    dateTime = new DateTime(closestYear, month, day, hour, min, 0);
                    return true;
                }
                catch
                {
                    parseSuccess = false;
                    return false;
                }
            }
            match = ShortYearDateTimeNoSlashEu.Match(input);
            if (match.Success)
            {
                var day = Int32.Parse(match.Groups[1].Value);
                var month = Int32.Parse(match.Groups[2].Value);
                var year = CorrectCentury(Int32.Parse(match.Groups[3].Value));
                var hour = match.Groups[5].Success ? Int32.Parse(match.Groups[5].Value) : 0;
                var min = match.Groups[6].Success ? Int32.Parse(match.Groups[6].Value) : 0;
                try
                {
                    dateTime = new DateTime(year, month, day, hour, min, 0);
                    return true;
                }
                catch
                {
                    parseSuccess = false;
                    return false;
                }
            }
            return false;
        }

        static int CorrectCentury(int year)
        {
            if (year >= 1000) return year;
            return year + 2000;
        }

        static int GetClosestYear(int month, DateTime? referenceDate)
        {
            var actualReferenceDate = referenceDate ?? DateTime.Now;
            var previousYearDiff = Math.Abs(month - 12 - actualReferenceDate.Month);
            var thisYearDiff = Math.Abs(month - actualReferenceDate.Month);
            var nextYearDiff = Math.Abs(month + 12 - actualReferenceDate.Month);

            if (previousYearDiff < thisYearDiff && previousYearDiff < nextYearDiff)
            {
                return actualReferenceDate.Year - 1;
            }
            if (thisYearDiff < nextYearDiff)
            {
                return actualReferenceDate.Year;
            }
            return actualReferenceDate.Year + 1;
        }

        public static bool TryParseOcrTime(string rawValue, out DateTime dateVal)
        {
            rawValue = rawValue.Replace("C", "0").Replace(";", "1");

            if (DateTime.TryParseExact(rawValue, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
    out dateVal))
            {
                return true;
            }

            if (DateTime.TryParseExact(rawValue, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
out dateVal))
            {
                return true;
            }

            TimeSpan ts;
            if (TryParseTime(rawValue, out ts))
            {
                dateVal = dateVal.SetTime(ts.Hours, ts.Minutes, ts.Seconds);
                return true;
            }
            return false;
        }

        public static DateTime MoveFowardTo(this DateTime value, TimeSpan timeOFDay)
        {
            var vTime = new TimeSpan(value.Hour, value.Minute, value.Second);
            if (vTime > timeOFDay)
            {
                value = value.AddDays(1);
            }
            return value.SetTime(timeOFDay.Hours, timeOFDay.Minutes, timeOFDay.Seconds);
        }

        public static DateTime MoveBackwardTo(this DateTime value, TimeSpan timeOFDay)
        {
            var vTime = new TimeSpan(value.Hour, value.Minute, value.Second);
            if (vTime < timeOFDay)
            {
                value = value.AddDays(-1);
            }
            return value.SetTime(timeOFDay.Hours, timeOFDay.Minutes, timeOFDay.Seconds);
        }

        public static bool AfterOrEqualTime(this DateTime value, int hour, int minute)
        {
            return value.Hour > hour || (value.Hour == hour && value.Minute >= minute);
        }
        public static bool BeforeOrEqualTime(this DateTime value, int hour, int minute)
        {
            return value.Hour < hour || (value.Hour == hour && value.Minute <= minute);
        }

    }
}
