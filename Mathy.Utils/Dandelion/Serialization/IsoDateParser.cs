
using System;

namespace Mathy.Utils.Dandelion.Serialization
{
	public class IsoDateParser : DateParser
	{
		protected override DateTime DeserializeOverride()
		{
			int year = ReadInteger();
			Skip('-');
			int month = ReadInteger();
			Skip('-');
			int day = ReadInteger();
			Skip('T');
			int hour = ReadInteger();
			Skip(':');
			int minute = ReadInteger();
			Skip(':');
			float num = ReadFloat();
			int num2 = 0;
			if (base.Current != 'Z')
			{
				int num3 = (base.Current == '+') ? 1 : (-1);
				Skip(base.Current);
				int num4 = ReadInteger();
				Skip(':');
				int num5 = ReadInteger();
				num2 = num3 * 1000 * (num4 * 3600 + num5 * 60);
			}
			DateTime dateTime = new DateTime(year, month, day, hour, minute, (int)Math.Floor(num), (int)(((double)num - Math.Floor(num)) * 1000.0), DateTimeKind.Utc).Subtract(TimeSpan.FromMilliseconds(num2));
			return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local);
		}

		protected override string SerializeOverride(DateTime dateTime, bool isUtc)
		{
			int num = (int)(Math.Abs(DateParser.TimeZoneOffset.TotalMilliseconds) / 1000.0);
			int num2 = (int)Math.Floor((float)num / 3600f);
			int num3 = (num - num2 * 3600) / 60;
			DateTime dateTime2 = isUtc ? TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local) : dateTime;
			return string.Format("{0}-{1:d02}-{2:d02}T{3:d02}:{4:d02}:{5:d02}.{6:d03}{7}{8:d02}:{9:d02}", dateTime2.Year, dateTime2.Month, dateTime2.Day, dateTime2.Hour, dateTime2.Minute, dateTime2.Second, dateTime2.Millisecond, (DateParser.TimeZoneOffset.TotalMilliseconds >= 0.0) ? "+" : "-", num2, num3);
		}
	}
}
