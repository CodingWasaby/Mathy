// Dandelion.StringExtensions

namespace Mathy.Utils.Dandelion
{
    public static class StringExtensions
    {
        public static string TrimBeginning(this string instance, string prefix)
        {
            return instance.StartsWith(prefix) ? instance.Substring(prefix.Length) : instance;
        }

        public static string TrimEnd(this string instance, string suffix)
        {
            return instance.EndsWith(suffix) ? instance.Substring(0, instance.Length - suffix.Length) : suffix;
        }
    }
}
