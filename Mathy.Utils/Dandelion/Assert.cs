using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Utils.Dandelion
{
	public class Assert
	{
		public static void Between<T>(T value, T from, T to) where T : IComparable
		{
			if (value.CompareTo(from) < 0 || value.CompareTo(to) > 0)
			{
				throw new Exception($"value should be between {from} and {to}, actually {value}");
			}
		}

		public static Exception Guard()
		{
			return new Exception("Cannot reach here");
		}
	}
}
