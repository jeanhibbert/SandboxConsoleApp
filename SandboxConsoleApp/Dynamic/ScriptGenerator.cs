using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsoleApp.Dynamic
{
    class ScriptGenerator
    {
        public static void GenerateScript()
        {
            dynamic p = new ExpandoObject();

            p.PersonId = 1;
            p.FirstName = "Miguel";

            var keyValuePairs = ((IDictionary<string, object>)p);

            foreach (var kp in keyValuePairs)
                Console.WriteLine($"{kp.Key} - {kp.Value}");

        }

    }

    public static class DynamicExtensions
    {

        public static dynamic ToExpando(this object o)
        {
            if (o.GetType() == typeof(ExpandoObject)) return o; //shouldn't have to... but just in case
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary
            if (o.GetType() == typeof(NameValueCollection) || o.GetType().IsSubclassOf(typeof(NameValueCollection)))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key])).ToList().ForEach(i => d.Add(i));
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props)
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }
            return result;
        }

        public static IDictionary<string, object> ToDictionary(this object thingy)
        {
            return (IDictionary<string, object>)thingy.ToExpando();
        }
    }
}
