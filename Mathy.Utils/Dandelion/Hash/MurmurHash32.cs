namespace Mathy.Utils.Dandelion.Hash
{
	internal class MurmurHash32
	{
		private const uint c1 = 3432918353u;

		private const uint c2 = 461845907u;

		private const int r1 = 15;

		private const int r2 = 13;

		private const uint m = 5u;

		private const uint n = 3864292196u;

		public static uint Compute(byte[] source, uint seed)
		{
			uint num = seed;
			int num2 = source.Length >> 2 << 2;
			for (int i = 0; i <= num2 - 4; i += 4)
			{
				uint num3 = (uint)(source[i] | (source[i + 1] << 8) | (source[i + 2] << 16) | (source[i + 3] << 24));
				num3 = (uint)((int)num3 * -862048943);
				num3 = ((num3 << 15) | (num3 >> 17));
				num3 *= 461845907;
				num ^= num3;
				num = (uint)((int)(((num << 13) | (num >> 19)) * 5) + -430675100);
			}
			uint num4 = 0u;
			switch (source.Length & 3)
			{
				case 3:
					num4 = (uint)((int)num4 ^ (source[num2 + 2] << 16));
					num4 = (uint)((int)num4 ^ (source[num2 + 1] << 8));
					num4 ^= source[num2];
					break;
				case 2:
					num4 = (uint)((int)num4 ^ (source[num2 + 1] << 8));
					num4 ^= source[num2];
					break;
				case 1:
					num4 ^= source[num2];
					break;
			}
			num4 = (uint)((int)num4 * -862048943);
			num4 = ((num4 << 15) | (num4 >> 17));
			num4 *= 461845907;
			num ^= num4;
			num = (uint)((int)num ^ source.Length);
			num ^= num >> 16;
			num = (uint)((int)num * -2048144789);
			num ^= num >> 13;
			num = (uint)((int)num * -1028477387);
			return num ^ (num >> 16);
		}
	}
}
