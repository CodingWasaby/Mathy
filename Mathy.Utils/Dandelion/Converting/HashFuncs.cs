// Dandelion.Converting.HashFuncs
using Mathy.Utils.Dandelion.Converting;
using Mathy.Utils.Dandelion.Hash;
using System.Security.Cryptography;
using System.Text;

namespace Mathy.Utils.Dandelion.Converting
{
	internal class HashFuncs
	{
		public static byte[] ToSHA1(byte[] bytes)
		{
			using (SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider())
			{
				return sHA1CryptoServiceProvider.ComputeHash(bytes);
			}
		}

		public static string ToMD5(byte[] bytes)
		{
			MD5Managed mD5Managed = new MD5Managed();
			mD5Managed.Initialize();
			mD5Managed.HashCore(bytes, 0, bytes.Length);
			return StringFuncs.ToHexString(mD5Managed.HashFinal());
		}

		public static byte[] ToHMacSHA1(string baseString, string keyString)
		{
			HMACSHA1 hMACSHA = new HMACSHA1(Encoding.UTF8.GetBytes(keyString));
			return hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(baseString));
		}
	}
}
