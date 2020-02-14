using System.Linq;

namespace Mathy.Libs
{
    public class StringFuncs
    {
        public static int len(string s)
        {
            return s.Length;
        }

        public static string reverse(string s)
        {
            return new string(s.Reverse().ToArray());
        }
    }
}
