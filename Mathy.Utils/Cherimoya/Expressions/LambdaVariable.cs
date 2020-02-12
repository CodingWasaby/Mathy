using System;

namespace Cherimoya.Expressions
{
    public class LambdaVariable
    {
        public string Name { get; set; }

        public Expression CallerMethod { get; set; }

        public Type Type { get; set; }

        public int Index { get; set; }
    }
}