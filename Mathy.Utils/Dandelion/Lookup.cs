// Dandelion.Lookup
using Mathy.Utils.Dandelion.Collections;
using System;

namespace Mathy.Utils.Dandelion
{
    public class Lookup
    {
        private static SymbolDictionary dictionary = new SymbolDictionary();

        public static void Put(string key, object value, string scope)
        {
            dictionary.Put(key, value, scope);
        }

        public static T Get<T>(string key, string scope)
        {
            return (T)dictionary.Get(key, scope);
        }

        public static void Put(string key, object value)
        {
            dictionary.Put(key, value, null);
        }

        public static void Put(string key, Enum enumValue)
        {
            Put(key, enumValue, enumValue.GetType().Name);
        }

        public static void Put(Enum key, object value)
        {
            Put(key.ToString(), value, key.GetType().Name);
        }

        public static object Get(string key)
        {
            return Get<object>(key);
        }

        public static object Get(Enum key)
        {
            return Get<object>(key);
        }

        public static T Get<T>(string key)
        {
            return (T)dictionary.Get(key, null);
        }

        public static T Get<T>(string key, Type enumType)
        {
            return (T)dictionary.Get(key, enumType.Name);
        }

        public static T Get<T>(Enum key)
        {
            return (T)dictionary.Get(key.ToString(), key.GetType().Name);
        }
    }
}
