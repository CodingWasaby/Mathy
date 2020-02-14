// Dandelion.Collections.DynamicObj
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mathy.Utils.Dandelion.Converting;

namespace Mathy.Utils.Dandelion.Collections
{
    public class DynamicObj
    {
        private List<KeyValuePair<string, object>> fields = new List<KeyValuePair<string, object>>();

        public bool AllowDuplicateKeys
        {
            get;
            set;
        }

        public int Count => fields.Count;

        public string[] Keys => fields.Select((KeyValuePair<string, object> i) => i.Key).ToArray();

        public DynamicObj()
        {
        }

        public DynamicObj(object source)
        {
            object obj = ObjectFormatter.Instance.Convert(source, ObjectFormatter.Options.DynamicObj);
            if (!(obj is DynamicObj))
            {
                throw new ArgumentException("Cannot convert to DynamicObj.");
            }
            DynamicObj dynamicObj = obj as DynamicObj;
            fields = dynamicObj.fields.ToList();
            AllowDuplicateKeys = dynamicObj.AllowDuplicateKeys;
        }

        private int FindFieldIndex(string name)
        {
            int num = 0;
            foreach (KeyValuePair<string, object> field in fields)
            {
                if (field.Key == name)
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        public object GetValue(string name)
        {
            foreach (KeyValuePair<string, object> field in fields)
            {
                if (field.Key == name)
                {
                    return field.Value;
                }
            }
            return null;
        }

        public bool ContainsKey(string key)
        {
            return fields.Any((KeyValuePair<string, object> i) => i.Key == key);
        }

        public static DynamicObj From(string key, object value)
        {
            DynamicObj dynamicObj = new DynamicObj();
            return dynamicObj.Put(key, value);
        }

        public DynamicObj Put(string key, object value)
        {
            int num = AllowDuplicateKeys ? (-1) : FindFieldIndex(key);
            if (num >= 0)
            {
                fields[num] = new KeyValuePair<string, object>(key, value);
            }
            else
            {
                fields.Add(new KeyValuePair<string, object>(key, value));
            }
            return this;
        }

        public IList GetList(string key)
        {
            object value = GetValue(key);
            return (value == null) ? null : (value as IList);
        }

        public IDictionary GetDictionary(string key)
        {
            object value = GetValue(key);
            return (value == null) ? null : (value as IDictionary);
        }

        public double GetDouble(string key)
        {
            object value = GetValue(key);
            return (value == null) ? 0.0 : Convert.ToDouble(value);
        }

        public float GetFloat(string key)
        {
            object value = GetValue(key);
            return (value == null) ? 0f : Convert.ToSingle(value);
        }

        public long GetLong(string key)
        {
            object value = GetValue(key);
            return (value == null) ? 0 : Convert.ToInt64(value);
        }

        public int GetInt(string key)
        {
            object value = GetValue(key);
            return (value != null) ? Convert.ToInt32(value) : 0;
        }

        public string GetString(string key)
        {
            object value = GetValue(key);
            return (value == null) ? null : Convert.ToString(value);
        }

        public DateTime GetDate(string key)
        {
            object value = GetValue(key);
            return (value == null) ? DateTime.MinValue : Convert.ToDateTime(value);
        }

        public IDictionary ToDictionary()
        {
            return fields.Select((KeyValuePair<string, object> i) => i.Key).Distinct().ToDictionary((string i) => i, (string i) => fields.First((KeyValuePair<string, object> j) => j.Key == i).Value);
        }
    }
}
