using System;

namespace Cherimoya.Expressions
{
    public class CompileException : Exception
    {
        public CompileException(string message) :
            base(message)
        {
        }
    }
}