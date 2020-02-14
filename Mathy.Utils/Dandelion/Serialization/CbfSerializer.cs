// Dandelion.Serialization.CbfSerializer
using Mathy.Utils.Dandelion.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Mathy.Utils.Dandelion.Serialization
{
	public class CbfSerializer : Serializer
	{
		private static DateTime originTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override byte[] Serialize(object data, Encoding encoding)
		{
			List<byte> list = new List<byte>();
			SerializeObject(data, list, encoding);
			return list.ToArray();
		}

		private void SerializePrimitiveValue(object data, List<byte> b, Encoding encoding)
		{
			if (data is bool)
			{
				b.Add((byte)(true.Equals(data) ? 116 : 102));
			}
			else if (data is string)
			{
				if (b.Count > 0 && b.Last() == 34)
				{
					b[b.Count - 1] = 39;
				}
				else
				{
					b.Add(34);
				}
				SerializeEscapedString(data as string, b, encoding);
				b.Add(34);
			}
			else if (data is DateTime)
			{
				b.Add(100);
				b.AddRange(GetDateBytes((DateTime)data));
			}
			else if ((data.GetType() == typeof(int) || data.GetType() == typeof(long) || data.GetType() == typeof(float) || data.GetType() == typeof(double)) && Convert.ToInt32(data) >= 0 && Convert.ToInt32(data) <= 9)
			{
				b.Add((byte)(65 + Convert.ToByte(data)));
			}
			else if (data.GetType().IsPrimitiveType())
			{
				b.AddRange(encoding.GetBytes(data.ToString()));
				b.Add(47);
			}
			else if (data.GetType().IsEnum)
			{
				if (base.SerializeEnumAsInteger)
				{
					b.AddRange(encoding.GetBytes(Enum.GetNames(data.GetType()).ToList().IndexOf(data.ToString()) + "/"));
				}
				else
				{
					b.AddRange(encoding.GetBytes($"\"{data}\""));
				}
			}
		}

		private byte[] GetDateBytes(DateTime date)
		{
			double totalSeconds = date.ToUniversalTime().Subtract(originTime).TotalSeconds;
			return BitConverter.GetBytes(totalSeconds);
		}

		private void SerializeEscapedString(string s, List<byte> b, Encoding encoding)
		{
			byte[] bytes = encoding.GetBytes(s);
			for (int i = 0; i <= bytes.Length - 1; i++)
			{
				switch (bytes[i])
				{
					case 34:
						b.Add(92);
						b.Add(34);
						break;
					case 92:
						b.Add(92);
						b.Add(92);
						break;
					case 39:
						b.Add(92);
						b.Add(39);
						break;
					default:
						b.Add(bytes[i]);
						break;
				}
			}
		}

		private byte GetNullValueString()
		{
			return 110;
		}

		private void SerializeObject(object data, List<byte> b, Encoding encoding)
		{
			if (data is IDictionary)
			{
				SerializeDictionaryToDictionary((IDictionary)data, b, encoding);
			}
			else if (data is IEnumerable)
			{
				SerializeToArray((IEnumerable)data, b, encoding);
			}
			else
			{
				SerializeObjectToDictionary(data, b, encoding);
			}
		}

		private void SerializeDictionaryToDictionary(IDictionary data, List<byte> b, Encoding encoding)
		{
			b.Add(123);
			foreach (string key in data.Keys)
			{
				object obj = data[key];
				if (obj != null)
				{
					if (obj.GetType().IsPrimitiveType() || obj.GetType().IsEnum)
					{
						SerializePrimitiveValue(obj, b, encoding);
					}
					else
					{
						SerializeObject(obj, b, encoding);
					}
				}
				else
				{
					b.Add(GetNullValueString());
				}
			}
			b.Add(125);
		}

		private void SerializeObjectToDictionary(object data, List<byte> b, Encoding encoding)
		{
			b.Add(123);
			Field[] fields = EntityRepository.GetEntity(data.GetType()).Fields;
			foreach (Field field in fields)
			{
				object value = field.GetValue(data);
				if (value != null)
				{
					if (value.GetType().IsPrimitiveType() || value.GetType().IsEnum)
					{
						SerializePrimitiveValue(value, b, encoding);
					}
					else
					{
						SerializeObject(value, b, encoding);
					}
				}
				else
				{
					b.Add(GetNullValueString());
				}
			}
			b.Add(125);
		}

		private void SerializeToArray(IEnumerable data, List<byte> b, Encoding encoding)
		{
			b.Add(91);
			foreach (object datum in data)
			{
				if (datum.GetType().IsPrimitiveType() || datum.GetType().IsEnum)
				{
					SerializePrimitiveValue(datum, b, encoding);
				}
				else
				{
					SerializeObject(datum, b, encoding);
				}
			}
			if (b.Last() == 91)
			{
				b[b.Count - 1] = 101;
			}
			else
			{
				b.Add(93);
			}
		}
	}
}
