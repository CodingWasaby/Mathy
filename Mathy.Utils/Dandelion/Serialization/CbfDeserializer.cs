// Dandelion.Serialization.CbfDeserializer
using Mathy.Utils.Dandelion.Reflection;
using Mathy.Utils.Dandelion.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Utils.Dandelion.Serialization
{
	public class CbfDeserializer : Deserializer
	{
		private Encoding encoding;

		private int position;

		private byte[] text;

		private bool isWeak;

		private static DateTime originTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override object Deserialize(byte[] data, Encoding encoding, Type type)
		{
			this.encoding = encoding;
			text = data;
			position = 0;
			isWeak = false;
			return ReadData(type);
		}

		public override object Deserialize(byte[] data, Encoding encoding)
		{
			this.encoding = encoding;
			text = data;
			position = 0;
			isWeak = true;
			return ReadData(null);
		}

		private void Skip(char c)
		{
			if (text[position] != c)
			{
				throw new Exception($"({position}) Expect:{c} Found:{text[position]}");
			}
			position++;
		}

		private void Skip(string s)
		{
			for (int i = position; i <= position + s.Length - 1; i++)
			{
				if (text[i] != s[i - position])
				{
					throw new Exception(string.Format("({0}) Expect:{2} Found:{3}", position, s[position - i], text[position]));
				}
			}
			position += s.Length;
		}

		private string ReadString()
		{
			position++;
			List<byte> list = new List<byte>();
			while (true)
			{
				bool flag = true;
				byte b = text[position];
				switch (b)
				{
					case 34:
						position++;
						break;
					case 92:
						position++;
						list.Add(ReadEscapeCharacter());
						goto IL_0087;
					default:
						list.Add(b);
						goto IL_0087;
					case 39:
						break;
				}
				break;
			IL_0087:
				position++;
			}
			return encoding.GetString(list.ToArray());
		}

		private byte ReadEscapeCharacter()
		{
			switch (text[position])
			{
				case 34:
					return 34;
				case 92:
					return 92;
				case 39:
					return 39;
				default:
					throw new Exception($"({position}) invalid escape character {text[position]}");
			}
		}

		private float ReadNumber()
		{
			float num = 0f;
			float num2 = 0f;
			int num3 = 1;
			int num4 = 1;
			bool flag = false;
			bool flag2 = false;
			int num5;
			if (text[position] == 45)
			{
				position++;
				num5 = -1;
			}
			else
			{
				num5 = 1;
			}
			while (true)
			{
				bool flag3 = true;
				byte b = text[position];
				int num6;
				switch (b)
				{
					case 46:
						if (!flag)
						{
							flag = true;
							goto IL_01da;
						}
						throw new Exception($"({position} multiple dot in number)");
					default:
						num6 = ((b != 69) ? 1 : 0);
						break;
					case 101:
						num6 = 0;
						break;
				}
				if (num6 == 0)
				{
					if (flag2)
					{
						throw new Exception($"({position} multiple e/E in number)");
					}
					flag2 = true;
					num3 = ((text[position + 1] == 43) ? 1 : ((text[position + 1] != 45) ? 1 : (-1)));
				}
				else if (b == 43)
				{
					if (flag2)
					{
						throw new Exception($"({position} multiple e/E in number)");
					}
					flag2 = true;
				}
				else
				{
					if (b < 48 || b > 57)
					{
						break;
					}
					if (!flag)
					{
						num = num * 10f + (float)(b - 48);
					}
					else if (!flag2)
					{
						num4 *= 10;
						num += (float)(b - 48) / (float)num4;
					}
					else
					{
						num2 = num2 * 10f + (float)(b - 48);
					}
				}
				goto IL_01da;
			IL_01da:
				position++;
			}
			if (num2 != 0f)
			{
				for (int i = 1; (float)i <= num2; i++)
				{
					num = ((num3 != 1) ? (num / 10f) : (num * 10f));
				}
			}
			position++;
			return num * (float)num5;
		}

		private DateTime ReadDate()
		{
			byte[] array = new byte[8];
			for (int i = 0; i <= 7; i++)
			{
				array[i] = text[position + i];
			}
			position += 8;
			double value = BitConverter.ToDouble(array, 0);
			return originTime.Add(TimeSpan.FromSeconds(value)).ToLocalTime();
		}

		private object ReadPrimitive()
		{
			byte b = text[position];
			if (b == 34 || b == 39)
			{
				return ReadString();
			}
			if (b >= 65 && b <= 90)
			{
				position++;
				return b - 65;
			}
			if ((b >= 48 && b <= 57) || b == 45)
			{
				float num = ReadNumber();
				return ((float)(int)num == num) ? ((float)(int)num) : num;
			}
			switch (b)
			{
				case 100:
					position++;
					return ReadDate();
				case 116:
					position++;
					return true;
				case 102:
					position++;
					return false;
				case 110:
					position++;
					return null;
				default:
					return null;
			}
		}

		private object ReadData(Type type)
		{
			if (text[position] == 91 || text[position] == 101)
			{
				return ReadArray((type == null) ? null : (type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0]));
			}
			if (text[position] == 123)
			{
				return ReadObject(type);
			}
			return ReadPrimitive();
		}

		private object ReadArray(Type elementType)
		{
			IList list = isWeak ? new List<object>() : ((elementType == null) ? null : ((IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))));
			if (text[position] == 101)
			{
				position++;
			}
			else
			{
				Skip('[');
				while (text[position] != 93)
				{
					object value = ReadData(elementType);
					list?.Add(value);
				}
				Skip(']');
			}
			return list;
		}

		private object ReadObject(Type type)
		{
			object obj = isWeak ? new Dictionary<string, object>() : ((type == null) ? null : Activator.CreateInstance(type));
			Entity entity = (type == null) ? null : EntityRepository.GetEntity(type);
			Skip('{');
			int num = 0;
			while (text[position] != 125)
			{
				Field field = (entity != null) ? entity.Fields[num] : null;
				object value = ReadData(field?.Type);
				if (isWeak)
				{
					(obj as IDictionary).Add(num.ToString(), value);
				}
				else
				{
					field?.SetValue(obj, value);
				}
				num++;
			}
			Skip('}');
			return obj;
		}
	}
}
