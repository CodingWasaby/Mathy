using Cherimoya.Evaluation;
using Cherimoya.Expressions;
using Cherimoya.Lexicons;
using System.Collections.Generic;

namespace Cherimoya.Language
{
    public abstract class LanguageService
    {
        private static List<string> importedPackages = new List<string>();

        public static string[] GetImportedPackages()
        {
            return importedPackages.ToArray();
        }

        public static void ImportNamespace(string packageName)
        {
            if (!importedPackages.Contains(packageName))
            {
                importedPackages.Add(packageName);
            }
        }


        public Expression[] Compile(string s, VariableContext variableContext)
        {
            ExpressionCompiler compiler = CreateCompiler();


            Lexicon[] lexicons;

            try
            {
                lexicons = new Lexiconizer(LexiconizationRuleTable.GetRule(compiler)).Lexiconize(s);
            }
            catch (LexiconizationException e)
            {
                throw new CompileException(e.Message);
            }


            CompileErrorProvider errorProvider = new CompileErrorProvider(s, lexicons, compiler.PerformTypeChecking);
            Expression[] roots = compiler.Compile(s, lexicons, errorProvider);

            if (variableContext == null)
            {
                variableContext = new VariableContext();
            }


            foreach (Expression root in roots)
            {
                TypeCheckingContext context = new TypeCheckingContext();
                OnCreateTypeCheckingContext(context);

                context.CreateVariableOnAssign = compiler.CreateVariableOnAssing();
                context.VariableContext = variableContext;
                context.LambdaContext = new LambdaContext();
                context.ErrorProvider = errorProvider;

                CreateTypeChecker().PerformTypeChecking(root, context);
            }

            return roots;
        }

        public TypeCheckingContext CreateTypeCheckingContext(VariableContext variableContext)
        {
            ExpressionCompiler compiler = CreateCompiler();

            TypeCheckingContext context = new TypeCheckingContext();
            OnCreateTypeCheckingContext(context);

            CompileErrorProvider errorProvider = new CompileErrorProvider(null, null, compiler.PerformTypeChecking);

            if (variableContext == null)
            {
                variableContext = new VariableContext();
            }


            context.CreateVariableOnAssign = compiler.CreateVariableOnAssing();
            context.VariableContext = variableContext;
            context.LambdaContext = new LambdaContext();
            context.ErrorProvider = errorProvider;

            return context;
        }


        public abstract void OnCreateTypeCheckingContext(TypeCheckingContext context);

        public abstract ExpressionCompiler CreateCompiler();

        public abstract TypeChecker CreateTypeChecker();

        public abstract ExpressionEvaluator CreateEvaluator();
    }
}
