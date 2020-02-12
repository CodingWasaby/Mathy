using Cherimoya.Expressions;
using Cherimoya.Lexicons;
using Mathy.Utils.Dandelion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cherimoya.Language.JavaLike
{
    public abstract class JavaLikeExpressionCompiler : ExpressionCompiler
    {
        protected abstract Expression OnCustomCompileValue(int lambdaLevel);


        internal override LexiconizationRule GetLexiconizationRule()
        {
            return LexiconizationRuleFactory.CreateRuleForJava();
        }

        public override bool CreateVariableOnAssing()
        {
            return false;
        }


        protected override Expression[] Compile()
        {
            List<Expression> expressions = new List<Expression>();

            while (!End)
            {
                expressions.Add(Compile(1));

                if (IsPunctuationOf(";"))
                {
                    Move();
                }
                else if (!End)
                {
                    ThrowException("Expression should end here. Extra characters found.");
                }
            }

            return expressions.ToArray();
        }

        protected Expression Compile(int lambdaLevel)
        {
            Expression expression = CompileBinaryExpression(0, lambdaLevel);

            if (IsPunctuationOf("?"))
            {
                PushPosition();
                Move();
                Expression positive = Compile(lambdaLevel);
                SkipPunctuation(":");
                Expression negative = Compile(lambdaLevel);
                IfElseExpression ifElse = new IfElseExpression(expression, positive, negative, PeekPos(), Pos);
                PopPosition();
                return ifElse;
            }
            else
            {
                return expression;
            }
        }

        private Expression CompileUnaryExpression(int lambdaLevel)
        {
            Expression current = null;

            UnaryOperator op = UnaryOperator.None;

            if (IsPunctuationOf("+"))
            {
                op = UnaryOperator.Identity;
            }
            else if (IsPunctuationOf("-"))
            {
                op = UnaryOperator.Negation;
            }
            else if (IsPunctuationOf("!"))
            {
                op = UnaryOperator.Not;
            }

            if (op != UnaryOperator.None)
            {
                PushPosition();
                Move();
                int pos = PeekPos();
                current = new UnaryExpression(op, CompileUnaryExpression(lambdaLevel), pos, Pos);
                PopPosition();
            }
            else
            {
                current = CompileValue(lambdaLevel);
            }

            return current;
        }

        private Expression CompileValue(int lambdaLevel)
        {
            PushPosition();

            Type ownerType = null;

            Expression current = OnCustomCompileValue(lambdaLevel);
            if (current != null)
            {
            }
            else if (End)
            {
                ThrowException("Expects a value.");
            }
            else if (IsConstant)
            {
                current = new ConstantExpression(GetConstant(), PeekPos(), Pos);
                Move();
            }
            else if ((ownerType = PredictIsClassName()) != null)
            {
                if (!IsPunctuationOf("."))
                {
                    ThrowExpects(".");
                }
            }
            else if (PredictIsLambda())
            {
                current = CompileLambdaExpression(lambdaLevel);
            }
            else if (PredictIsCast())
            {
                current = CompileCastExpression(lambdaLevel);
            }
            else if (IsWord)
            {
                PushPosition();

                string word = GetWord();
                Move();

                if (word == "true")
                {
                    current = new ConstantExpression(true, PeekPos(), Pos);
                }
                else if (word == "false")
                {
                    current = new ConstantExpression(false, PeekPos(), Pos);
                }
                else if (word == "null")
                {
                    current = new ConstantExpression(null, PeekPos(), Pos);
                }
                else if (IsPunctuationOf("("))
                {
                    Move();
                    current = CompileFunctionCall(word, lambdaLevel);
                }
                else if (word == "new")
                {
                    current = CompileInstantiazation(lambdaLevel);
                }
                else
                {
                    current = new VariableExpression(word, PeekPos(), Pos);
                    PopPosition();
                }
            }
            else if (IsPunctuationOf("("))
            {
                Move();
                current = Compile(lambdaLevel);
                SkipPunctuation(")");
            }
            else
            {
                ThrowException("Unexpected token");
            }

            PopPosition();


            while (true)
            {

                PushPosition();

                if (IsPunctuationOf("."))
                {

                    Move();

                    if (!IsWord)
                    {
                        PushPosition();
                        ThrowException("Identifier expected.");
                    }

                    if (Next is PunctuationLexicon && ((PunctuationLexicon)Next).Value == "(")
                    {
                        PushPosition();
                    }

                    string fieldName = GetWord();
                    Move();

                    if (IsPunctuationOf("("))
                    {
                        current = CompileMethodCall(current, ownerType, fieldName, lambdaLevel);
                    }
                    else
                    {
                        current = new FieldExpression(current, ownerType, fieldName, PeekPos(), Pos);
                    }

                    ownerType = null;

                    PopPosition();
                }
                else if (IsPunctuationOf("["))
                {
                    current = CompileIndexingExpression(current, lambdaLevel);
                    PopPosition();
                }
                else
                {
                    PopPosition();
                    break;
                }
            }

            return current;

        }

        protected virtual Expression CompileIndexingExpression(Expression operand, int lambdaLevel)
        {
            Move();
            Expression indexer = Compile(lambdaLevel);
            SkipPunctuation("]");
            return new IndexingExpression(operand, indexer, PeekPos(), Pos);
        }

        private Expression CompileBinaryExpression(int baseLevel, int lambdaLevel)
        {
            Expression current = CompileUnaryExpression(lambdaLevel);

            PushPosition();


            while (!End)
            {
                BinaryOperator op = ExpressionSnippets.GetBinaryOperator(GetPunctuation());

                if (op == BinaryOperator.None)
                {
                    break;
                }


                if (ExpressionSnippets.GetOperatorLevel(op) <= baseLevel)
                {
                    break;
                }
                else
                {
                    Move();

                    int fromPos = PeekPos();
                    int toPos = Pos;

                    current = new BinaryExpression(op, current, CompileBinaryExpression(ExpressionSnippets.GetOperatorLevel(op), lambdaLevel), fromPos, toPos);
                }
            }

            PopPosition();
            return current;
        }

        private Expression CompileFunctionCall(string functionName, int lambdaLevel)
        {
            List<Expression> parameters = new List<Expression>();

            while (!End)
            {
                if (IsPunctuationOf(")"))
                {
                    Move();
                    break;
                }

                parameters.Add(Compile(lambdaLevel));
                if (IsPunctuationOf(","))
                {
                    Move();
                }
                else if (!IsPunctuationOf(")"))
                {
                    ThrowExpects(")");
                }
            }


            FunctionCallExpression func = new FunctionCallExpression(functionName, parameters.ToArray(), PeekPos(), Pos);
            PopPosition();
            return func;
        }

        private Expression CompileMethodCall(Expression operand, Type ownerType, string methodName, int lambdaLevel)
        {

            Move();


            List<Expression> parameters = new List<Expression>();

            while (!End)
            {

                if (IsPunctuationOf(")"))
                {
                    Move();
                    break;
                }

                parameters.Add(Compile(lambdaLevel));
                if (IsPunctuationOf(","))
                {
                    Move();
                }
                else if (!IsPunctuationOf(")"))
                {
                    ThrowExpects(")");
                }
            }


            MethodCallExpression method =
                    ownerType == null ?
                    new MethodCallExpression(operand, methodName, parameters.ToArray(), PeekPos(), Pos) :
                    new MethodCallExpression(ownerType, methodName, parameters.ToArray(), PeekPos(), Pos);
            PopPosition();
            return method;
        }

        private Expression CompileCastExpression(int lambdaLevel)
        {

            PushPosition();

            SkipPunctuation("(");
            string className = ReadClassName();
            SkipPunctuation(")");

            Expression operand = CompileUnaryExpression(lambdaLevel);
            CastExpression cast = new CastExpression(operand, className, PeekPos(), Pos);

            PopPosition();

            return cast;
        }

        private Expression CompileInstantiazation(int lambdaLevel)
        {

            PushPosition();

            string className = ReadClassName();
            Type clazz = null;
            try
            {
                clazz = Types.GetType(className, LanguageService.GetImportedPackages());
            }
            catch
            {
                ThrowException(string.Format("{0} not defined.", className));
            }


            PopPosition();

            if (IsPunctuationOf("("))
            {
                return CompileCreateObjectExpression(clazz, lambdaLevel);
            }
            else if (IsPunctuationOf("["))
            {
                return CompileCreateArrayExpression(clazz, lambdaLevel);
            }
            else
            {
                ThrowExpects("(, [");
            }

            return null;
        }

        private Expression CompileCreateObjectExpression(Type clazz, int lambdaLevel)
        {

            PushPosition();

            SkipPunctuation("(");

            List<Expression> parameters = new List<Expression>();

            while (!End)
            {

                if (IsPunctuationOf(")"))
                {
                    Move();
                    break;
                }

                parameters.Add(Compile(lambdaLevel));
                if (IsPunctuationOf(","))
                {
                    Move();
                }
                else if (!IsPunctuationOf(")"))
                {
                    ThrowExpects(")");
                }
            }


            CreateObjectExpression expr = new CreateObjectExpression(parameters.ToArray(), clazz, PeekPos(), Pos);
            PopPosition();
            return expr;
        }

        private Expression CompileCreateArrayExpression(Type clazz, int lambdaLevel)
        {

            PushPosition();

            SkipPunctuation("[");
            SkipPunctuation("]");
            SkipPunctuation("{");

            List<Expression> elements = new List<Expression>();

            while (!End)
            {

                if (IsPunctuationOf("}"))
                {
                    Move();
                    break;
                }


                PushPosition();

                Expression expression = Compile(lambdaLevel);

                if (IsPunctuationOf("..."))
                {

                    Move();
                    Expression from = expression;
                    Expression to = Compile(lambdaLevel);
                    Expression by = new ConstantExpression(1, Pos, Pos);
                    if (IsWord)
                    {
                        if (GetWord() != "by")
                        {
                            ThrowExpects("by");
                        }
                        Move();
                        by = Compile(lambdaLevel);
                    }

                    elements.Add(new CondensedArrayExpression(clazz, from, to, by, PeekPos(), Pos));
                }
                else
                {
                    elements.Add(expression);
                }

                PopPosition();

                if (IsPunctuationOf(","))
                {
                    Move();
                }
                else if (!IsPunctuationOf("}"))
                {
                    ThrowExpects("}");
                }
            }


            CreateArrayExpression expr = new CreateArrayExpression(elements.ToArray(), clazz, PeekPos(), Pos);
            PopPosition();
            return expr;
        }

        private Expression CompileLambdaExpression(int lambdaLevel)
        {

            PushPosition();

            List<string> variableNames = new List<string>();

            if (IsPunctuationOf("("))
            {

                Move();

                while (!End)
                {

                    if (IsPunctuationOf(")"))
                    {
                        Move();
                        break;
                    }

                    string variable = GetWord();
                    variableNames.Add(variable);
                    Move();

                    if (IsPunctuationOf(","))
                    {
                        Move();
                    }
                    else if (!IsPunctuationOf(")"))
                    {
                        ThrowExpects(")");
                    }
                }
            }
            else
            {
                string variable = GetWord();
                variableNames.Add(variable);
                Move();
            }

            SkipPunctuation("->");
            Expression body = Compile(lambdaLevel + 1);


            LambdaExpression expr = new LambdaExpression(variableNames.ToArray(), body, lambdaLevel, PeekPos(), Pos);
            PopPosition();
            return expr;
        }


        protected string ReadClassName()
        {

            StringBuilder b = new StringBuilder();

            while (!End)
            {

                if (IsWord)
                {
                    b.Append(GetWord());
                    Move();
                }
                else if (IsPunctuationOf("."))
                {
                    b.Append('.');
                    Move();
                }
                else
                {
                    break;
                }
            }

            return b.ToString();
        }


        private bool PredictIsCast()
        {

            if (!IsPunctuationOf("("))
            {
                return false;
            }


            bool result = false;

            Save();
            Move();

            while (!End)
            {
                if (IsPunctuationOf(")"))
                {
                    Move();
                    if (IsConstant || IsWord || IsPunctuationOf("("))
                    {
                        result = true;
                    }
                    break;
                }
                else if (!(IsWord || IsPunctuationOf(".")))
                {
                    break;
                }
                else
                {
                    Move();
                }
            }

            GoBack();

            return result;
        }

        private bool PredictIsLambda()
        {

            bool result = false;

            Save();


            if (IsWord)
            {
                Move();
                result = IsPunctuationOf("->");
            }
            else if (IsPunctuationOf("("))
            {
                int leftParenCount = 0;
                int rightParenCount = 0;

                while (!End)
                {

                    if (IsPunctuationOf("("))
                    {
                        leftParenCount++;
                        if (leftParenCount > 1)
                        {
                            break;
                        }
                    }
                    else if (IsPunctuationOf(")"))
                    {
                        rightParenCount++;
                        if (rightParenCount > 1)
                        {
                            break;
                        }
                    }
                    else if (IsPunctuationOf("->"))
                    {
                        result = leftParenCount == 1 && rightParenCount == 1;
                    }

                    Move();
                }
            }

            GoBack();

            return result;
        }

        private Type PredictIsClassName()
        {

            if (!IsWord)
            {
                return null;
            }


            Save();

            StringBuilder b = new StringBuilder();
            Type ownerType = null;

            while (!End)
            {

                if (IsWord)
                {
                    b.Append(GetWord());
                    Move();
                    try
                    {
                        ownerType = Types.GetType(b.ToString(), LanguageService.GetImportedPackages());
                        break;
                    }
                    catch (Exception e)
                    {
                    }
                }
                else if (IsPunctuationOf("."))
                {
                    b.Append('.');
                    Move();
                }
                else
                {
                    break;
                }
            }

            if (ownerType == null)
            {
                GoBack();
            }


            return ownerType;
        }
    }
}