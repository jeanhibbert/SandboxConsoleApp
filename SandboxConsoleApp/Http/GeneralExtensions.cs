using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.Http
{
    public static class GeneralExtensions
    {
        public static string FormatWith(this string stringToFormat, params object[] args)
        {
            return string.Format(stringToFormat, args);
        }

        public static string ToUrl(this string urlBase, string controller, string method, object queryStringArgs = null)
        {
            var url = "{0}/{1}/{2}".FormatWith(urlBase.Trim('/'), controller.Trim('/'), method.Trim('/'));

            if (queryStringArgs != null)
            {
                url += "?{0}".FormatWith(queryStringArgs.ToQueryString().TrimStart('?'));
            }

            return url;
        }

        public static string ToUrl(this string urlBase, string controllerMethod, object queryStringArgs = null)
        {
            var url = "{0}/{1}".FormatWith(urlBase.Trim('/'), controllerMethod.Trim('/'));

            if (queryStringArgs != null)
            {
                url += "?{0}".FormatWith(queryStringArgs.ToQueryString().TrimStart('?'));
            }

            return url;
        }

        public static IEnumerable<KeyValuePair<string, string>> PublicPropertiesToKeyValuePairs(this object source)
        {
            return source.GetType().GetProperties().Select(pi => new KeyValuePair<string, string>(pi.Name, (pi.GetValue(source, null) ?? string.Empty).ToString()));
        }

        public static T ToObject<T>(this IDictionary<string, object> source)
                where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType.GetProperty(item.Key).SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static string ToQueryString<T>(this T instance)
            where T : class
        {
            const string DatetimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

            var querystringParams = instance.GetType().GetProperties()
                .Select(pi =>
                {
                    var objValue = pi.GetValue(instance, null);
                    var value = string.Empty;

                    if (objValue != null)
                    {
                        if (pi.PropertyType == typeof(DateTime) ||
                            pi.PropertyType == typeof(DateTime?))
                        {
                            value = ((DateTime)objValue).ToString(DatetimeFormat);
                        }
                        else
                        {
                            value = objValue.ToString();
                        }
                    }

                    return string.Format("{0}={1}", pi.Name, Uri.EscapeDataString(value));
                })
                .ToArray();

            return string.Join("&", querystringParams);
        }

    }
}
