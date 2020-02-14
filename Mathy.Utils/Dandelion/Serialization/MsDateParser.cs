
using System;
namespace Mathy.Utils.Dandelion.Serialization
{
	public class MsDateParser : DateParser
	{
		protected override DateTime DeserializeOverride()
		{
			Skip("/Date(");
			long num = ReadLong();
			return TimeZoneInfo.ConvertTime(DateParser.StartTime.AddMilliseconds(num), TimeZoneInfo.Local);
		}

		protected override string SerializeOverride(DateTime dateTime, bool isUtc)
		{
			int num = (int)(Math.Abs(DateParser.TimeZoneOffset.TotalMilliseconds) / 1000.0);
			int num2 = (int)Math.Floor((float)num / 3600f);
			int num3 = (num - num2 * 3600) / 60;
			DateTime dateTime2 = isUtc ? dateTime : TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc);
			return string.Format("/Date({0}{1}{2:d02}{3:d02})/", (long)dateTime2.Subtract(DateParser.StartTime).TotalMilliseconds, (DateParser.TimeZoneOffset.TotalMilliseconds >= 0.0) ? "+" : "-", num2, num3);
		}
	}
}
