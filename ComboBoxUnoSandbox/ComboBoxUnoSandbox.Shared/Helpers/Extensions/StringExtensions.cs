using System;
using System.Collections.Generic;
using System.Linq;

namespace ComboBoxUnoSandbox.Shared.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIc(this string s, string s2)
        {
            return s.Equals(s2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsIc(this string s, string s2, int length)
        {
            return s.Substring(0, length).Equals(s2.Substring(0, length), StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsIc(this IEnumerable<string> codes, string c)
        {
            return codes.Contains(c, StringComparer.OrdinalIgnoreCase);
        }

        public static List<int> StringToIntList(this string s)
        {
            int tmp;
            return s?.Split(',')
                       .Where(ds => int.TryParse(ds, out tmp))
                       .Select(int.Parse).ToList() ?? new List<int>();
        }
    }
}
