using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public class FormatStrings
    {

        static FormatStrings()
        {
            ShortDate = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            ShortDateTime = ShortDate + " HH:mm";
        }

        public const string ReportDateParam = "yyyy-MM-dd";
        public static string ShortDate;
        public const string DayAndMonth = "d MMM";
        public const string DayAndMonthAndYear = "dd MMM yy";
        public static string ShortDateTime;
        public static string Short24Time = "HH:mm";
    }
}
