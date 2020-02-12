using Cherimoya.Evaluation;
using Cherimoya.Expressions;
using Mathy.DAL;
using Mathy.Repository.Repo;
using Mathy.Shared.Entity;
using Mathy.Utils.Dandelion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cherimoya.Language.JavaLike
{
    public abstract class JavaLikeExpressionEvaluator : ExpressionEvaluator
    {
        public override object Evaluate(Expression expression, VariableContext context)
        {
            if (expression is CastExpression)
            {
                return EvaluteCastExpression((CastExpression)expression, context);
            }
            else if (expression is CreateObjectExpression)
            {
                return EvaluteCreateObjectExpression((CreateObjectExpression)expression, context);
            }
            else if (expression is CreateArrayExpression)
            {
                return EvaluteCreateArrayExpression((CreateArrayExpression)expression, context);
            }
            else if (expression is BinaryExpression)
            {
                return EvaluteBinaryExpression((BinaryExpression)expression, context);
            }
            else if (expression is VariableExpression)
            {
                return EvaluateVariableExpression((VariableExpression)expression, context);
            }
            else if (expression is MethodCallExpression)
            {
                return EvaluteMethodCallExpression((MethodCallExpression)expression, context);
            }
            else if (expression is FunctionCallExpression)
            {
                return EvaluteFunctionCallExpression((FunctionCallExpression)expression, context);
            }
            else if (expression is IndexingExpression)
            {
                return EvaluteIndexingExpression((IndexingExpression)expression, context);
            }
            else if (expression is FieldExpression)
            {
                return EvaluteFieldExpression((FieldExpression)expression, context);
            }
            else if (expression is LambdaExpression)
            {
                return expression;
            }
            else if (expression is ConstantExpression)
            {
                return EvaluateConstantExpression((ConstantExpression)expression);
            }
            else if (expression is UnaryExpression)
            {
                return EvaluateUnaryExpression((UnaryExpression)expression, context);
            }
            else if (expression is IfElseExpression)
            {
                return EvaluateIfElseExpression((IfElseExpression)expression, context);
            }


            return null;
        }

        private object EvaluateConstantExpression(ConstantExpression expression)
        {
            return expression.Value;
        }

        private object EvaluateUnaryExpression(UnaryExpression expression, VariableContext context)
        {

            UnaryOperator op = expression.Operator;

            object operand = Evaluate(expression.Operand, context);

            if (op == UnaryOperator.Negation)
            {

                int typeIndex = Types.GetPrimitiveTypeIndex(operand.GetType());

                if (typeIndex == 0)
                {
                    return -(double)operand;
                }
                else if (typeIndex == 1)
                {
                    return -(float)operand;
                }
                else if (typeIndex == 2)
                {
                    return -(long)operand;
                }
                else if (typeIndex == 3)
                {
                    return -(int)operand;
                }
                else if (typeIndex == 4)
                {
                    return -(short)operand;
                }
                else if (typeIndex == 6)
                {
                    return -(byte)operand;
                }
            }
            else if (op == UnaryOperator.Identity)
            {
                return operand;
            }

            return null;
        }

        private object EvaluteBinaryExpression(BinaryExpression expression, VariableContext context)
        {

            BinaryOperator op = expression.Operator;

            if (op == BinaryOperator.Assign)
            {
                string variableName = ((VariableExpression)expression.Left).VariableName;
                object right1 = Evaluate(expression.Right, context);
                context.Set(variableName, right1);
                return right1;
            }


            object left = Evaluate(expression.Left, context);

            if (op == BinaryOperator.And)
            {
                return !(bool)left ? false : (bool)Evaluate(expression.Right, context);
            }
            else if (op == BinaryOperator.Or)
            {
                return (bool)left ? true : (bool)Evaluate(expression.Right, context);
            }


            object right = Evaluate(expression.Right, context);

            if (op == BinaryOperator.Add)
            {
                return Add(left, right);
            }
            else if (op == BinaryOperator.Subtract)
            {
                return Subtract(left, right);
            }
            else if (op == BinaryOperator.Multiply)
            {
                return Multiply(left, right);
            }
            else if (op == BinaryOperator.Divide)
            {
                return Divide(left, right);
            }
            else if (op == BinaryOperator.LessThan)
            {
                return LessThan(left, right);
            }
            else if (op == BinaryOperator.LessEqualThan)
            {
                return LessEqualThan(left, right);
            }
            else if (op == BinaryOperator.GreaterThan)
            {
                return GreaterThan(left, right);
            }
            else if (op == BinaryOperator.GreaterEqualThan)
            {
                return GreaterEqualThan(left, right);
            }
            else if (op == BinaryOperator.Equal)
            {
                return Equal(left, right);
            }
            else if (op == BinaryOperator.NotEqual)
            {
                return NotEqual(left, right);
            }
            return null;
        }

        private object EvaluateVariableExpression(VariableExpression expression, VariableContext context)
        {
            return context.GetValue(expression.VariableName);
        }

        private object EvaluteMethodCallExpression(MethodCallExpression expression, VariableContext context)
        {

            List<object> parameters = new List<object>();

            for (int i = 0; i <= expression.Parameters.Length - 1; i++)
            {
                object p = Evaluate(expression.Parameters[i], context);

                if (p is LambdaExpression)
                {
                    p = CreateFunctionalInterface((LambdaExpression)p, expression.Method.GetParameters()[i + (expression.IsExtension ? 1 : 0)].ParameterType);
                }

                parameters.Add(p);
            }


            if (expression.IsStatic)
            {
                return expression.Method.Invoke(null, parameters.ToArray());
            }


            object operand = Evaluate(expression.Operand, context);


            if (!expression.IsExtension)
            {
                return expression.Method.Invoke(operand, parameters.ToArray());
            }
            else
            {
                parameters.Insert(0, operand);
                return expression.Method.Invoke(null, parameters.ToArray());
            }
        }

        private object EvaluteFunctionCallExpression(FunctionCallExpression expression, VariableContext context)
        {
            List<object> parameters = new List<object>();

            for (int i = 0; i <= expression.Parameters.Length - 1; i++)
            {
                object p = Evaluate(expression.Parameters[i], context);

                parameters.Add(p);
            }

            if (expression.Method.GetParameters().Length > 0 && expression.Method.GetParameters().Last().ParameterType == typeof(VariableContext))
            {
                parameters.Add(context);
            }

            if (expression.MethodName == "Loop" || expression.MethodName.Contains("MCM") || expression.MethodName.Contains("Draw_"))
            {
                parameters.Add(context.GetValue("_EvaluationContext"));
            }

            if (expression.IsCustomFunc)
            {
                List<CoefficientDetail> data;
                if (context.HasVariable(expression.MethodName))
                {
                    data = (List<CoefficientDetail>)context.GetValue(expression.MethodName);
                }
                else
                {
                    var dal = new CoefficientRepo();
                    data = dal.GetCoefficientDetail(expression.MethodName);
                }
                var colName = Evaluate(expression.Parameters[0], context).ToString();
                var rowIndex = (int)Evaluate(expression.Parameters[1], context);
                return data.FirstOrDefault(m => m.CoefficientDetailRow == rowIndex && m.CoefficientDetailName == colName).CoefficientDetailValue;
            }

            try
            {
                //可变参数处理
                if (expression.Parameters.Count() > 1 && parameters[0].GetType() != typeof(double[]))
                {
                    var p = new List<object>();
                    var o = new List<double[]>();
                    foreach (var n in parameters)
                    {
                        if (n.GetType() != typeof(double[]))
                        {
                            p.Add(n);
                        }
                        else
                        {
                            o.Add(n as double[]);
                        }
                    }
                    if (o.Count > 0)
                    {
                        p.Add(o.ToArray());
                        parameters = p;
                    }
                }
                return expression.Method.Invoke(this, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(expression.MethodName + "\r\n" + (ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
        }


        private object CreateFunctionalInterface(LambdaExpression expression, Type targetType)
        {

            /*
            if (targetType.equals(Predicate.class)) {
                return new Predicate<object>() {
				
                    private VariableContext context = new VariableContext();

                    @Override
                    public bool test(object value) {
                        context.set(expression.getVariableNames()[0], value);
                        try {
                            return (bool)Evaluate(expression.getBody(), context);
                        } catch (Exception e) {
                            return false;
                        }
                    }
                };
            }
            else if (targetType.equals(IntPredicate.class)) {
                return new IntPredicate() {
				
                    private VariableContext context = new VariableContext();

                    @Override
                    public bool test(int value) {
                        context.set(expression.getVariableNames()[0], value);
                        try {
                            return (bool)Evaluate(expression.getBody(), context);
                        } catch (Exception e) {
                            return false;
                        }
                    }
                };
            }
            */

            return null;
        }


        private object EvaluateIfElseExpression(IfElseExpression expression, VariableContext context)
        {
            return (bool)Evaluate(expression.Condition, context) ?
                    Evaluate(expression.PositiveBranch, context) :
                    Evaluate(expression.NegativeBranch, context);
        }

        private object EvaluteCastExpression(CastExpression expression, VariableContext context)
        {
            object value = Evaluate(expression.Operand, context);


            return Types.CoerceValue(value, expression.TargetType);
        }

        private object EvaluteCreateObjectExpression(CreateObjectExpression expression, VariableContext context)
        {

            object[] parameters = new object[expression.Parameters.Length];
            Type[] parameterClasses = new Type[expression.Parameters.Length];
            for (int i = 0; i <= expression.Parameters.Length - 1; i++)
            {
                parameters[i] = Evaluate(expression.Parameters[i], context);
                parameterClasses[i] = parameters[i].GetType();
            }

            return expression.Type
                    .GetConstructor(parameterClasses)
                    .Invoke(parameters);
        }

        private object EvaluteCreateArrayExpression(CreateArrayExpression expression, VariableContext context)
        {

            Type c = expression.ElementType;

            List<object> items = new List<object>();

            for (int i = 0; i <= expression.Elements.Length - 1; i++)
            {
                if (!(expression.Elements[i] is CondensedArrayExpression))
                {
                    items.Add(Evaluate(expression.Elements[i], context));
                }
                else
                {

                    CondensedArrayExpression condensed = (CondensedArrayExpression)expression.Elements[i];
                    object from = Evaluate(condensed.From, context);
                    object to = Evaluate(condensed.To, context);
                    object by = Evaluate(condensed.By, context);


                    AddElements(from, to, by, Types.GetPrimitiveTypeIndex(c), items);
                }
            }


            return items.ToArray();
        }

        private void AddElements(object from, object to, object by, int type, List<object> items)
        {

            try
            {
                object f = Types.CoerceValue(from, type);
                object t = Types.CoerceValue(to, type);
                object b = Types.CoerceValue(by, type);

                if (type == 0)
                {
                    for (double i = (double)f; i <= (double)t; i += (double)b)
                    {
                        items.Add(i);
                    }
                }
                else if (type == 1)
                {
                    for (float i = (float)f; i <= (float)t; i += (float)b)
                    {
                        items.Add(i);
                    }
                }
                else if (type == 2)
                {
                    for (long i = (long)f; i <= (long)t; i += (long)b)
                    {
                        items.Add(i);
                    }
                }
                else if (type == 3)
                {
                    for (int i = (int)f; i <= (int)t; i += (int)b)
                    {
                        items.Add(i);
                    }
                }
                else if (type == 4)
                {
                    for (short i = (short)f; i <= (short)t; i += (short)b)
                    {
                        items.Add(i);
                    }
                }
                else if (type == 5)
                {
                    for (char i = (char)f; i <= (char)t; i += (char)b)
                    {
                        items.Add(i);
                    }
                }
                else if (type == 6)
                {
                    for (byte i = (byte)f; i <= (byte)t; i += (byte)b)
                    {
                        items.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private object EvaluteIndexingExpression(IndexingExpression expression, VariableContext context)
        {
            object instance = Evaluate(expression.Operand, context);
            int index = (int)Evaluate(expression.Indexer, context);
            return ((object[])instance)[index];
        }

        private object EvaluteFieldExpression(FieldExpression expression, VariableContext context)
        {
            object instance = Evaluate(expression.Operand, context);
            try
            {
                if (expression.Field != null)
                {
                    return expression.Field.GetValue(instance);
                }
                else
                {
                    return expression.Property.GetValue(instance);
                }
            }
            catch
            {
                return null;
            }
        }


        private object Add(object left, object right)
        {
            if (left.GetType() == typeof(string))
            {
                return (string)left + right;
            }
            else if (right.GetType() == typeof(string))
            {
                return left + (string)right;
            }
            else if (left.GetType() == typeof(char))
            {
                return Types.ConvertValue<string>((char)left) + right;
            }
            else if (right.GetType() == typeof(char))
            {
                return left + Types.ConvertValue<string>((char)right);
            }
            else if (Types.IsPrimitiveType(left.GetType()) && Types.IsPrimitiveType(right.GetType()))
            {
                int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());
                return MathSnippets.Add(
                       Types.CoerceValue(left, index),
                       Types.CoerceValue(right, index),
                       index);
            }


            return null;
        }

        private object Subtract(object left, object right)
        {
            int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

            return MathSnippets.Subtract(
                   Types.CoerceValue(left, index),
                   Types.CoerceValue(right, index),
                   index);
        }

        private object Multiply(object left, object right)
        {
            int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

            return MathSnippets.Multiply(
                   Types.CoerceValue(left, index),
                   Types.CoerceValue(right, index),
                   index);
        }

        private object Divide(object left, object right)
        {
            int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

            return MathSnippets.Divide(
                   Types.CoerceValue(left, index),
                   Types.CoerceValue(right, index),
                   index);
        }

        private object LessThan(object left, object right)
        {
            if (left == null && right != null && right.GetType() == typeof(string))
            {
                return true;
            }
            else if (left != null && left.GetType() == typeof(string) && right == null)
            {
                return false;
            }
            else if (left.GetType() == typeof(string) && right.GetType() == typeof(string))
            {
                return ((string)left).CompareTo((string)right) < 0;
            }
            else
            {
                int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

                return MathSnippets.LessThan(
                       Types.CoerceValue(left, index),
                       Types.CoerceValue(right, index),
                       index);
            }
        }

        private object LessEqualThan(object left, object right)
        {
            if (left == null && right != null && right.GetType() == typeof(string))
            {
                return true;
            }
            else if (left != null && left.GetType() == typeof(string) && right == null)
            {
                return false;
            }
            else if (left.GetType() == typeof(string) && right.GetType() == typeof(string))
            {
                return ((string)left).CompareTo((string)right) <= 0;
            }
            else
            {
                int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

                return MathSnippets.LessEqualThan(
                       Types.CoerceValue(left, index),
                       Types.CoerceValue(right, index),
                       index);
            }
        }

        private object GreaterThan(object left, object right)
        {
            if (left == null && right != null && right.GetType() == typeof(string))
            {
                return true;
            }
            else if (left != null && left.GetType() == typeof(string) && right == null)
            {
                return false;
            }
            else if (left.GetType() == typeof(string) && right.GetType() == typeof(string))
            {
                return ((string)left).CompareTo((string)right) > 0;
            }
            else
            {
                int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

                return MathSnippets.GreaterThan(
                       Types.CoerceValue(left, index),
                       Types.CoerceValue(right, index),
                       index);
            }
        }

        private object GreaterEqualThan(object left, object right)
        {
            if (left == null && right != null && right.GetType() == typeof(string))
            {
                return true;
            }
            else if (left != null && left.GetType() == typeof(string) && right == null)
            {
                return false;
            }
            else if (left.GetType() == typeof(string) && right.GetType() == typeof(string))
            {
                return ((string)left).CompareTo((string)right) >= 0;
            }
            else
            {
                int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

                return MathSnippets.GreaterEqualThan(
                       Types.CoerceValue(left, index),
                       Types.CoerceValue(right, index),
                       index);
            }
        }

        private object Equal(object left, object right)
        {
            if (left == null && right != null && right.GetType() == typeof(string))
            {
                return true;
            }
            else if (left != null && left.GetType() == typeof(string) && right == null)
            {
                return false;
            }
            else if (left.GetType() == typeof(string) && right.GetType() == typeof(string))
            {
                return ((string)left).CompareTo((string)right) == 0;
            }
            else
            {
                int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

                return MathSnippets.Equal(
                       Types.CoerceValue(left, index),
                       Types.CoerceValue(right, index),
                       index);
            }
        }

        private object NotEqual(object left, object right)
        {
            if (left == null && right != null && right.GetType() == typeof(string))
            {
                return true;
            }
            else if (left != null && left.GetType() == typeof(string) && right == null)
            {
                return false;
            }
            else if (left.GetType() == typeof(string) && right.GetType() == typeof(string))
            {
                return ((string)left).CompareTo((string)right) != 0;
            }
            else
            {
                int index = Types.ResolvePrimitiveTypeIndex(left.GetType(), right.GetType());

                return MathSnippets.NotEqual(
                       Types.CoerceValue(left, index),
                       Types.CoerceValue(right, index),
                       index);
            }
        }
    }
}