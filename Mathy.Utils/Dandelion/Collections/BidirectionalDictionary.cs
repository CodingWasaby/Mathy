// Dandelion.Collections.BidirectionalDictionary<K,V>
using System.Collections.Generic;


namespace Mathy.Utils.Dandelion.Collections
{
    internal class BidirectionalDictionary<K, V>
    {
        private Dictionary<K, V> keyValueMappings = new Dictionary<K, V>();

        private Dictionary<V, K> valueKeyMppings = new Dictionary<V, K>();

        public void Add(K key, V value)
        {
            if (keyValueMappings.ContainsKey(key))
            {
                keyValueMappings[key] = value;
                valueKeyMppings[value] = key;
            }
            else
            {
                keyValueMappings.Add(key, value);
                valueKeyMppings.Add(value, key);
            }
        }

        public void Remove(K key)
        {
            if (keyValueMappings.ContainsKey(key))
            {
                V key2 = keyValueMappings[key];
                keyValueMappings.Remove(key);
                valueKeyMppings.Remove(key2);
            }
        }

        public V GetValue(K key)
        {
            return keyValueMappings.ContainsKey(key) ? keyValueMappings[key] : default(V);
        }

        public K GetKey(V value)
        {
            return valueKeyMppings.ContainsKey(value) ? valueKeyMppings[value] : default(K);
        }
    }
}
