using Cherimoya.Expressions;
using Cherimoya.Language.JavaLike;
using Mathy.Utils.Dandelion;
using Mathy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Language
{
    class MathyExpressionEvaluator : JavaLikeExpressionEvaluator
    {
        public override object Evaluate(Expression expression, VariableContext context)
        {
            object result = null;
            bool isEvaluated = false;

            if (expression is MatrixExpression)
            {
                result = EvaluateMatrixExpression(expression as MatrixExpression, context);
                isEvaluated = true;
            }
            else if (expression is MultiIndexingExpression)
            {
                result = EvaluteMultiIndexingExpression(expression as MultiIndexingExpression, context);
                isEvaluated = true;
            }
            else if (expression is VariableContextExpression)
            {
                result = EvaluateVariableContextExpression(expression as VariableContextExpression, context);
                isEvaluated = true;
            }
            else if (expression is DictionaryExpression)
            {
                result = EvaluateDictionaryExpression(expression as DictionaryExpression, context);
                isEvaluated = true;
            }
            else if (expression is ArrayExpression)
            {
                result = EvaluateArrayExpression(expression as ArrayExpression, context);
                isEvaluated = true;
            }
            else if (expression is IterationSumExpression)
            {
                result = EvaluateIterationSumExpression(expression as IterationSumExpression, context);
                isEvaluated = true;
            }
            else if (expression is BinaryExpression)
            {
                result = EvaluteBinaryExpression(expression as BinaryExpression, context, out isEvaluated);
            }
            else if (expression is PredefinedConstantExpression)
            {
                result = EvalutePredefinedConstantExpression(expression as PredefinedConstantExpression, context);
                isEvaluated = true;
            }

            if (!isEvaluated)
            {
                result = base.Evaluate(expression, context);
            }


            return result;
        }


        private object EvaluateMatrixExpression(MatrixExpression e, VariableContext context)
        {
            List<double> items = new List<double>();

            foreach (Expression[] row in e.Rows)
            {
                foreach (Expression item in row)
                {
                    items.Add(Convert.ToDouble(Evaluate(item, context)));
                }
            }


            return new Matrix(e.RowCount, e.ColumnCount, items.ToArray());
        }

        private object EvaluteMultiIndexingExpression(MultiIndexingExpression e, VariableContext context)
        {
            object value = Evaluate(e.Operand, context);
            int[] indexers = e.Indexers.Select(i => Types.ConvertValue<int>(Evaluate(i, context))).ToArray();

            if (e.Operand.Type.IsArray)
            {
                Array array = value as Array;

                return array.GetValue(indexers[0]);
            }
            else if (e.Operand.Type.Equals(typeof(Matrix)))
            {
                Matrix m = value as Matrix;

                if (indexers.Length == 1)
                {
                    return m.GetRow(indexers[0] - 1);
                }
                else
                {
                    return m[indexers[0] - 1, indexers[1] - 1];
                }
            }
            else
            {
                Vector v = value as Vector;

                return v[indexers[0] - 1];
            }
        }

        private object EvaluateVariableContextExpression(VariableContextExpression e, VariableContext context)
        {
            e.VariableContext = context;
            return e;
        }

        private object EvaluateDictionaryExpression(DictionaryExpression e, VariableContext context)
        {
            return e.Dictionary.ToDictionary(i => i.Key, i => Evaluate(i.Value, context));
        }

        private object EvaluateArrayExpression(ArrayExpression e, VariableContext context)
        {
            Array array = Array.CreateInstance(e.Type.GetElementType(), e.Items.Length);

            for (int i = 0; i <= e.Items.Length - 1; i++)
            {
                array.SetValue(Types.CoerceValue(Evaluate(e.Items[i], context), e.Type.GetElementType()), i);
            }
            return array;
        }

        private object EvaluateIterationSumExpression(IterationSumExpression e, VariableContext context)
        {
            int[] values = new int[e.Variables.Length];
            int[] fromValues = new int[e.Variables.Length];
            int[] toValues = new int[e.Variables.Length];

            int index = 0;
            foreach (IterationSumVariable variable in e.Variables)
            {
                if (context.HasVariable(variable.Name))
                {
                    throw new Exception(string.Format("Iteration variable {0} is already in use.", variable.Name));
                }


                fromValues[index] = Types.ConvertValue<int>(Evaluate(variable.From, context));
                toValues[index] = Types.ConvertValue<int>(Evaluate(variable.To, context));

                context.Set(variable.Name, fromValues[index]);
                values[index] = fromValues[index];


                index++;
            }


            double sum = 0;

            while (true)
            {
                double value = Types.ConvertValue<double>(Evaluate(e.Body, context)); ;

                if (!double.IsNaN(value))
                {
                    sum += value;
                }


                if (!IncreaseIterationValues(values, fromValues, toValues))
                {
                    break;
                }
                else
                {
                    for (int i = 0; i <= e.Variables.Length - 1; i++)
                    {
                        context.Set(e.Variables[i].Name, values[i]);
                    }
                }
            }


            foreach (IterationSumVariable variable in e.Variables)
            {
                context.Remove(variable.Name);
            }


            return sum;
        }

        private static bool IncreaseIterationValues(int[] values, int[] fromValues, int[] toValues)
        {
            int index = values.Length - 1;

            while (true)
            {
                if (values[index] == toValues[index])
                {
                    if (index == 0)
                    {
                        return false;
                    }

                    values[index] = fromValues[index];
                    index--;
                }
                else
                {
                    values[index]++;
                    return true;
                }
            }
        }

        private object EvaluteBinaryExpression(BinaryExpression e, VariableContext context, out bool isEvaluated)
        {
            isEvaluated = false;


            bool isTupleValue = e.Right.Type.Name.Contains("Tuple");

            if (e.Operator == BinaryOperator.Assign && (e.Left is MultipleVariableExpression || isTupleValue))
            {
                isEvaluated = true;

                object rightValue = Evaluate(e.Right, context);


                if (e.Left is MultipleVariableExpression)
                {
                    MultipleVariableExpression mv = e.Left as MultipleVariableExpression;

                    if (isTupleValue)
                    {
                        int count = mv.Type.GenericTypeArguments.Length;

                        for (int i = 0; i <= count - 1; i++)
                        {
                            context.Set(mv.Variables[i], rightValue.GetType().GetProperty("Item" + (i + 1)).GetValue(rightValue));
                        }
                    }
                    else
                    {
                        context.Set(mv.Variables[0], rightValue);
                    }
                }
                else
                {
                    context.Set((e.Left as VariableExpression).VariableName, rightValue.GetType().GetProperty("Item1").GetValue(rightValue));
                }


                return rightValue;
            }
            else if (e.Operator == BinaryOperator.Add)
            {
                if (e.Left.Type == typeof(Matrix) && e.Right.Type == typeof(Matrix))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Matrix).Add(Evaluate(e.Right, context) as Matrix);
                }
                else if (e.Left.Type == typeof(Matrix) && Types.IsNumberType(e.Right.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Matrix).Add(Types.ConvertValue<double>(Evaluate(e.Right, context)));
                }
                else if (e.Right.Type == typeof(Matrix) && Types.IsNumberType(e.Left.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Right, context) as Matrix).Add(Types.ConvertValue<double>(Evaluate(e.Left, context)));
                }
                else if (e.Left.Type == typeof(Vector) && Types.IsNumberType(e.Right.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Vector).Add(Types.ConvertValue<double>(Evaluate(e.Right, context)));
                }
                else if (e.Right.Type == typeof(Vector) && Types.IsNumberType(e.Left.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Right, context) as Vector).Add(Types.ConvertValue<double>(Evaluate(e.Left, context)));
                }
            }
            else if (e.Operator == BinaryOperator.Subtract)
            {
                if (e.Left.Type == typeof(Matrix) && e.Right.Type == typeof(Matrix))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Matrix).Add((Evaluate(e.Right, context) as Matrix).ToNegative());
                }
                else if (e.Left.Type == typeof(Matrix) && Types.IsNumberType(e.Right.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Matrix).Subtract(Types.ConvertValue<double>(Evaluate(e.Right, context)));
                }
                else if (e.Right.Type == typeof(Matrix) && Types.IsNumberType(e.Left.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Right, context) as Matrix).Subtract(Types.ConvertValue<double>(Evaluate(e.Left, context))).GetNegative();
                }
                else if (e.Left.Type == typeof(Vector) && Types.IsNumberType(e.Right.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Vector).Subtract(Types.ConvertValue<double>(Evaluate(e.Right, context)));
                }
                else if (e.Right.Type == typeof(Vector) && Types.IsNumberType(e.Left.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Right, context) as Vector).Subtract(Types.ConvertValue<double>(Evaluate(e.Left, context))).GetNegative();
                }
            }
            else if (e.Operator == BinaryOperator.Multiply)
            {
                if (e.Left.Type == typeof(Matrix) && e.Right.Type == typeof(Matrix))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Matrix).Multiply(Evaluate(e.Right, context) as Matrix);
                }
                else if (e.Left.Type == typeof(Matrix))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Matrix).Multiply(Convert.ToDouble(Evaluate(e.Right, context)));
                }
                else if (e.Right.Type == typeof(Matrix))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Right, context) as Matrix).Multiply(Convert.ToDouble(Evaluate(e.Left, context)));
                }
                else if (e.Left.Type == typeof(Vector) && Types.IsNumberType(e.Right.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Vector).Multiply(Types.ConvertValue<double>(Evaluate(e.Right, context)));
                }
                else if (e.Right.Type == typeof(Vector) && Types.IsNumberType(e.Left.Type))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Right, context) as Vector).Multiply(Types.ConvertValue<double>(Evaluate(e.Left, context))).GetNegative();
                }
            }
            else if (e.Operator == BinaryOperator.Divide)
            {
                if (e.Left.Type == typeof(Matrix) && e.Right.Type == typeof(Matrix))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Matrix).Divide((Evaluate(e.Right, context) as Matrix));
                }
                else if (e.Left.Type == typeof(Matrix))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Matrix).Multiply(1 / Convert.ToDouble(Evaluate(e.Right, context)));
                }
                else if (e.Right.Type == typeof(Matrix))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Right, context) as Matrix).ElementInvert().Multiply(Convert.ToDouble(Evaluate(e.Left, context)));
                }
                if (e.Left.Type == typeof(Vector))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Left, context) as Vector).Multiply(1 / Convert.ToDouble(Evaluate(e.Right, context)));
                }
                else if (e.Right.Type == typeof(Vector))
                {
                    isEvaluated = true;
                    return (Evaluate(e.Right, context) as Vector).ElementInvert().Multiply(Convert.ToDouble(Evaluate(e.Left, context)));
                }
            }


            return null;
        }

        private object EvalutePredefinedConstantExpression(PredefinedConstantExpression e, VariableContext context)
        {
            return e.Value;
        }
    }
}
