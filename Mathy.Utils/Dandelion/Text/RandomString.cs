// Dandelion.Text.RandomString
using System;
using System.Text;

namespace Mathy.Utils.Dandelion.Text
{

    public class RandomString
    {
        private static Random random = new Random();

        public static string GenerateAlphasAndDigits(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i <= length; i++)
            {
                int num = random.Next(62);
                char value = (num < 0 || num > 25) ? ((num < 26 || num > 51) ? ((char)(48 + (num - 52))) : ((char)(65 + (num - 26)))) : ((char)(97 + num));
                stringBuilder.Append(value);
            }
            return stringBuilder.ToString();
        }

        public static string GenerateDigits(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i <= length; i++)
            {
                int num = random.Next(10);
                stringBuilder.Append((char)(48 + num));
            }
            return stringBuilder.ToString();
        }
    }
}
