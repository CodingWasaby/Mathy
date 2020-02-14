// Dandelion.Serialization.Extensions
using System;
using System.Text;
namespace Mathy.Utils.Dandelion.Serialization
{
	internal static class Extensions
	{
		public static bool IsPrimitiveType(this Type type)
		{
			if (type.Name.Contains("Nullable"))
			{
				return IsPrimitiveType(type.GetGenericArguments()[0]);
			}
			return type.Equals(typeof(string)) || type.Equals(typeof(int)) || type.Equals(typeof(long)) || type.Equals(typeof(byte)) || type.Equals(typeof(float)) || type.Equals(typeof(double)) || type.Equals(typeof(bool)) || type.Equals(typeof(void)) || type.Equals(typeof(decimal)) || type.Equals(typeof(DateTime)) || type.Equals(typeof(void));
		}

		public static string RepeatsString(string s, int times)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 1; i <= times; i++)
			{
				stringBuilder.Append(s);
			}
			return stringBuilder.ToString();
		}

		public static string GetEscapedString(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i <= s.Length - 1; i++)
			{
				char c = s[i];
				if (c == '"')
				{
					stringBuilder.Append("\\\"");
				}
				else if (c == '\\')
				{
					stringBuilder.Append("\\\\");
				}
				else if (c == '/')
				{
					stringBuilder.Append("\\/");
				}
				else if (c == '\b')
				{
					stringBuilder.Append("\\b");
				}
				else if (c == '\f')
				{
					stringBuilder.Append("\\f");
				}
				else if (c == '\n')
				{
					stringBuilder.Append("\\n");
				}
				else if (c == '\r')
				{
					stringBuilder.Append("\\r");
				}
				else if (c == '\t')
				{
					stringBuilder.Append("\\t");
				}
				else if (c > '\u001e')
				{
					stringBuilder.Append(s[i]);
				}
			}
			return stringBuilder.ToString();
		}

		public static string GetUnescapedString(string s)
		{
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			for (; i <= s.Length - 1; i++)
			{
				char c = s[i];
				if (c == '\\')
				{
					i++;
					stringBuilder.Append(ReadEscapeCharacter(s, i));
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private static char ReadEscapeCharacter(string s, int position)
		{
			char c = s[position];
			switch (c)
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
				default:
					return c;
			}
		}
	}
}