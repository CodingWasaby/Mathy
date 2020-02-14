using Cherimoya.Expressions;
using Cherimoya.Language.JavaLike;
using Mathy.Utils.Dandelion;
using Mathy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Language
{
    class MathyTypeChecker : JavaLikeTypeChecker
    {
        public override void PerformTypeChecking(Expression expression, TypeCheckingContext context)
        {
            if (expression is MatrixExpression)
            {
                CheckMatrixExpression(expression as MatrixExpression, context);
            }
            else if (expression is MultiIndexingExpression)
            {
                CheckMultiIndexingExpression(expression as MultiIndexingExpression, context);
            }
            else if (expression is VariableContextExpression)
            {
                CheckVariableContextExpression(expression as VariableContextExpression, context);
            }
            else if (expression is MultipleVariableExpression)
            {
                CheckMultipleVariableExpression(expression as MultipleVariableExpression, context);
            }
            else if (expression is DictionaryExpression)
            {
                CheckDictionaryExpression(expression as DictionaryExpression, context);
            }
            else if (expression is ArrayExpression)
            {
                CheckArrayExpression(expression as ArrayExpression, context);
            }
            else if (expression is IterationSumExpression)
            {
                CheckIterationSumExpression(expression as IterationSumExpression, context);
            }
            else if (expression is PredefinedConstantExpression)
            {
                CheckPredefinedConstantExpression(expression as PredefinedConstantExpression, context);
            }
            else
            {
                base.PerformTypeChecking(expression, context);
            }
        }


        private void CheckMatrixExpression(MatrixExpression e, TypeCheckingContext context)
        {
            foreach (Expression[] row in e.Rows)
            {
                foreach (Expression item in row)
                {
                    PerformTypeChecking(item, context);
                    if (!Types.IsNumberType(item.Type))
                    {
                        context.ErrorProvider.ThrowException("An element of a matrix must be convertialbe to double.", item);
                    }
                }
            }

            e.Type = typeof(Matrix);
        }

        protected override void CheckBinaryExpresionType(BinaryExpression e, TypeCheckingContext context)
        {
            if (e.Operator == BinaryOperator.Assign)
            {
                if (e.Left is MultiIndexingExpression)
                {
                    e.Type = typeof(double);
                }
                else if (e.Left is MultipleVariableExpression)
                {
                    e.Type = e.Right.Type;
                }
            }
            if (e.Operator == BinaryOperator.Add)
            {
                if (e.Left.Type == typeof(Matrix) && e.Right.Type == typeof(Matrix))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Left.Type == typeof(Matrix) && Types.IsNumberType(e.Right.Type))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Right.Type == typeof(Matrix) && Types.IsNumberType(e.Left.Type))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Left.Type == typeof(Vector) && Types.IsNumberType(e.Right.Type))
                {
                    e.Type = typeof(Vector);
                }
                else if (e.Right.Type == typeof(Vector) && Types.IsNumberType(e.Left.Type))
                {
                    e.Type = typeof(Vector);
                }
            }
            else if (e.Operator == BinaryOperator.Subtract)
            {
                if (e.Left.Type == typeof(Matrix) && e.Right.Type == typeof(Matrix))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Left.Type == typeof(Matrix) && Types.IsNumberType(e.Right.Type))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Right.Type == typeof(Matrix) && Types.IsNumberType(e.Left.Type))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Left.Type == typeof(Vector) && Types.IsNumberType(e.Right.Type))
                {
                    e.Type = typeof(Vector);
                }
                else if (e.Right.Type == typeof(Vector) && Types.IsNumberType(e.Left.Type))
                {
                    e.Type = typeof(Vector);
                }
            }
            else if (e.Operator == BinaryOperator.Multiply)
            {
                if (e.Left.Type == typeof(Matrix) && e.Right.Type == typeof(Matrix))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Left.Type == typeof(Matrix) && Types.IsNumberType(e.Right.Type))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Right.Type == typeof(Matrix) && Types.IsNumberType(e.Left.Type))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Left.Type == typeof(Vector) && Types.IsNumberType(e.Right.Type))
                {
                    e.Type = typeof(Vector);
                }
                else if (e.Right.Type == typeof(Vector) && Types.IsNumberType(e.Left.Type))
                {
                    e.Type = typeof(Vector);
                }
            }
            else if (e.Operator == BinaryOperator.Divide)
            {
                if (e.Left.Type == typeof(Matrix) && Types.IsNumberType(e.Right.Type))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Right.Type == typeof(Matrix) && Types.IsNumberType(e.Left.Type))
                {
                    e.Type = typeof(Matrix);
                }
                else if (e.Left.Type == typeof(Vector) && Types.IsNumberType(e.Right.Type))
                {
                    e.Type = typeof(Vector);
                }
                else if (e.Right.Type == typeof(Vector) && Types.IsNumberType(e.Left.Type))
                {
                    e.Type = typeof(Vector);
                }
            }


            if (e.Type == null)
            {
                base.CheckBinaryExpresionType(e, context);
            }
        }


        private void CheckMultiIndexingExpression(MultiIndexingExpression e, TypeCheckingContext context)
        {
            PerformTypeChecking(e.Operand, context);

            foreach (Expression indexer in e.Indexers)
            {
                PerformTypeChecking(indexer, context);
            }


            if (!e.Indexers.All(i => Types.IsNumberType(i.Type)))
            {
                context.ErrorProvider.ThrowException("Matrix multi indexer parameters must be of number types.", e);
            }


            if (e.Operand.Type.IsArray)
            {
                if (e.Indexers.Length != 1)
                {
                    context.ErrorProvider.ThrowException("Array multi indexer must be of 1 parameter.", e);
                }
                else
                {
                    e.Type = e.Operand.Type.GetElementType();
                }
            }
            else if (e.Operand.Type == typeof(Matrix))
            {
                if (e.Indexers.Length == 1)
                {
                    e.Type = typeof(double[]);
                }
                else if (e.Indexers.Length == 2)
                {
                    e.Type = typeof(double);
                }
                else
                {
                    context.ErrorProvider.ThrowException("Matrix multi indexer must be of 1 or 2 parameters.", e);
                }
            }
            else if (e.Operand.Type == typeof(Vector))
            {
                if (e.Indexers.Length == 1)
                {
                    e.Type = typeof(double);
                }
                else
                {
                    context.ErrorProvider.ThrowException("Vector multi indexer must be of 1 parameter.", e);
                }
            }
            else
            {
                context.ErrorProvider.ThrowException("Only matrix can use multi indexer.", e);
            }
        }

        private void CheckVariableContextExpression(VariableContextExpression e, TypeCheckingContext context)
        {
            foreach (VariableInfo variable in e.Variables)
            {
                context.VariableContext.Set(variable.Name, context.CreateAutoCreatedVariableValue(variable.Type));
            }


            PerformTypeChecking(e.Expression, context);

            foreach (VariableInfo variable in e.Variables)
            {
                context.VariableContext.Remove(variable.Name);
            }


            e.Type = typeof(VariableContextExpression);
            e.Evaluator = new MathyExpressionEvaluator();
        }

        private void CheckMultipleVariableExpression(MultipleVariableExpression e, TypeCheckingContext context)
        {
            List<Type> elementTypes = new List<Type>();

            int index = 0;
            foreach (string variable in e.Variables)
            {
                Type elementType = context.RightOperandType.GenericTypeArguments[index];
                elementTypes.Add(elementType);
                context.VariableContext.Set(variable, context.CreateAutoCreatedVariableValue(elementType));
                index++;

                if (index > context.RightOperandType.GenericTypeArguments.Length - 1)
                {
                    break;
                }
            }


            Type baseType = null;

            int elementCount = elementTypes.Count;

            if (elementCount == 1)
            {
                baseType = typeof(Tuple<>);
            }
            else if (elementCount == 2)
            {
                baseType = typeof(Tuple<,>);
            }
            else if (elementCount == 3)
            {
                baseType = typeof(Tuple<,,>);
            }
            else if (elementCount == 4)
            {
                baseType = typeof(Tuple<,,,>);
            }
            else if (elementCount == 5)
            {
                baseType = typeof(Tuple<,,,,>);
            }
            else if (elementCount == 6)
            {
                baseType = typeof(Tuple<,,,,,>);
            }

            e.Type = baseType.MakeGenericType(elementTypes.ToArray());
        }

        private void CheckDictionaryExpression(DictionaryExpression e, TypeCheckingContext context)
        {
            foreach (Expression value in e.Dictionary.Values)
            {
                PerformTypeChecking(value, context);
            }


            e.Type = typeof(Dictionary<string, object>);
        }

        private void CheckArrayExpression(ArrayExpression e, TypeCheckingContext context)
        {
            List<List<Type>> baseTypes = new List<List<Type>>();

            foreach (Expression item in e.Items)
            {
                PerformTypeChecking(item, context);

                List<Type> types = new List<Type>();
                Type current = item.Type;
                while (current != null)
                {
                    types.Add(current);
                    current = current.BaseType;
                }

                baseTypes.Add(types);
            }


            Type commonBaseType = baseTypes[0].FirstOrDefault(i => baseTypes.Skip(1).All(j => j.Contains(i)));

            if (commonBaseType != typeof(ValueType))
            {
                e.Type = commonBaseType.MakeArrayType();
            }
            else
            {
                e.Type = Types.GetPrimitiveType(e.Items.Min(i => Types.GetPrimitiveTypeIndex(i.Type))).MakeArrayType();
            }
        }

        private void CheckIterationSumExpression(IterationSumExpression e, TypeCheckingContext context)
        {
            foreach (IterationSumVariable variable in e.Variables)
            {
                PerformTypeChecking(variable.From, context);
                PerformTypeChecking(variable.To, context);


                if (!Types.IsNumberType(variable.From.Type))
                {
                    context.ErrorProvider.ThrowException(string.Format("Start value of {0} is not number.", variable.Name), e);
                }

                if (!Types.IsNumberType(variable.To.Type))
                {
                    context.ErrorProvider.ThrowException(string.Format("End value of {1} is not number.", variable.Name), e);
                }


                context.VariableContext.Set(variable.Name, 0);
            }

            PerformTypeChecking(e.Body, context);


            foreach (IterationSumVariable variable in e.Variables)
            {
                context.VariableContext.Remove(variable.Name);
            }

            if (!Types.IsNumberType(e.Body.Type))
            {
                context.ErrorProvider.ThrowException("Can only perform iteration sum on an expression of number type.", e);
            }

            e.Type = typeof(double);
        }

        private void CheckPredefinedConstantExpression(PredefinedConstantExpression e, TypeCheckingContext context)
        {
            e.Type = e.Value.GetType();
        }

        protected override Type ResolveLambdaParameterType(Type operandType, int index)
        {
            if (operandType == typeof(Matrix))
            {
                if (index == 0)
                {
                    return typeof(double[]);
                }
                else if (index == 1)
                {
                    return typeof(int);
                }
            }


            return base.ResolveLambdaParameterType(operandType, index);
        }
    }
}
