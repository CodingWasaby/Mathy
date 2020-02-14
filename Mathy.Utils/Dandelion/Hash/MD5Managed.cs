// Dandelion.Hash.MD5Managed
using Mathy.Utils.Dandelion.Hash;
using System;
namespace Mathy.Utils.Dandelion.Hash
{
	internal class MD5Managed
	{
		public class MD5_CTX
		{
			public readonly uint[] state;

			public readonly uint[] count;

			public readonly byte[] buffer;

			public MD5_CTX()
			{
				state = new uint[4];
				count = new uint[2];
				buffer = new byte[64];
			}

			public void Clear()
			{
				Array.Clear(state, 0, state.Length);
				Array.Clear(count, 0, count.Length);
				Array.Clear(buffer, 0, buffer.Length);
			}
		}

		private const int S11 = 7;

		private const int S12 = 12;

		private const int S13 = 17;

		private const int S14 = 22;

		private const int S21 = 5;

		private const int S22 = 9;

		private const int S23 = 14;

		private const int S24 = 20;

		private const int S31 = 4;

		private const int S32 = 11;

		private const int S33 = 16;

		private const int S34 = 23;

		private const int S41 = 6;

		private const int S42 = 10;

		private const int S43 = 15;

		private const int S44 = 21;

		private readonly MD5_CTX _context = new MD5_CTX();

		private readonly byte[] _digest = new byte[16];

		private bool _hashCoreCalled;

		private bool _hashFinalCalled;

		private static byte[] PADDING;

		public byte[] Hash
		{
			get
			{
				if (!_hashCoreCalled)
				{
					throw new NullReferenceException();
				}
				if (!_hashFinalCalled)
				{
					throw new Exception("Hash must be finalized before the hash value is retrieved.");
				}
				return _digest;
			}
		}

		public int HashSize => _digest.Length * 8;

		public MD5Managed()
		{
			InitializeVariables();
		}

		public void Initialize()
		{
			InitializeVariables();
		}

		private void InitializeVariables()
		{
			MD5Init(_context);
			_hashCoreCalled = false;
			_hashFinalCalled = false;
		}

		public void HashCore(byte[] array, int ibStart, int cbSize)
		{
			if (null == array)
			{
				throw new ArgumentNullException("array");
			}
			if (_hashFinalCalled)
			{
				throw new Exception("Hash not valid for use in specified state.");
			}
			_hashCoreCalled = true;
			MD5Update(_context, array, (uint)ibStart, (uint)cbSize);
		}

		public byte[] HashFinal()
		{
			_hashFinalCalled = true;
			MD5Final(_digest, _context);
			return Hash;
		}

		static MD5Managed()
		{
			PADDING = new byte[64];
			PADDING[0] = 128;
		}

		private static uint F(uint x, uint y, uint z)
		{
			return (x & y) | (~x & z);
		}

		private static uint G(uint x, uint y, uint z)
		{
			return (x & z) | (y & ~z);
		}

		private static uint H(uint x, uint y, uint z)
		{
			return x ^ y ^ z;
		}

		private static uint I(uint x, uint y, uint z)
		{
			return y ^ (x | ~z);
		}

		private static uint ROTATE_LEFT(uint x, int n)
		{
			return (x << n) | (x >> 32 - n);
		}

		private static void FF(ref uint a, uint b, uint c, uint d, uint x, int s, uint ac)
		{
			a += F(b, c, d) + x + ac;
			a = ROTATE_LEFT(a, s);
			a += b;
		}

		private static void GG(ref uint a, uint b, uint c, uint d, uint x, int s, uint ac)
		{
			a += G(b, c, d) + x + ac;
			a = ROTATE_LEFT(a, s);
			a += b;
		}

		private static void HH(ref uint a, uint b, uint c, uint d, uint x, int s, uint ac)
		{
			a += H(b, c, d) + x + ac;
			a = ROTATE_LEFT(a, s);
			a += b;
		}

		private static void II(ref uint a, uint b, uint c, uint d, uint x, int s, uint ac)
		{
			a += I(b, c, d) + x + ac;
			a = ROTATE_LEFT(a, s);
			a += b;
		}

		private static void MD5Init(MD5_CTX context)
		{
			context.count[0] = (context.count[1] = 0u);
			context.state[0] = 1732584193u;
			context.state[1] = 4023233417u;
			context.state[2] = 2562383102u;
			context.state[3] = 271733878u;
		}

		private static void MD5Update(MD5_CTX context, byte[] input, uint inputIndex, uint inputLen)
		{
			uint num = (context.count[0] >> 3) & 0x3F;
			if ((context.count[0] += inputLen << 3) < inputLen << 3)
			{
				context.count[1]++;
			}
			context.count[1] += inputLen >> 29;
			uint num2 = 64 - num;
			uint num3 = 0u;
			if (inputLen >= num2)
			{
				Buffer.BlockCopy(input, (int)inputIndex, context.buffer, (int)num, (int)num2);
				MD5Transform(context.state, context.buffer, 0u);
				for (num3 = num2; num3 + 63 < inputLen; num3 += 64)
				{
					MD5Transform(context.state, input, inputIndex + num3);
				}
				num = 0u;
			}
			Buffer.BlockCopy(input, (int)(inputIndex + num3), context.buffer, (int)num, (int)(inputLen - num3));
		}

		private static void MD5Final(byte[] digest, MD5_CTX context)
		{
			byte[] array = new byte[8];
			Encode(array, context.count, 8u);
			uint num = (context.count[0] >> 3) & 0x3F;
			uint inputLen = (num < 56) ? (56 - num) : (120 - num);
			MD5Update(context, PADDING, 0u, inputLen);
			MD5Update(context, array, 0u, 8u);
			Encode(digest, context.state, 16u);
			context.Clear();
		}

		private static void MD5Transform(uint[] state, byte[] block, uint blockIndex)
		{
			uint a = state[0];
			uint a2 = state[1];
			uint a3 = state[2];
			uint a4 = state[3];
			uint[] array = new uint[16];
			Decode(array, block, blockIndex, 64u);
			FF(ref a, a2, a3, a4, array[0], 7, 3614090360u);
			FF(ref a4, a, a2, a3, array[1], 12, 3905402710u);
			FF(ref a3, a4, a, a2, array[2], 17, 606105819u);
			FF(ref a2, a3, a4, a, array[3], 22, 3250441966u);
			FF(ref a, a2, a3, a4, array[4], 7, 4118548399u);
			FF(ref a4, a, a2, a3, array[5], 12, 1200080426u);
			FF(ref a3, a4, a, a2, array[6], 17, 2821735955u);
			FF(ref a2, a3, a4, a, array[7], 22, 4249261313u);
			FF(ref a, a2, a3, a4, array[8], 7, 1770035416u);
			FF(ref a4, a, a2, a3, array[9], 12, 2336552879u);
			FF(ref a3, a4, a, a2, array[10], 17, 4294925233u);
			FF(ref a2, a3, a4, a, array[11], 22, 2304563134u);
			FF(ref a, a2, a3, a4, array[12], 7, 1804603682u);
			FF(ref a4, a, a2, a3, array[13], 12, 4254626195u);
			FF(ref a3, a4, a, a2, array[14], 17, 2792965006u);
			FF(ref a2, a3, a4, a, array[15], 22, 1236535329u);
			GG(ref a, a2, a3, a4, array[1], 5, 4129170786u);
			GG(ref a4, a, a2, a3, array[6], 9, 3225465664u);
			GG(ref a3, a4, a, a2, array[11], 14, 643717713u);
			GG(ref a2, a3, a4, a, array[0], 20, 3921069994u);
			GG(ref a, a2, a3, a4, array[5], 5, 3593408605u);
			GG(ref a4, a, a2, a3, array[10], 9, 38016083u);
			GG(ref a3, a4, a, a2, array[15], 14, 3634488961u);
			GG(ref a2, a3, a4, a, array[4], 20, 3889429448u);
			GG(ref a, a2, a3, a4, array[9], 5, 568446438u);
			GG(ref a4, a, a2, a3, array[14], 9, 3275163606u);
			GG(ref a3, a4, a, a2, array[3], 14, 4107603335u);
			GG(ref a2, a3, a4, a, array[8], 20, 1163531501u);
			GG(ref a, a2, a3, a4, array[13], 5, 2850285829u);
			GG(ref a4, a, a2, a3, array[2], 9, 4243563512u);
			GG(ref a3, a4, a, a2, array[7], 14, 1735328473u);
			GG(ref a2, a3, a4, a, array[12], 20, 2368359562u);
			HH(ref a, a2, a3, a4, array[5], 4, 4294588738u);
			HH(ref a4, a, a2, a3, array[8], 11, 2272392833u);
			HH(ref a3, a4, a, a2, array[11], 16, 1839030562u);
			HH(ref a2, a3, a4, a, array[14], 23, 4259657740u);
			HH(ref a, a2, a3, a4, array[1], 4, 2763975236u);
			HH(ref a4, a, a2, a3, array[4], 11, 1272893353u);
			HH(ref a3, a4, a, a2, array[7], 16, 4139469664u);
			HH(ref a2, a3, a4, a, array[10], 23, 3200236656u);
			HH(ref a, a2, a3, a4, array[13], 4, 681279174u);
			HH(ref a4, a, a2, a3, array[0], 11, 3936430074u);
			HH(ref a3, a4, a, a2, array[3], 16, 3572445317u);
			HH(ref a2, a3, a4, a, array[6], 23, 76029189u);
			HH(ref a, a2, a3, a4, array[9], 4, 3654602809u);
			HH(ref a4, a, a2, a3, array[12], 11, 3873151461u);
			HH(ref a3, a4, a, a2, array[15], 16, 530742520u);
			HH(ref a2, a3, a4, a, array[2], 23, 3299628645u);
			II(ref a, a2, a3, a4, array[0], 6, 4096336452u);
			II(ref a4, a, a2, a3, array[7], 10, 1126891415u);
			II(ref a3, a4, a, a2, array[14], 15, 2878612391u);
			II(ref a2, a3, a4, a, array[5], 21, 4237533241u);
			II(ref a, a2, a3, a4, array[12], 6, 1700485571u);
			II(ref a4, a, a2, a3, array[3], 10, 2399980690u);
			II(ref a3, a4, a, a2, array[10], 15, 4293915773u);
			II(ref a2, a3, a4, a, array[1], 21, 2240044497u);
			II(ref a, a2, a3, a4, array[8], 6, 1873313359u);
			II(ref a4, a, a2, a3, array[15], 10, 4264355552u);
			II(ref a3, a4, a, a2, array[6], 15, 2734768916u);
			II(ref a2, a3, a4, a, array[13], 21, 1309151649u);
			II(ref a, a2, a3, a4, array[4], 6, 4149444226u);
			II(ref a4, a, a2, a3, array[11], 10, 3174756917u);
			II(ref a3, a4, a, a2, array[2], 15, 718787259u);
			II(ref a2, a3, a4, a, array[9], 21, 3951481745u);
			state[0] += a;
			state[1] += a2;
			state[2] += a3;
			state[3] += a4;
			Array.Clear(array, 0, array.Length);
		}

		private static void Encode(byte[] output, uint[] input, uint len)
		{
			uint num = 0u;
			for (uint num2 = 0u; num2 < len; num2 += 4)
			{
				output[num2] = (byte)(input[num] & 0xFF);
				output[num2 + 1] = (byte)((input[num] >> 8) & 0xFF);
				output[num2 + 2] = (byte)((input[num] >> 16) & 0xFF);
				output[num2 + 3] = (byte)((input[num] >> 24) & 0xFF);
				num++;
			}
		}

		private static void Decode(uint[] output, byte[] input, uint inputIndex, uint len)
		{
			uint num = 0u;
			for (uint num2 = 0u; num2 < len; num2 += 4)
			{
				output[num] = (uint)(input[inputIndex + num2] | (input[inputIndex + num2 + 1] << 8) | (input[inputIndex + num2 + 2] << 16) | (input[inputIndex + num2 + 3] << 24));
				num++;
			}
		}
	}
}
