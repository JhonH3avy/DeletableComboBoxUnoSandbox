using System;
using System.Collections.Generic;
using System.Text;
using ComboBoxUnoSandbox.Shared.Helpers;

namespace ComboBoxUnoSandbox.Shared.Models
{
    public interface IValueFormatter
    {
        string Format(object val);
    }

    public class DateValueFormatter : IValueFormatter
    {
        public string Format(object val)
        {
            if (!(val is DateTime)) return null;

            var dt = (DateTime)val;
            return dt.ToString(FormatStrings.ReportDateParam);
        }
    }

    public class DefaultValueFormatter : IValueFormatter
    {
        public string Format(object val)
        {
            return val == null ? null : val.ToString();
        }
    }
}
