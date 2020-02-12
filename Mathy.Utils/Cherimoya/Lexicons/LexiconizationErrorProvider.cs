using System;
using System.Text;

namespace Cherimoya.Lexicons
{
    public class LexiconizationErrorProvider
    {

        public LexiconizationErrorProvider(string text)
        {
            this.text = text;
        }


        private string text;


        public void ThrowException(string message, int pos)
        {
            int pos1 = Math.Max(pos - 20, 0);
            int pos2 = Math.Min(pos + 20, text.Length - 1);

            StringBuilder b = new StringBuilder();

            b.Append("\r\n");
            b.Append(text.Substring(pos1, pos2 - pos1 + 1)).Append("\r\n");

            for (int i = pos1; i <= pos - pos1 - 1; i++)
            {
                b.Append(' ');
            }

            b.Append("^\r\n\r\n");
            b.Append(message).Append("\r\n");


            throw new LexiconizationException(b.ToString());
        }
    }
}