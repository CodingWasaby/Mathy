using System;

namespace Mathy.Utils.Dandelion.Serialization
{
	public class YmdHmsDateParser : DateParser
	{
		protected override DateTime DeserializeOverride()
		{
			int year = ReadInteger();
			Skip('-');
			int month = ReadInteger();
			Skip('-');
			int day = ReadInteger();
			if (base.IsEndOfText)
			{
				return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
			}
			Skip(' ');
			int hour = ReadInteger();
			Skip(':');
			int minute = ReadInteger();
			Skip(':');
			int second = ReadInteger();
			return new DateTime(year, month, day, hour, minute, second, 0, DateTimeKind.Local);
		}

		protected override string SerializeOverride(DateTime dateTime, bool isUtc)
		{
			DateTime dateTime2 = isUtc ? TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local) : dateTime;
			if (dateTime2.Hour == 0 && dateTime2.Minute == 0 && dateTime2.Second == 0)
			{
				return $"{dateTime2.Year}-{dateTime2.Month}-{dateTime2.Day}";
			}
			return $"{dateTime2.Year}-{dateTime2.Month}-{dateTime2.Day} {dateTime2.Hour}:{dateTime2.Minute:d02}:{dateTime2.Second:d02}";
		}
	}
}
