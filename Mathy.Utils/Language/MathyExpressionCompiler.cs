using Cherimoya.Expressions;
using Cherimoya.Language;
using Cherimoya.Language.JavaLike;
using Mathy.Utils.Dandelion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Language
{
    public class MathyExpressionCompiler : JavaLikeExpressionCompiler
    {
        public override bool CreateVariableOnAssing()
        {
            return true;
        }

        protected override Expression CompileIndexingExpression(Expression operand, int lambdaLevel)
        {
            PushPosition();
            Move();

            List<Expression> indexers = new List<Expression>();

            while (true)
            {
                indexers.Add(Compile(lambdaLevel));

                if (IsPunctuationOf(","))
                {
                    Move();
                }
                else
                {
                    SkipPunctuation("]");
                    break;
                }
            }

            Expression result = new MultiIndexingExpression(operand, indexers.ToArray(), PeekPos(), Pos);
            PopPosition();

            return result;
        }

        protected override Expression OnCustomCompileValue(int lambdaLevel)
        {
            if (IsPunctuationOf("["))
            {
                return CompileArray(lambdaLevel);
            }
            if (IsPunctuationOf("{"))
            {
                return CompileDictionary(lambdaLevel);
            }
            if (IsPunctuationOf("@"))
            {
                Move();
                if (IsPunctuationOf("["))
                {
                    return CompileMatrix(lambdaLevel);
                }
                else
                {
                    return CompileVariableContextExpression(lambdaLevel);
                }
            }
            else if (IsWord && GetWord() == "isum")
            {
                return CompileIterationSum(lambdaLevel);
            }
            else if (IsPunctuationOf("#"))
            {
                Move();
                if (IsPunctuationOf("{"))
                {
                    return CompileVariableExpression(lambdaLevel);
                }
                else
                {
                    return CompilePredefinedConstantExpression();
                }
            }
            else
            {
                return null;
            }
        }

        private Expression CompileVariableExpression(int lambdaLevel)
        {
            SkipPunctuation("{");

            List<string> variables = new List<string>();

            PushPosition();

            while (true)
            {
                variables.Add(GetWord());
                Move();

                if (IsPunctuationOf(","))
                {
                    Move();
                }
                else
                {
                    break;
                }
            }


            SkipPunctuation("}");

            Expression result =
                variables.Count == 0 ?
                (Expression)new VariableExpression(variables[0], PeekPos(), Pos) :
                (Expression)new MultipleVariableExpression(variables.ToArray(), PeekPos(), Pos);

            PopPosition();


            return result;
        }

        private Expression CompileMatrix(int lambdaLevel)
        {
            PushPosition();

            Move();

            List<List<Expression>> elements = new List<List<Expression>>();

            while (true)
            {
                Expression element = Compile(lambdaLevel);

                if (elements.Count == 0)
                {
                    elements.Add(new List<Expression>());
                }

                elements.Last().Add(element);

                if (IsPunctuationOf(","))
                {
                    Move();
                }
                else if (IsPunctuationOf(";"))
                {
                    Move();
                    elements.Add(new List<Expression>());
                }
                else if (IsPunctuationOf("]"))
                {
                    Move();
                    break;
                }
                else
                {
                    ThrowExpects(", ; or ]");
                }
            }

            elements = elements.Where(i => i.Count > 0).ToList();


            if (elements.Count == 0)
            {
                ThrowException("A matrix must have at least one row.");
            }
            else if (elements[0].Count == 0)
            {
                ThrowException("A matrix must have at least one column.");
            }
            else if (!elements.Skip(1).All(i => i.Count == elements[0].Count))
            {
                ThrowException("Each row of the matrix must have the same count of elements.");
            }


            MatrixExpression result = new MatrixExpression(elements.Select(i => i.ToArray()).ToArray(), PeekPos(), Pos);
            PopPosition();

            return result;
        }

        private Expression CompileVariableContextExpression(int lambdaLevel)
        {
            PushPosition();

            List<VariableInfo> variables = new List<VariableInfo>();

            while (!IsPunctuationOf("->"))
            {
                string variableName = GetWord();
                Move();


                Type clazz = null;

                if (IsPunctuationOf(":"))
                {
                    SkipPunctuation(":");

                    string className = ReadClassName();
                    try
                    {
                        clazz = Types.GetType(className, LanguageService.GetImportedPackages());
                    }
                    catch
                    {
                        ThrowException(string.Format("{0} not defined.", className));
                    }
                }
                else
                {
                    clazz = typeof(double);
                }

                variables.Add(new VariableInfo() { Name = variableName, Type = clazz });


                if (IsPunctuationOf(","))
                {
                    Move();
                }
            }


            Move();

            Expression expression = Compile(lambdaLevel);

            VariableContextExpression result = new VariableContextExpression(expression, variables.ToArray(), PeekPos(), Pos);
            PopPosition();


            return result;
        }

        private Expression CompileDictionary(int lambdaLevel)
        {
            PushPosition();

            Dictionary<string, Expression> dict = new Dictionary<string, Expression>();


            SkipPunctuation("{");

            if (!IsPunctuationOf("}"))
            {
                while (true)
                {
                    string key = GetWord();
                    Move();

                    SkipPunctuation(":");
                    Expression value = Compile(lambdaLevel);

                    dict.Add(key, value);

                    if (IsPunctuationOf(","))
                    {
                        Move();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            SkipPunctuation("}");


            DictionaryExpression result = new DictionaryExpression(dict, PeekPos(), Pos);
            PopPosition();

            return result;
        }

        private Expression CompileArray(int lambdaLevel)
        {
            PushPosition();

            SkipPunctuation("[");

            List<Expression> items = new List<Expression>();

            if (!IsPunctuationOf("]"))
            {
                while (true)
                {
                    items.Add(Compile(lambdaLevel));

                    if (IsPunctuationOf(","))
                    {
                        Move();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            SkipPunctuation("]");

            ArrayExpression result = new ArrayExpression(items.ToArray(), PeekPos(), Pos);
            PopPosition();

            return result;
        }

        private Expression CompileIterationSum(int lambdaLevel)
        {
            PushPosition();

            Move();
            SkipPunctuation("(");


            List<IterationSumVariable> variables = new List<IterationSumVariable>();
            Expression body;

            while (true)
            {
                bool isVariable;

                PushPosition();

                Move();
                isVariable = IsPunctuationOf(":");

                Pos = PeekPos();


                if (isVariable)
                {
                    if (!IsWord)
                    {
                        ThrowException("Expecting iteration variable name.");
                    }

                    string variableName = GetWord();
                    Move();

                    SkipPunctuation(":");

                    Expression from = Compile(lambdaLevel);
                    SkipPunctuation("->");
                    Expression to = Compile(lambdaLevel);

                    SkipPunctuation(",");

                    variables.Add(new IterationSumVariable() { From = from, To = to, Name = variableName });
                }
                else
                {
                    body = Compile(lambdaLevel);
                    SkipPunctuation(")");
                    break;
                }
            }

            Expression result = new IterationSumExpression(variables.ToArray(), body, PeekPos(), Pos);
            PopPosition();

            return result;
        }

        private Expression CompilePredefinedConstantExpression()
        {
            PushPosition();

            if (!IsWord)
            {
                ThrowException("Expect word.");
            }

            string name = GetWord();
            Move();

            object value = MathyConstants.GetValue(name);
            if (value == null)
            {
                ThrowException(string.Format("Undefined constant {0}", name));
            }

            Expression expression = new PredefinedConstantExpression("#" + name, value, PeekPos(), Pos);
            PopPosition();

            return expression;
        }
    }
}
