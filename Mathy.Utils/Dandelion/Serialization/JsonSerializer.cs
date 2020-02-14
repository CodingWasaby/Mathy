// Dandelion.Serialization.JsonSerializer
using Mathy.Utils.Dandelion;
using Mathy.Utils.Dandelion.Collections;
using Mathy.Utils.Dandelion.Reflection;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Mathy.Utils.Dandelion.Serialization
{
	public class JsonSerializer : StringSerializer
	{
		public bool PrettifyJson
		{
			get;
			set;
		}

		public override string SerializeToString(object data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SerializeObject(data, stringBuilder, isInArray: false, 0);
			return stringBuilder.ToString();
		}

		private void Write(StringBuilder b, object value, int level)
		{
			if (!PrettifyJson)
			{
				b.Append(value);
				return;
			}
			for (int i = 0; i <= level - 1; i++)
			{
				b.Append('\t');
			}
			b.Append(value);
		}

		private void RemoveExtraEnding(StringBuilder b)
		{
			bool flag = false;
			if ((!PrettifyJson) ? (b.Length >= 1 && b[b.Length - 1] == ',') : (b.Length >= 3 && b[b.Length - 3] == ','))
			{
				b.Remove(b.Length - ((!PrettifyJson) ? 1 : 3), 1);
			}
		}

		private string GetSerializedPrimitiveValue(object data)
		{
			if (data is bool)
			{
				return true.Equals(data) ? "true" : "false";
			}
			if (data is string)
			{
				return $"\"{Extensions.GetEscapedString(data as string)}\"";
			}
			if (data is DateTime)
			{
				return $"\"{DateParser.Serialize((DateTime)data)}\"";
			}
			if (data.GetType().IsPrimitiveType())
			{
				return data.ToString();
			}
			if (data.GetType().IsEnum)
			{
				if (base.SerializeEnumAsInteger)
				{
					return Enum.GetNames(data.GetType()).ToList().IndexOf(data.ToString())
						.ToString();
				}
				return $"\"{data}\"";
			}
			throw Assert.Guard();
		}

		private void SerializeObject(object data, StringBuilder b, bool isInArray, int level)
		{
			if (data is DynamicObj)
			{
				SerializeDictionaryToDictionary(((DynamicObj)data).ToDictionary(), b, isInArray, level);
			}
			else if (data is IDictionary)
			{
				SerializeDictionaryToDictionary((IDictionary)data, b, isInArray, level);
			}
			else if (data is IEnumerable)
			{
				SerializeToArray((IEnumerable)data, b, isInArray, level);
			}
			else
			{
				SerializeObjectToDictionary(data, b, isInArray, level);
			}
		}

		private void SerializeDictionaryToDictionary(IDictionary data, StringBuilder b, bool isInArray, int level)
		{
			if (isInArray)
			{
				Write(b, '{', level);
			}
			else
			{
				b.Append('{');
			}
			if (PrettifyJson)
			{
				b.Append("\r\n");
			}
			foreach (string key in data.Keys)
			{
				object obj = data[key];
				if (obj != null)
				{
					if (obj.GetType().IsPrimitiveType() || obj.GetType().IsEnum)
					{
						Write(b, $"\"{key}\":{GetSerializedPrimitiveValue(obj)}", level + 1);
					}
					else
					{
						Write(b, $"\"{key}\":", level + 1);
						SerializeObject(obj, b, isInArray: false, level + 1);
					}
					b.Append(",");
					if (PrettifyJson)
					{
						b.Append("\r\n");
					}
				}
			}
			RemoveExtraEnding(b);
			Write(b, "}", level);
		}

		private void SerializeObjectToDictionary(object data, StringBuilder b, bool isInArray, int level)
		{
			if (isInArray)
			{
				Write(b, '{', level);
			}
			else
			{
				b.Append('{');
			}
			if (PrettifyJson)
			{
				b.Append("\r\n");
			}
			Field[] fields = EntityRepository.GetEntity(data.GetType()).Fields;
			foreach (Field field in fields)
			{
				object value = field.GetValue(data);
				if (value != null)
				{
					if (value.GetType().IsPrimitiveType() || value.GetType().IsEnum)
					{
						Write(b, $"\"{field.Name}\":{GetSerializedPrimitiveValue(value)}", level + 1);
					}
					else
					{
						Write(b, $"\"{field.Name}\":", level + 1);
						SerializeObject(value, b, isInArray: false, level + 1);
					}
					b.Append(",");
					if (PrettifyJson)
					{
						b.Append("\r\n");
					}
				}
			}
			RemoveExtraEnding(b);
			Write(b, "}", level);
		}

		private void SerializeToArray(IEnumerable data, StringBuilder b, bool isInArray, int level)
		{
			b.Append('[');
			if (PrettifyJson)
			{
				b.Append("\r\n");
			}
			foreach (object datum in data)
			{
				if (datum.GetType().IsPrimitiveType() || datum.GetType().IsEnum)
				{
					Write(b, GetSerializedPrimitiveValue(datum), level + 1);
				}
				else
				{
					SerializeObject(datum, b, isInArray: true, level + 1);
				}
				b.Append(",");
				if (PrettifyJson)
				{
					b.Append("\r\n");
				}
			}
			RemoveExtraEnding(b);
			Write(b, "]", level);
		}
	}
}
