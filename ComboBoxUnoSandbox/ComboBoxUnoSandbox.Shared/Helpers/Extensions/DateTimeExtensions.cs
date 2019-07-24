using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Helpers.Extensions
{
    public static class DateTimeExtensions
    {
        public const int SecondsPerHour = 3600;

        public static DateTime Round(this DateTime d, RoundTo rt)
        {
            var dtRounded = new DateTime();

            switch (rt)
            {
                case RoundTo.Second:
                    dtRounded = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
                    if (d.Millisecond >= 500) dtRounded = dtRounded.AddSeconds(1);
                    break;
                case RoundTo.Minute:
                    dtRounded = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
                    if (d.Second >= 30) dtRounded = dtRounded.AddMinutes(1);
                    break;
                case RoundTo.Hour:
                    dtRounded = new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
                    if (d.Minute >= 30) dtRounded = dtRounded.AddHours(1);
                    break;
                case RoundTo.Day:
                    dtRounded = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
                    if (d.Hour >= 12) dtRounded = dtRounded.AddDays(1);
                    break;
            }

            return dtRounded;
        }

        public enum RoundTo
        {
            Second, Minute, Hour, Day
        }

        public static DateTime StartOfWeek(this DateTime dt)
        {
            throw new NotImplementedException();
            //            var ci = Thread.CurrentThread.CurrentCulture;
            //            var fdow = ci.DateTimeFormat.FirstDayOfWeek;
            //            return Previous(dt, fdow);
        }

        /// <summary>
        /// Returns the very end of the given day i.e. Midnight or 00:00 on the next date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            if (date.Hour == 0 && date.Minute == 0 && date.Second == 0 && date.Millisecond == 0)
            {
                return date.AddDays(1);
            }
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0).AddDays(1);
        }

        /// <summary>
        /// Returns the Start of the given day (the fist millisecond of the given date)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime BeginningOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Subtracts given TimeSpan from current date (DateTime.Now) and returns resulting DateTime in the past
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static DateTime Ago(this TimeSpan from)
        {
            return from.Ago(DateTime.Now);
        }

        /// <summary>
        /// Subtracts given TimeSpan from originalValue DateTime and returns resulting DateTime in the past
        /// </summary>
        /// <param name="from"></param>
        /// <param name="originalValue"></param>
        /// <returns></returns>
        public static DateTime Ago(this TimeSpan from, DateTime originalValue)
        {
            return new DateTime((originalValue - from).Ticks);
        }


        /// <summary>
        /// Adds given TimeSpan to current DateTime (Now) and returns resulting DateTime in the future
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static DateTime FromNow(this TimeSpan from)
        {
            return from.From(DateTime.Now);
        }

        /// <summary>
        /// Adds given TimeSpan to supplied originalValue DateTime and returns resulting DateTime in the future
        /// </summary>
        /// <param name="from"></param>
        /// <param name="originalValue"></param>
        /// <returns></returns>
        public static DateTime From(this TimeSpan from, DateTime originalValue)
        {
            return new DateTime((originalValue + from).Ticks);
        }

        /// <summary>
        /// Adds given TimeSpan to supplied originalValue DateTime and returns resulting DateTime in the future
        /// Synonim of From method
        /// </summary>
        /// <param name="from"></param>
        /// <param name="originalValue"></param>
        /// <returns></returns>
        public static DateTime Since(this TimeSpan from, DateTime originalValue)
        {
            return new DateTime((originalValue + from).Ticks);
        }


        /// <summary>
        /// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the next calendar year. 
        /// If that day does not exist in next year in same month, number of missing days is added to the last day in same month next year.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static DateTime NextYear(this DateTime start)
        {
            var nextYear = start.Year + 1;
            var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, start.Month);

            if (numberOfDaysInSameMonthNextYear < start.Day)
            {
                var differenceInDays = start.Day - numberOfDaysInSameMonthNextYear;
                return
                    new DateTime(nextYear, start.Month, numberOfDaysInSameMonthNextYear, start.Hour, start.Minute,
                                 start.Second, start.Millisecond) + differenceInDays.Days();
            }
            return new DateTime(nextYear, start.Month, start.Day, start.Hour, start.Minute, start.Second, start.Millisecond);
        }

        /// <summary>
        /// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the previous calendar year.
        /// If that day does not exist in previous year in same month, number of missing days is added to the last day in same month previous year.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static DateTime PreviousYear(this DateTime start)
        {
            var previousYear = start.Year - 1;
            var numberOfDaysInSameMonthPreviousYear = DateTime.DaysInMonth(previousYear, start.Month);

            if (numberOfDaysInSameMonthPreviousYear < start.Day)
            {
                var differenceInDays = start.Day - numberOfDaysInSameMonthPreviousYear;
                return
                    new DateTime(previousYear, start.Month, numberOfDaysInSameMonthPreviousYear, start.Hour, start.Minute,
                                 start.Second, start.Millisecond) + differenceInDays.Days();
            }
            return new DateTime(previousYear, start.Month, start.Day, start.Hour, start.Minute, start.Second, start.Millisecond);
        }

        /// <summary>
        /// Returns DateTime increased by 24 hours ie Next Day
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static DateTime NextDay(this DateTime start)
        {
            return start + 1.Days();
        }

        /// <summary>
        /// Returns DateTime decreased by 24h period ie Previous Day
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static DateTime PreviousDay(this DateTime start)
        {
            return start - 1.Days();
        }

        /// <summary>
        /// Returns first next occurence of specified DayOfTheWeek
        /// </summary>
        /// <param name="start"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime Next(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.NextDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }

        /// <summary>
        /// Returns first next occurence of specified DayOfTheWeek
        /// </summary>
        /// <param name="start"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime Previous(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.PreviousDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }


        /// <summary>
        /// Increases supplied DateTime for 7 days ie returns the Next Week
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static DateTime WeekAfter(this DateTime start)
        {
            return start + 1.Weeks();
        }

        /// <summary>
        /// Decreases supplied DateTime for 7 days ie returns the Previous Week
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static DateTime WeekEarlier(this DateTime start)
        {
            return start - 1.Weeks();
        }

        /// <summary>
        /// Generates TimeSpan value for given number of Years ( (number of years * 365) days span )
        /// </summary>
        /// <param name="years"></param>
        /// <returns></returns>
        public static TimeSpan Years(this int years)
        {
            return new TimeSpan(years * 365, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns TimeSpan value for given number of Months (number of months * 30)
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        public static TimeSpan Months(this int months)
        {
            return new TimeSpan(months * 30, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns TimeSpan for given number of Weeks (number of weeks * 7)
        /// </summary>
        /// <param name="weeks"></param>
        /// <returns></returns>
        public static TimeSpan Weeks(this int weeks)
        {
            return new TimeSpan(weeks * 7, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns TimeSpan for given number of Days
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this int days)
        {
            return new TimeSpan(days, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns TimeSpan for given number of Hours
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static TimeSpan Hours(this int hours)
        {
            return new TimeSpan(0, hours, 0, 0, 0);
        }

        /// <summary>
        /// Returns TimeSpan for given number of Minutes
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static TimeSpan Minutes(this int minutes)
        {
            return new TimeSpan(0, 0, minutes, 0, 0);
        }

        /// <summary>
        /// Returns TimeSpan for given number of Seconds
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan Seconds(this int seconds)
        {
            return new TimeSpan(0, 0, 0, seconds, 0);
        }

        /// <summary>
        /// Returns TimeSpan for given number of Milliseconds
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static TimeSpan Milliseconds(this int milliseconds)
        {
            return new TimeSpan(0, 0, 0, 0, milliseconds);
        }

        /// <summary>
        /// Increases the DateTime object with given TimeSpan value
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="toAdd"></param>
        /// <returns></returns>
        public static DateTime IncreaseTime(this DateTime startDate, TimeSpan toAdd)
        {
            return startDate + toAdd;
        }

        /// <summary>
        /// Decreases the DateTime object with given TimeSpan value
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="toSubtract"></param>
        /// <returns></returns>
        public static DateTime DecreaseTime(this DateTime startDate, TimeSpan toSubtract)
        {
            return startDate - toSubtract;
        }

        /// <summary>
        /// Returns the original DateTime with Hour part changed to supplied hour parameter
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime originalDate, int hour)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, originalDate.Minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns the original DateTime with Hour and Minute parts changed to supplied hour and minute parameters
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns the original DateTime with Hour, Minute and Second parts changed to supplied hour, minute and second parameters
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute, int second)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, originalDate.Millisecond);
        }

        public static DateTime SetTime(this DateTime originalDate, DateTime time)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
        }

        /// <summary>
        /// Returns the original DateTime with Hour, Minute, Second and Millisecond parts changed to supplied hour, minute, second and millisecond parameters
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="millisecond"></param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Hour part
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static DateTime SetHour(this DateTime originalDate, int hour)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, originalDate.Minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Minute part
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static DateTime SetMinute(this DateTime originalDate, int minute)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Second part
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static DateTime SetSecond(this DateTime originalDate, int second)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, originalDate.Minute, second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Millisecond part
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="millisecond"></param>
        /// <returns></returns>
        public static DateTime SetMillisecond(this DateTime originalDate, int millisecond)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, originalDate.Minute, originalDate.Second, millisecond);
        }

        /// <summary>
        /// Returns original DateTime value with time part set to midnight (alias for BeginingOfDay method)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime Midnight(this DateTime value)
        {
            return value.BeginningOfDay();
        }

        /// <summary>
        /// Returns original DateTime value with time part set to Noon (12:00:00h)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime Noon(this DateTime value)
        {
            return value.SetTime(12, 0, 0, 0);
        }

        /// <summary>
        /// Returns DateTime with changed Year part
        /// </summary>
        /// <param name="value"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime SetDate(this DateTime value, int year)
        {
            return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Year and Month part
        /// </summary>
        /// <param name="value"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime SetDate(this DateTime value, int year, int month)
        {
            return new DateTime(year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Year, Month and Day part
        /// </summary>
        /// <param name="value"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime SetDate(this DateTime value, int year, int month, int day)
        {
            return new DateTime(year, month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Year part
        /// </summary>
        /// <param name="value"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime SetYear(this DateTime value, int year)
        {
            return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Month part
        /// </summary>
        /// <param name="value"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime SetMonth(this DateTime value, int month)
        {
            return new DateTime(value.Year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns DateTime with changed Day part
        /// </summary>
        /// <param name="value"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime SetDay(this DateTime value, int day)
        {
            return new DateTime(value.Year, value.Month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        public static DateTime Earlier(this DateTime value, DateTime other)
        {
            return value < other ? value : other;
        }
    }
}
