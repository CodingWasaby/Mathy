using System;

namespace Cherimoya.Lexicons
{
    public class LexiconizationException : Exception
    {
        public LexiconizationException(string message) :
            base(message)
        {
        }
    }
}