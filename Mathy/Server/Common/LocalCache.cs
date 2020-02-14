using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mathy.Server.Common
{
    public class LocalCache
    {
        private static Dictionary<string, object> _Cache = new Dictionary<string, object>();

        public static void Set(string key, object value)
        {
            _Cache.Remove(key);
            _Cache.Add(key, value);
        }
        public static T Get<T>(string key)
        {
            if (_Cache.TryGetValue(key, out object obj))
            {
                return (T)obj;
            }
            return default;
        }
    }
}
