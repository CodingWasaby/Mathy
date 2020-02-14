// Dandelion.Hash.MurmurHash128
using Mathy.Utils.Dandelion.Hash;
using System;
namespace Mathy.Utils.Dandelion.Hash
{
	public class MurmurHash128
	{
		public static ulong READ_SIZE = 16uL;

		private static ulong C1 = 9782798678568883157uL;

		private static ulong C2 = 5545529020109919103uL;

		private ulong length;

		private uint seed;

		private ulong h1;

		private ulong h2;

		public byte[] Hash
		{
			get
			{
				h1 ^= length;
				h2 ^= length;
				h1 += h2;
				h2 += h1;
				h1 = MixFinal(h1);
				h2 = MixFinal(h2);
				h1 += h2;
				h2 += h1;
				byte[] array = new byte[READ_SIZE];
				Array.Copy(BitConverter.GetBytes(h1), 0, array, 0, 8);
				Array.Copy(BitConverter.GetBytes(h2), 0, array, 8, 8);
				return array;
			}
		}

		private void MixBody(ulong k1, ulong k2)
		{
			h1 ^= MixKey1(k1);
			h1 = h1.RotateLeft(27);
			h1 += h2;
			h1 = h1 * 5 + 1390208809;
			h2 ^= MixKey2(k2);
			h2 = h2.RotateLeft(31);
			h2 += h1;
			h2 = h2 * 5 + 944331445;
		}

		private static ulong MixKey1(ulong k1)
		{
			k1 *= C1;
			k1 = k1.RotateLeft(31);
			k1 *= C2;
			return k1;
		}

		private static ulong MixKey2(ulong k2)
		{
			k2 *= C2;
			k2 = k2.RotateLeft(33);
			k2 *= C1;
			return k2;
		}

		private static ulong MixFinal(ulong k)
		{
			k ^= k >> 33;
			k = (ulong)((long)k * -49064778989728563L);
			k ^= k >> 33;
			k = (ulong)((long)k * -4265267296055464877L);
			k ^= k >> 33;
			return k;
		}

		public byte[] ComputeHash(byte[] bb)
		{
			ProcessBytes(bb);
			return Hash;
		}

		private void ProcessBytes(byte[] bb)
		{
			h1 = seed;
			length = 0uL;
			int num = 0;
			ulong num2 = (ulong)bb.Length;
			while (num2 >= READ_SIZE)
			{
				ulong uInt = bb.GetUInt64(num);
				num += 8;
				ulong uInt2 = bb.GetUInt64(num);
				num += 8;
				length += READ_SIZE;
				num2 -= READ_SIZE;
				MixBody(uInt, uInt2);
			}
			if (num2 != 0)
			{
				ProcessBytesRemaining(bb, num2, num);
			}
		}

		private void ProcessBytesRemaining(byte[] bb, ulong remaining, int pos)
		{
			ulong num = 0uL;
			ulong num2 = 0uL;
			length += remaining;
			if ((long)remaining <= 15L && (long)remaining >= 1L)
			{
				switch (remaining - 1)
				{
					case 14uL:
						num2 ^= (ulong)bb[pos + 14] << 48;
						goto case 13uL;
					case 13uL:
						num2 ^= (ulong)bb[pos + 13] << 40;
						goto case 12uL;
					case 12uL:
						num2 ^= (ulong)bb[pos + 12] << 32;
						goto case 11uL;
					case 11uL:
						num2 ^= (ulong)bb[pos + 11] << 24;
						goto case 10uL;
					case 10uL:
						num2 ^= (ulong)bb[pos + 10] << 16;
						goto case 9uL;
					case 9uL:
						num2 ^= (ulong)bb[pos + 9] << 8;
						goto case 8uL;
					case 8uL:
						num2 ^= bb[pos + 8];
						goto case 7uL;
					case 7uL:
						num ^= bb.GetUInt64(pos);
						goto IL_0147;
					case 6uL:
						num ^= (ulong)bb[pos + 6] << 48;
						goto case 5uL;
					case 5uL:
						num ^= (ulong)bb[pos + 5] << 40;
						goto case 4uL;
					case 4uL:
						num ^= (ulong)bb[pos + 4] << 32;
						goto case 3uL;
					case 3uL:
						num ^= (ulong)bb[pos + 3] << 24;
						goto case 2uL;
					case 2uL:
						num ^= (ulong)bb[pos + 2] << 16;
						goto case 1uL;
					case 1uL:
						num ^= (ulong)bb[pos + 1] << 8;
						goto case 0uL;
					case 0uL:
						{
							num ^= bb[pos];
							goto IL_0147;
						}
					IL_0147:
						h1 ^= MixKey1(num);
						h2 ^= MixKey2(num2);
						return;
				}
			}
			throw new Exception("Something went wrong with remaining bytes calculation.");
		}

		public void GetHash(out ulong? d1, out ulong? d2)
		{
			h2 ^= length;
			h1 += h2;
			h2 += h1;
			h1 = MixFinal(h1);
			h2 = MixFinal(h2);
			h1 += h2;
			h2 += h1;
			d1 = h1;
			d2 = h2;
		}
	}
}
