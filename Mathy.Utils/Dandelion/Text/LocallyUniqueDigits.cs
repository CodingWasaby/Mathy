
using System;
namespace Mathy.Utils.Dandelion.Text
{

	public class LocallyUniqueDigits
	{
		private static string last;

		private static object locker = new object();

		private string content;

		public static LocallyUniqueDigits create(int length)
		{
			lock (locker)
			{
				string text = null;
				int num = 0;
				while (true)
				{
					bool flag = true;
					DateTime now = DateTime.Now;
					text = $"{now.Year}{now.Month:00}{now.Day:00}{now.Hour:00}{now.Minute:00}{now.Second:00}{now.Millisecond * 1000000:000000000}{RandomString.GenerateDigits(length - 23)}";
					if (last == null || last != text)
					{
						break;
					}
					num++;
					if (num > 200)
					{
						throw new Exception("Algorithm employed by LocallyUniqueDigits is flawed for it cannot produce a locally unique digit sequence in 200 rounds of retry. Currently is:\r\n" + text + "\r\nPlease report this to the Gardeners team, and we will be greatly honored if you may provide us a correct implementation.");
					}
				}
				last = text;
				LocallyUniqueDigits locallyUniqueDigits = new LocallyUniqueDigits();
				locallyUniqueDigits.content = text;
				return locallyUniqueDigits;
			}
		}

		public string toString()
		{
			return content;
		}
	}
}
