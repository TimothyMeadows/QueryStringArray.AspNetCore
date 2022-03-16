using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QueryStringArray.AspNetCore.Options;

namespace QueryStringArray.AspNetCore.Helpers
{
    public static class QueryStringArrayHtmlHelper
    {
        public static string QueryStringArray<T>(this HtmlHelper helper, QueryStringArrayOptions options = null, params T[] values)
        {
            options ??= new QueryStringArrayOptions();

            var strings = string.Join(options.Separator, values.Select(value => value?.ToString()));
            return $"{options.OpenTag}{strings}{options.CloseTag}";
        }

        public static string QueryStringArray<T>(this HtmlHelper helper, T[] values, QueryStringArrayOptions options = null)
        {
            options ??= new QueryStringArrayOptions();

            var strings = string.Join(options.Separator, values.Select(value => value?.ToString()));
            return $"{options.OpenTag}{strings}{options.CloseTag}";
        }
    }
}
