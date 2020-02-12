using Cherimoya.Evaluation;
using Cherimoya.Expressions;

namespace Cherimoya.Language.Java
{
    public class JavaLanguageService : LanguageService
    {
        public override void OnCreateTypeCheckingContext(TypeCheckingContext context)
        {
        }

        public override ExpressionCompiler CreateCompiler()
        {
            return new JavaExpressionCompiler();
        }

        public override TypeChecker CreateTypeChecker()
        {
            return new JavaTypeChecker();
        }

        public override ExpressionEvaluator CreateEvaluator()
        {
            return new JavaExpressionEvaluator();
        }
    }
}
