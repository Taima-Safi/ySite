using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Helper
{
    public static class Extensions
    {
        public static T To<T>(this object input)
        {
            if (input == null)
            {
                return default;
            }
            return (T)Convert.ChangeType(input, typeof(T));
        }

        public static string ToSafeString(object input)
        {
            return input?.ToString() ?? string.Empty;
        }

        public static T Deserialize<T>(this string input)
        {
            if (input == null)
            {
                return default;
            }
            return (T)JsonConvert.DeserializeObject<T>(input);
        }

        public static string Serialize(this object input)
        {
            if (input == null)
            {
                return "";
            }
            return JsonConvert.SerializeObject(input);
        }
    }
}
