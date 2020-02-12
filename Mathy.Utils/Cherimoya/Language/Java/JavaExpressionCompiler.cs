using Cherimoya.Expressions;
using Cherimoya.Language.JavaLike;
using System;

namespace Cherimoya.Language.Java
{
    public class JavaExpressionCompiler : JavaLikeExpressionCompiler
    {
        protected override Expression OnCustomCompileValue(int lambdaLevel)
        {
            throw new NotImplementedException();
        }
    }
}
