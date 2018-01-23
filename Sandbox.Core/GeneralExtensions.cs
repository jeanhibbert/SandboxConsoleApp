using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sandbox.Core
{
    public static class GeneralExtensions
    {
        public static string ToJson<T>(this T target, bool pretty = true)
        {
            if (target == null)
            {
                return null;
            }

            var format = pretty ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(target, format);
        }

        public static T FromJson<T>(this string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

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

        public static string ReplaceTokens(this string target, object args, params Func<string, string>[] additionalReplacements)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            var value = args.GetType()
                .GetProperties()
                .Aggregate(
                    target,
                    (current, propertyInfo) =>
                    Regex.Replace(
                        current,
                        @"\{{\{{{0}\}}\}}".FormatWith(propertyInfo.Name),
                        (propertyInfo.GetValue(args) == null) ? string.Empty : propertyInfo.GetValue(args).ToString(),
                RegexOptions.IgnoreCase));

            value = additionalReplacements.Aggregate(value, (current, replaceFunc) => Regex.Replace(current, @"\{{\{{(\w*)\}}\}}", m => replaceFunc(m.Value)));
            return value;
        }

        public static T InjectFrom<T, TS>(this T target, TS source)
        {
            var sourceProperties = source.GetType().GetProperties().Where(pr => pr.CanRead).OrderBy(p => p.Name);
            var targetProperties = target.GetType().GetProperties().Where(pr => pr.CanWrite).OrderBy(p => p.Name);
            var targetFields = target.GetType().GetFields().OrderBy(p => p.Name);

            foreach (var sourceInfo in sourceProperties)
            {
                var targetInfo = targetProperties.SingleOrDefault(pi => pi.Name.Equals(sourceInfo.Name) &&
                    pi.PropertyType == sourceInfo.PropertyType);

                if (targetInfo == null)
                {
                    var targetField = targetFields.SingleOrDefault(fi => fi.Name.Equals(sourceInfo.Name) &&
                        fi.FieldType == sourceInfo.PropertyType);

                    if (targetField != null)
                        targetField.SetValue(target, sourceInfo.GetValue(source));
                }
                else
                    targetInfo.SetValue(target, sourceInfo.GetValue(source));
            }

            return target;
        }

    }
}
