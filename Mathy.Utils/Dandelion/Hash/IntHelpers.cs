// Dandelion.Hash.IntHelpers
using System;
namespace Mathy.Utils.Dandelion.Hash
{
	public static class IntHelpers
	{
		public static ulong RotateLeft(this ulong original, int bits)
		{
			return (original << bits) | (original >> 64 - bits);
		}

		public static ulong RotateRight(this ulong original, int bits)
		{
			return (original >> bits) | (original << 64 - bits);
		}

		public static ulong GetUInt64(this byte[] bb, int pos)
		{
			return BitConverter.ToUInt64(bb, pos);
		}
	}
}
