using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cherimoya.Lexicons
{
    public class Punctuations
    {
        private List<string> punctuations = new List<string>();

        private List<char> puncChars = new List<char>();

        public Punctuations(string[] punctuations)
        {
            foreach (string punctuation in punctuations)
            {

                this.punctuations.Add(punctuation);

                char c = punctuation[0];
                if (!puncChars.Contains(c))
                {
                    puncChars.Add(c);
                }
            }
        }

        public string Match(string s, int fromPosition)
        {
            char c = s[fromPosition];

            if (!Accepts(c))
            {
                return "";
            }
            else if (!puncChars.Contains(c))
            {
                return s.Substring(fromPosition, 1);
            }


            StringBuilder b = new StringBuilder();
            b.Append(c);

            for (int i = fromPosition + 1; i <= s.Length - 1; i++)
            {
                b.Append(s[i]);
                if (!Accepts(c) || !punctuations.Any(j => j.StartsWith(b.ToString())))
                {
                    b.Remove(b.Length - 1, 1);
                    break;
                }
            }

            return b.ToString();
        }

        private bool Accepts(char c)
        {
            return !(c == ' ' || c == '_' || c >= '0' && c <= '9' || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z');
        }
    }
}