// Dandelion.Collections.SymbolDictionary
using System.Collections.Generic;

namespace Mathy.Utils.Dandelion.Collections
{
    internal class SymbolDictionary
    {
        private Dictionary<string, object> items = new Dictionary<string, object>();

        public void Put(string key, object value, string scope)
        {
            items.Add(GetFullKey(key, scope), value);
        }

        public object Get(string key, string scope)
        {
            string fullKey = GetFullKey(key, scope);
            return items.ContainsKey(fullKey) ? items[fullKey] : null;
        }

        private static string GetFullKey(string key, string scope)
        {
            return (scope == null) ? key : (scope + "," + key);
        }
    }
}
