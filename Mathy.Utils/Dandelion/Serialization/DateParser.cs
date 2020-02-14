// Dandelion.Serialization.DateParser
using System;

namespace Mathy.Utils.Dandelion.Serialization
{
	public abstract class DateParser
	{
		private string text;

		private int position;

		private int length;

		public static readonly DateTime StartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static readonly TimeSpan TimeZoneOffset = TimeZoneInfo.Local.BaseUtcOffset;

		private static MsDateParser ms = new MsDateParser();

		private static IsoDateParser iso = new IsoDateParser();

		private static YmdHmsDateParser ymdhms = new YmdHmsDateParser();

		protected char Current => text[position];

		protected bool IsEndOfText => position >= length;

		public DateTime Deserialize(string s)
		{
			text = s;
			position = 0;
			length = s.Length;
			return DeserializeOverride();
		}

		public string Serialize(DateTime dateTime)
		{
			return SerializeOverride(dateTime, dateTime.Kind == DateTimeKind.Utc);
		}

		protected abstract DateTime DeserializeOverride();

		protected abstract string SerializeOverride(DateTime dateTime, bool isUtc);

		protected void Skip(char c)
		{
			position++;
		}

		protected void Skip(string s)
		{
			position += s.Length;
		}

		protected float ReadFloat()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 1;
			bool flag = false;
			while (position <= length - 1)
			{
				char current = Current;
				if ((current < '0' || current > '9') && (flag || current != '.'))
				{
					break;
				}
				if (current == '.')
				{
					flag = true;
				}
				else if (flag)
				{
					num2 = num2 * 10 + (current - 48);
					num3 *= 10;
				}
				else
				{
					num = num * 10 + (current - 48);
				}
				position++;
			}
			return (float)num + (float)num2 / (float)num3;
		}

		protected int ReadInteger()
		{
			int num = 0;
			while (position <= length - 1)
			{
				char current = Current;
				if (current < '0' || current > '9')
				{
					break;
				}
				num = num * 10 + (current - 48);
				position++;
			}
			return num;
		}

		protected long ReadLong()
		{
			long num = 0L;
			while (position <= length - 1)
			{
				char current = Current;
				if (current < '0' || current > '9')
				{
					break;
				}
				num = num * 10 + (current - 48);
				position++;
			}
			return num;
		}

		public static DateParser GetParserForDate(string date)
		{
			if (date[0] == '/')
			{
				return ms;
			}
			if (date.Contains("T"))
			{
				return iso;
			}
			return ymdhms;
		}
	}
}
