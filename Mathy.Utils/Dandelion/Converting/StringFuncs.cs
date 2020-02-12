// Dandelion.Converting.StringFuncs
using Mathy.Utils.Dandelion;
using Mathy.Utils.Dandelion.Serialization;
using System;
using System.Text;

namespace Mathy.Utils.Dandelion.Converting
{
	internal class StringFuncs
	{
		public static string ToString(object obj)
		{
			if (obj == null)
			{
				return "null";
			}
			if (Types.IsPrimitiveType(obj.GetType()) || obj is string || obj is DateTime)
			{
				return obj.ToString();
			}
			JsonSerializer jsonSerializer = new JsonSerializer();
			jsonSerializer.PrettifyJson = true;
			JsonSerializer jsonSerializer2 = jsonSerializer;
			return jsonSerializer2.SerializeToString(obj);
		}

		public static string ToCamel(string s)
		{
			if (s == null || s.Length == 0)
			{
				return s;
			}
			return ToLower(s.Substring(0, 1)) + s.Substring(1);
		}

		public static string ToPascal(string s)
		{
			if (s == null || s.Length == 0)
			{
				return s;
			}
			return ToUpper(s.Substring(0, 1)) + s.Substring(1);
		}

		public static string ToHexString(byte[] instance)
		{
			char[] array = new char[instance.Length << 1];
			for (int i = 0; i <= instance.Length - 1; i++)
			{
				int num = instance[i] >> 4;
				int num2 = instance[i] & 0xF;
				array[i << 1] = (char)((num <= 9) ? (48 + num) : (97 + (num - 10)));
				array[(i << 1) + 1] = (char)((num2 <= 9) ? (48 + num2) : (97 + (num2 - 10)));
			}
			return new string(array);
		}

		public static string ToLower(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i <= s.Length - 1; i++)
			{
				char c = s[i];
				stringBuilder.Append((c >= 'A' && c <= 'Z') ? ((char)(c + 32)) : c);
			}
			return stringBuilder.ToString();
		}

		public static string ToUpper(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i <= s.Length - 1; i++)
			{
				char c = s[i];
				stringBuilder.Append((c >= 'a' && c <= 'z') ? ((char)(c - 32)) : c);
			}
			return stringBuilder.ToString();
		}
	}
}
