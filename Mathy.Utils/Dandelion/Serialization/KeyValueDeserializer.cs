using Mathy.Utils.Dandelion.Reflection;
using Mathy.Utils.Dandelion.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Utils.Dandelion.Serialization
{
	public class KeyValueDeserializer : StringDeserializer
	{
		public string Separator
		{
			get;
			set;
		}

		public KeyValueDeserializer()
		{
			Separator = "=";
		}

		public override object DeserializeString(string s, Type type)
		{
			return Deserialize(s, type);
		}

		public override object DeserializeString(string s)
		{
			return Deserialize(s, null);
		}

		private object Deserialize(string s, Type type)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			int num = 0;
			bool isArray = true;
			while (true)
			{
				bool flag = true;
				int num2 = s.IndexOf("\r\n", num);
				if (num2 == -1)
				{
					break;
				}
				string text = s.Substring(num, num2 - num);
				num = num2 + 2;
				int num3 = text.IndexOf(Separator);
				string text2 = text.Substring(0, num3).Trim();
				string value = text.Substring(num3 + Separator.Length).Trim();
				if (text2[0] != '[')
				{
					isArray = false;
				}
				dictionary.Add(text2, value);
			}
			object data = CreateObject(type, isArray);
			foreach (string key in dictionary.Keys)
			{
				DeserializePath(ref data, type, key, dictionary[key]);
			}
			return data;
		}

		private object CreateObject(Type type, bool isArray)
		{
			if (isArray)
			{
				if (type == null)
				{
					return new List<Dictionary<string, object>>();
				}
				return Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
			}
			if (type == null)
			{
				return new Dictionary<string, object>();
			}
			return Activator.CreateInstance(type);
		}

		private object EnsureCount(object data, int count, Type type)
		{
			if (data is IList)
			{
				while ((data as IList).Count < count)
				{
					(data as IList).Add(CreateObject(type, isArray: false));
				}
				return data;
			}
			Array array = data as Array;
			if (array.GetLength(0) <= count)
			{
				return array;
			}
			Array array2 = Activator.CreateInstance(array.GetType().GetElementType().MakeArrayType(1), count) as Array;
			Array.Copy(array, array2, array.Length);
			return array2;
		}

		private object GetValue(string s)
		{
			if (s.Length == 0)
			{
				return null;
			}
			if (s[0] == '"')
			{
				return Extensions.GetUnescapedString(s.Substring(1, s.Length - 2));
			}
			if (s == "True" || s == "true")
			{
				return true;
			}
			if (s == "False" || s == "false")
			{
				return false;
			}
			return double.Parse(s);
		}

		private void DeserializePath(ref object data, Type type, string keyPath, string value)
		{
			int num = keyPath.IndexOf(".");
			string key = (num == -1) ? keyPath : keyPath.Substring(0, num);
			if (key[0] == '[')
			{
				int num2 = key.IndexOf("]");
				int num3 = int.Parse(key.Substring(1, num2 - 1));
				data = EnsureCount(data, num3 + 1, type);
				object obj = null;
				if (data is IList)
				{
					obj = (data as IList)[num3];
				}
				else
				{
					obj = (data as Array).GetValue(num3);
				}
				if (num2 == keyPath.Length - 1)
				{
					(data as IList)[num3] = GetValue(value);
					return;
				}
				object data2 = (data as IList)[num3];
				DeserializePath(ref data2, type, keyPath.Substring(num2 + 2), value);
				return;
			}
			int num4 = -1;
			bool flag = false;
			for (int j = 0; j <= keyPath.Length - 1; j++)
			{
				char c = keyPath[j];
				if (c == '.' || c == '[')
				{
					flag = (c == '[');
					num4 = j;
					break;
				}
			}
			Entity entity = (type == null) ? null : EntityRepository.GetEntity(type);
			if (num4 == -1)
			{
				if (data is IDictionary)
				{
					(data as IDictionary).Add(key, GetValue(value));
				}
				else if (entity.Fields.Any((Field i) => i.Name == key))
				{
					entity.Fields.First((Field i) => i.Name == key).SetValue(data, value);
				}
				return;
			}
			string fieldName = key.Substring(0, num4);
			string keyPath2 = (!flag) ? keyPath.Substring(num4 + 1) : keyPath.Substring(num4);
			if (data is IDictionary)
			{
				if (!(data as IDictionary).Contains(fieldName))
				{
					(data as IDictionary).Add(fieldName, CreateObject(null, flag));
				}
				object data2 = (data as IDictionary)[fieldName];
				DeserializePath(ref data2, null, keyPath2, value);
			}
			else if (entity.Fields.Any((Field i) => i.Name == fieldName))
			{
				Field field = entity.Fields.First((Field i) => i.Name == fieldName);
				object obj2 = Types.ConvertValue(CreateObject(type, flag), field.Type);
				if (field.GetValue(data) == null)
				{
					field.SetValue(data, obj2);
				}
				object data2 = field.GetValue(data);
				DeserializePath(ref data2, (obj2 is Array) ? obj2.GetType().GetElementType() : obj2.GetType().GetGenericArguments()[0], keyPath2, value);
			}
		}
	}
}
