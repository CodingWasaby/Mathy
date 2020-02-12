// Dandelion.Converting.UrlEncodeFuncs
using System;
using System.Text;

namespace Mathy.Utils.Dandelion.Converting
{
	internal class UrlEncodeFuncs
	{
		public static string Encode(string s)
		{
			return Encode(s, Encoding.UTF8);
		}

		public static string Encode(string s, Encoding encoding)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i <= s.Length - 1; i++)
			{
				char c = s[i];
				if (!needsEncoding(c))
				{
					stringBuilder.Append(c);
					continue;
				}
				if (c <= 'ÿ')
				{
					AppendEncodedCharacter(c, stringBuilder);
					continue;
				}
				byte[] bytes = encoding.GetBytes(s.Substring(i, 1));
				byte[] array = bytes;
				foreach (byte c2 in array)
				{
					AppendEncodedCharacter(c2, stringBuilder);
				}
			}
			return stringBuilder.ToString();
		}

		private static void AppendEncodedCharacter(int c, StringBuilder b)
		{
			int num = (int)Math.Floor((float)c / 16f);
			int num2 = c - num * 16;
			b.Append('%');
			b.Append((char)((num <= 9) ? (48 + num) : (65 + (num - 10))));
			b.Append((char)((num2 <= 9) ? (48 + num2) : (65 + (num2 - 10))));
		}

		private static bool needsEncoding(char c)
		{
			return c > 'ÿ' || ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z') && (c < '0' || c > '9') && c != '-' && c != '.' && c != '_' && c != '~');
		}
	}
}
