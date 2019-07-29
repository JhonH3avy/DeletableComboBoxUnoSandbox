using ComboBoxUnoSandbox.Wasm.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Helpers.Linq
{
    public static class EnumerableExtentions
    {

        /// <summary>
        /// Creates a string from the sequence by concatenating the result
        /// of the specified string selector function for each element.
        /// </summary>
        public static string ToConcatenatedString<T>(this IEnumerable<T> source,
            Func<T, string> stringSelector)
        {
            return source.ToConcatenatedString(stringSelector, String.Empty);
        }

        ///  <summary>
        ///  Creates a string from the sequence by concatenating the result
        ///  of the specified string selector function for each element.
        ///  </summary>
        /// <param name="stringSelector"></param>
        /// <param name="separator">The string which separates each concatenated item.</param>
        /// <param name="source"></param>
        public static string ToConcatenatedString<T>(this IEnumerable<T> source, Func<T, string> stringSelector, string separator)
        {
            var b = new StringBuilder();
            var needsSeparator = false; // don't use for first item

            foreach (var item in source)
            {
                if (needsSeparator)
                    b.Append(separator);

                b.Append(stringSelector(item));
                needsSeparator = true;
            }

            return b.ToString();
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Run<T>(this IEnumerable<T> enumerable, [InstantHandle] Action<T> action)
        {
            var result = new List<T>();
            foreach (var o in enumerable)
            {
                action(o);
                result.Add(o);
            }
            return result;
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Run<T>(this IEnumerable<T> enumerable, Action<int, T> action)
        {
            var result = new List<T>();
            var i = 0;
            foreach (var o in enumerable)
            {
                action(i++, o);
                result.Add(o);
            }
            return result;
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }

        public static IEnumerable EmptyIfNull(this IEnumerable enumerable)
        {
            return enumerable ?? Enumerable.Empty<object>();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static IEnumerable<T> TakeUntilInclusive<T>(this IEnumerable<T> enumerable, Func<T, bool> match)
        {
            foreach (var o in enumerable)
            {
                yield return o;
                if (match(o)) yield break;
            }
        }

        public static IEnumerable<T> TakeUpToInclusive<T>(this IEnumerable<T> enumerable, Func<T, bool> match)
        {
            foreach (var o in enumerable)
            {
                if (match(o))
                {
                    yield return o;
                    yield break;
                }
                yield return o;
            }
        }

        /// <summary>
        /// Has to look at the whole list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="match"></param>
        /// <param name="isException"></param>
        /// <returns></returns>
        public static IEnumerable<T> SkipUntilLastWithIntermediateExceptions<T>(this IEnumerable<T> enumerable, Func<T, bool> match, Func<T, bool> isException)
        {
            var canMatch = true;
            var temp = new List<Tuple<bool, T>>();
            // cant reverse as the match could have side effects
            foreach (var e in enumerable)
            {
                var matched = false;
                if (canMatch)
                {
                    matched = match(e);
                }
                canMatch &= matched || isException(e);
                temp.Add(new Tuple<bool, T>(matched, e));
            }

            // m=true, op1 = false, m2 = true, op2 = false, op3 = false, m3 = false

            return ((IEnumerable<Tuple<bool, T>>)temp).Reverse().TakeWhile(i => !i.Item1).Reverse().Select(i => i.Item2);
        }

        /// <summary>
        /// Warning: see match arg comments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="match">Must not have side effects as applied in reverse</param>
        /// <returns></returns>
        public static IEnumerable<T> SkipUntilLast<T>(this IEnumerable<T> enumerable, Func<T, bool> match)
        {
            return enumerable.Reverse().TakeWhile(i => !match(i)).Reverse();
        }

        public static bool None<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }

        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> match)
        {
            return !enumerable.Any(match);
        }

        public static IEnumerable<T> RemoveRepeats<T>(this IEnumerable<T> sequence)
        {
            return sequence.RemoveRepeats(Comparer<T>.Default);
        }

        public static T OnlyOrDefault<T>(this IEnumerable<T> sequence)
        {
            var e = sequence.GetEnumerator();
            if (e.MoveNext())
            {
                var only = e.Current;
                return e.MoveNext() ? default(T) : only;
            }
            return default(T);
        }

        public static IEnumerable<T> RemoveRepeats<T>(this IEnumerable<T> sequence, IComparer<T> comparer)
        {
            var init = false;
            var current = default(T);

            foreach (var x in sequence)
            {
                if (!init || comparer.Compare(current, x) != 0)
                    yield return x;

                current = x;
                init = true;
            }
        }

        public static IEnumerable<T> JustRepeats<T>(this IEnumerable<T> sequence, IComparer<T> comparer)
        {
            var init = false;
            var current = default(T);

            foreach (var x in sequence)
            {
                if (init && comparer.Compare(current, x) == 0)
                    yield return x;

                current = x;
                init = true;
            }
        }

    }
}
