// Dandelion.C
using Mathy.Utils.Dandelion.Converting;
using System.IO;
using System.Text;

namespace Mathy.Utils.Dandelion
{
    public static class C
    {
        public static string ToCamel(string s)
        {
            return StringFuncs.ToCamel(s);
        }

        public static string ToLower(string s)
        {
            return StringFuncs.ToLower(s);
        }

        public static string ToMD5(string s)
        {
            return HashFuncs.ToMD5(Encoding.UTF8.GetBytes(s));
        }

        public static string ToPascal(string s)
        {
            return StringFuncs.ToPascal(s);
        }

        public static string ToUpper(string s)
        {
            return StringFuncs.ToUpper(s);
        }

        public static string ToUrlEncoded(string s)
        {
            return UrlEncodeFuncs.Encode(s, Encoding.UTF8);
        }

        public static string Repeat(string s, int times)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i <= times; i++)
            {
                stringBuilder.Append(s);
            }
            return stringBuilder.ToString();
        }

        public static object FormatAsDictionary(object obj)
        {
            return ObjectFormatter.Instance.Convert(obj, ObjectFormatter.Options.Dictionary);
        }

        public static object FormatAsDynamicObj(object obj)
        {
            return ObjectFormatter.Instance.Convert(obj, ObjectFormatter.Options.DynamicObj);
        }

        public static object FormatAsObject(object obj)
        {
            return ObjectFormatter.Instance.Convert(obj, ObjectFormatter.Options.Object);
        }

        public static string ToSHA1(FileInfo file)
        {
            return StringFuncs.ToHexString(HashFuncs.ToSHA1(File.ReadAllBytes(file.FullName)));
        }

        public static string ToSHA1(string s)
        {
            return StringFuncs.ToHexString(HashFuncs.ToSHA1(Encoding.UTF8.GetBytes(s)));
        }

        public static byte[] ToSHA1Bytes(string s)
        {
            return HashFuncs.ToSHA1(Encoding.UTF8.GetBytes(s));
        }

        public static byte[] ToHMacSHA1(string s, string key)
        {
            return HashFuncs.ToHMacSHA1(s, key);
        }
    }
}
