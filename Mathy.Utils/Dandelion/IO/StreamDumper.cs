// Dandelion.IO.StreamDumper
using System.IO;
namespace Mathy.Utils.Dandelion.IO
{
	public class StreamDumper
	{
		public static void DumpTo(Stream from, Stream to)
		{
			DumpTo(from, to, 1024);
		}

		public static void DumpTo(Stream from, Stream to, int blockSize)
		{
			byte[] buffer = new byte[blockSize];
			while (true)
			{
				bool flag = true;
				int num = from.Read(buffer, 0, blockSize);
				if (num == 0)
				{
					break;
				}
				to.Write(buffer, 0, num);
			}
		}
	}
}
