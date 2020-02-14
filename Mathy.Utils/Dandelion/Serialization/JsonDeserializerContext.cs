// Dandelion.Serialization.JsonDeserializerContext
using Mathy.Utils.Dandelion.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Mathy.Utils.Dandelion.Serialization
{
	internal class JsonDeserializerContext
	{
		private int position;

		private string text;

		private StringBuilder builder = new StringBuilder();

		private bool isWeak;

		public object DeserializeString(string s, Type type)
		{
			text = s;
			position = 0;
			builder.Clear();
			isWeak = false;
			object obj = ReadData(type);
			if (obj == null)
			{
				return null;
			}
			return Types.ConvertValue(obj, type);
		}

		public object DeserializeString(string s)
		{
			text = s;
			position = 0;
			builder.Clear();
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

		private void SkipWhitespaces()
		{
			while (position <= text.Length - 1 && IsWhitespace(text[position]))
			{
				position++;
			}
		}

		private string ReadString()
		{
			position++;
			builder.Clear();
			char c;
			for (; (c = text[position]) != '"'; position++)
			{
				int num;
				switch (c)
				{
					case '\\':
						position++;
						builder.Append(ReadEscapeCharacter());
						continue;
					default:
						num = ((c == '\n') ? 1 : 0);
						break;
					case '\r':
						num = 1;
						break;
				}
				if (num == 0)
				{
					builder.Append(c);
				}
			}
			position++;
			return builder.ToString();
		}

		private char ReadEscapeCharacter()
		{
			switch (text[position])
			{
				case '"':
					return '"';
				case '\\':
					return '\\';
				case '/':
					return '/';
				case 'r':
					return '\r';
				case 'n':
					return '\n';
				case 't':
					return '\t';
				case 'u':
					{
						int num = 0;
						for (int i = 1; i <= 4; i++)
						{
							position++;
							num = num * 16 + ToDecimalValue(text[position]);
						}
						return Convert.ToChar(num);
					}
				default:
					throw new Exception($"({position}) invalid escape character {text[position]}");
			}
		}

		private object ReadNumber()
		{
			int num = 0;
			float num2 = 0f;
			int num3 = 1;
			int num4 = 1;
			bool flag = false;
			bool flag2 = false;
			int num5;
			if (text[position] == '-')
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
				char c = text[position];
				int num6;
				switch (c)
				{
					case '.':
						if (!flag)
						{
							flag = true;
							goto IL_01be;
						}
						throw new Exception($"({position} multiple dot in number)");
					default:
						num6 = ((c != 'E') ? 1 : 0);
						break;
					case 'e':
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
					if (text[position + 1] == '+')
					{
						num3 = 1;
						position++;
					}
					else if (text[position + 1] == '-')
					{
						num3 = -1;
						position++;
					}
					else
					{
						num3 = 1;
					}
				}
				else
				{
					if (c < '0' || c > '9')
					{
						break;
					}
					if (!flag)
					{
						num = num * 10 + (c - 48);
					}
					else if (!flag2)
					{
						num4 *= 10;
						num += (c - 48) / num4;
					}
					else
					{
						num2 = num2 * 10f + (float)(c - 48);
					}
				}
				goto IL_01be;
			IL_01be:
				builder.Append(c);
				position++;
			}
			if (num2 != 0f)
			{
				for (int i = 1; (float)i <= num2; i++)
				{
					num = ((num3 != 1) ? (num / 10) : (num * 10));
				}
			}
			int num7 = num * num5;
			if (flag || flag2)
			{
				return num7;
			}
			return num7;
		}

		private object ReadPrimitive()
		{
			char c = text[position];
			int num;
			switch (c)
			{
				case '"':
					return ReadString();
				default:
					num = ((c != '-') ? 1 : 0);
					break;
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					num = 0;
					break;
			}
			if (num == 0)
			{
				return ReadNumber();
			}
			switch (c)
			{
				case 't':
					Skip("true");
					return true;
				case 'f':
					Skip("false");
					return false;
				case 'n':
					Skip("null");
					return null;
				default:
					return null;
			}
		}

		private object ReadData(Type type)
		{
			SkipWhitespaces();
			object result = (text[position] == '[') ? ReadArray((type == null) ? null : (type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0])) : ((text[position] != '{') ? ReadPrimitive() : ReadObject(type));
			SkipWhitespaces();
			return result;
		}

		private object ReadArray(Type elementType)
		{
			IList list = isWeak ? new List<object>() : ((elementType == null) ? null : ((IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))));
			Skip('[');
			SkipWhitespaces();
			while (text[position] != ']')
			{
				object obj = ReadData(elementType);
				list?.Add((elementType == null) ? obj : Types.ConvertValue(obj, elementType));
				if (text[position] == ',')
				{
					position++;
				}
				SkipWhitespaces();
			}
			SkipWhitespaces();
			Skip(']');
			SkipWhitespaces();
			return list;
		}

		private object ReadObject(Type type)
		{
			object obj = isWeak ? new Dictionary<string, object>() : ((type == null) ? null : Activator.CreateInstance(type));
			Entity entity = (type == null) ? null : EntityRepository.GetEntity(type);
			Skip('{');
			SkipWhitespaces();
			while (text[position] != '}')
			{
				string fieldName = ReadString();
				Field field = entity?.Fields.FirstOrDefault((Field i) => i.Name == fieldName);
				SkipWhitespaces();
				Skip(':');
				SkipWhitespaces();
				object value = ReadData(field?.Type);
				SkipWhitespaces();
				if (isWeak)
				{
					(obj as IDictionary).Add(fieldName, value);
				}
				else
				{
					field?.SetValue(obj, value);
				}
				if (text[position] == ',')
				{
					position++;
				}
				SkipWhitespaces();
			}
			SkipWhitespaces();
			Skip('}');
			SkipWhitespaces();
			return obj;
		}

		private static int ToDecimalValue(char c)
		{
			return (c >= '0' && c <= '9') ? (c - 48) : (c - 97 + 10);
		}

		private static bool IsWhitespace(char c)
		{
			return c == ' ' || c <= '\u001e';
		}
	}
}
