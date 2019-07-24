using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    public static class UriBuilderExtensions
    {
        /// <summary>
        /// Sets the specified query parameter key-value pair of the URI.
        /// </summary>
        public static UriBuilder SetQueryParam(this UriBuilder uri, string key, string value)
        {
            var collection = uri.ParseQuery();

            // add (or replace existing) key-value pair
            collection.Add(new KeyValuePair<string, string>(key, value));
            string query = string.Join("&", collection
                          .Select(pair =>
                    pair.Key == null
                    ? WebUtility.UrlEncode(pair.Value)
                    : WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value)));

            uri.Query = query;

            return uri;
        }

        public static UriBuilder AddQueryParams(this UriBuilder uri, string key, IEnumerable<string> value)
        {
            var prefix = "";
            if (!string.IsNullOrEmpty(uri.Query)) prefix = "&";
            var query = string.Join("&", value.Select(v => WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(v)));

            uri.Query = new string(uri.Query.Skip(uri.Query.FirstOrDefault() == '?' ? 1 : 0).ToArray()) + prefix + query;

            return uri;
        }

        /// <summary>
        /// Gets the query string key-value pairs of the URI.
        /// Note that the one of the keys may be null ("?123") and
        /// that one of the keys may be an empty string ("?=123").
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> GetQueryParams(
            this UriBuilder uri)
        {
            return uri.ParseQuery();
        }

        public static UriBuilder RemoveQueryParam(this UriBuilder uri, string key)
        {
            var collection = uri.ParseQuery();
            // add (or replace existing) key-value pair
            var newCollection = collection.Where(kvp => !kvp.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            var query = string.Join("&", collection.Select(pair => pair.Key == null
                                                                              ? pair.Value : pair.Key + "=" + pair.Value));
            uri.Query = query;
            return uri;
        }

        public static string GetQueryParamSingle(this UriBuilder uri, string key)
        {
            return uri.GetQueryParam(key).FirstOrDefault();
        }

        public static IEnumerable<string> GetQueryParam(this UriBuilder uri, string key)
        {
            return uri.ParseQuery()
                .Where(kvp => kvp.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                .Select(kvp => kvp.Value);
        }

#if !SILVERLIGHT
        /// <summary>
        /// Converts the legacy NameValueCollection into a strongly-typed KeyValuePair sequence.
        /// </summary>
        static IEnumerable<KeyValuePair<string, string>> AsKeyValuePairs(this NameValueCollection collection)
        {
            return collection.AllKeys.Select(key => new KeyValuePair<string, string>(key, collection.Get(key)));
        }
#endif
        /// <summary>
        /// Parses the query string of the URI into a NameValueCollection.
        /// </summary>
        static List<KeyValuePair<string, string>> ParseQuery(this UriBuilder uri)
        {
            return ParseQueryString(uri.Query);
        }

        //lifted from reflector
        public static List<KeyValuePair<string, string>> ParseQueryString(string query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if ((query.Length > 0) && (query[0] == '?'))
            {
                query = query.Substring(1);
            }
            return FillFromString(query, true, Encoding.UTF8);
        }


        static List<KeyValuePair<string, string>> FillFromString(string s, bool urlencoded, Encoding encoding)
        {
            var retVal = new List<KeyValuePair<string, string>>();
            var num = (s != null) ? s.Length : 0;
            for (var i = 0; i < num; i++)
            {
                var startIndex = i;
                var num4 = -1;
                while (i < num)
                {
                    var ch = s[i];
                    if (ch == '=')
                    {
                        if (num4 < 0)
                        {
                            num4 = i;
                        }
                    }
                    else if (ch == '&')
                    {
                        break;
                    }
                    i++;
                }
                string str = null;
                string str2 = null;
                if (num4 >= 0)
                {
                    str = s.Substring(startIndex, num4 - startIndex);
                    str2 = s.Substring(num4 + 1, (i - num4) - 1);
                }
                else
                {
                    str2 = s.Substring(startIndex, i - startIndex);
                }
                if (urlencoded)
                {
                    retVal.Add(new KeyValuePair<string, string>(
                        Uri.UnescapeDataString(str),
                        Uri.UnescapeDataString(str2)
                        ));
                }
                else
                {
                    retVal.Add(new KeyValuePair<string, string>(str, str2));
                }
                if ((i == (num - 1)) && (s[i] == '&'))
                {
                    retVal.Add(new KeyValuePair<string, string>(null, string.Empty));  // looks dodgy
                }
            }
            return retVal;
        }
        //#endif
    }
}
