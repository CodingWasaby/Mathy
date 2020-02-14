// Dandelion.Serialization.KeyValueSerializer
using Mathy.Utils.Dandelion.Reflection;
using Mathy.Utils.Dandelion.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathy.Utils.Dandelion.Serialization
{
	public class KeyValueSerializer : StringSerializer
	{
		public string Separator
		{
			get;
			set;
		}

		public int TabLength
		{
			get;
			set;
		}

		public bool UseTabs
		{
			get;
			set;
		}

		public int CountOfSpacesAfterTabs
		{
			get;
			set;
		}

		public KeyValueSerializer()
		{
			Separator = "=";
			TabLength = 8;
		}

		public override string SerializeToString(object data)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			AddDataToDictionary(data, null, dictionary);
			dictionary = dictionary.ToDictionary((KeyValuePair<string, string> i) => i.Key + Separator, (KeyValuePair<string, string> i) => i.Value);
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			string str = null;
			if (UseTabs)
			{
				num = (int)Math.Ceiling((float)dictionary.Keys.Max((string i) => i.Length) / (float)TabLength);
				str = Extensions.RepeatsString(" ", CountOfSpacesAfterTabs);
			}
			foreach (string key in dictionary.Keys)
			{
				string arg = null;
				if (UseTabs)
				{
					int num2 = num - (int)Math.Ceiling((float)key.Length / (float)TabLength);
					if (key.Length % TabLength != 0)
					{
						num2++;
					}
					arg = Extensions.RepeatsString("\t", num2) + str;
				}
				stringBuilder.AppendFormat("{0}{1}{2}\r\n", key, arg, dictionary[key]);
			}
			return stringBuilder.ToString();
		}

		private string GetValueString(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is string)
			{
				return $"\"{Extensions.GetEscapedString((string)value)}\"";
			}
			if (value is DateTime)
			{
				return $"\"{DateParser.Serialize((DateTime)value)}\"";
			}
			return value.ToString();
		}

		private void AddDataToDictionary(object data, string baseField, Dictionary<string, string> dict)
		{
			if (data is IDictionary)
			{
				AddDictionaryToDictionary(data as IDictionary, baseField, dict);
			}
			else if (data is IEnumerable)
			{
				AddEnumerableToDictionary(data as IEnumerable, baseField, dict);
			}
			else
			{
				AddObjectToDictionary(data, baseField, dict);
			}
		}

		private void AddDictionaryToDictionary(IDictionary data, string baseField, Dictionary<string, string> dict)
		{
			foreach (string key in data.Keys)
			{
				object obj = data[key];
				if (obj != null)
				{
					if (obj.GetType().IsPrimitiveType())
					{
						dict.Add((baseField == null) ? key : (baseField + "." + key), GetValueString(obj));
					}
					else
					{
						AddDataToDictionary(obj, key, dict);
					}
				}
			}
		}

		private void AddEnumerableToDictionary(IEnumerable data, string baseField, Dictionary<string, string> dict)
		{
			int num = 0;
			foreach (object datum in data)
			{
				string text = $"{baseField}[{num}]";
				if (datum.GetType().IsPrimitiveType())
				{
					dict.Add(text, GetValueString(datum));
				}
				else
				{
					AddDataToDictionary(datum, text, dict);
				}
				num++;
			}
		}

		private void AddObjectToDictionary(object data, string baseField, Dictionary<string, string> dict)
		{
			Field[] fields = EntityRepository.GetEntity(data.GetType()).Fields;
			foreach (Field field in fields)
			{
				object value = field.GetValue(data);
				if (value != null)
				{
					if (value.GetType().IsPrimitiveType())
					{
						dict.Add((baseField == null) ? field.Name : (baseField + "." + field.Name), GetValueString(value));
					}
					else
					{
						AddDataToDictionary(value, field.Name, dict);
					}
				}
			}
		}
	}
}
