using Cherimoya.Evaluation;
using Cherimoya.Expressions;
using Mathy.Utils.Dandelion;
using Mathy.DAL;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cherimoya.Language.JavaLike
{
    public abstract class JavaLikeTypeChecker : TypeChecker
    {
        public override void PerformTypeChecking(Expression expression, TypeCheckingContext context)
        {
            if (expression is CastExpression)
            {
                CheckCastExpression((CastExpression)expression, context);
            }
            else if (expression is CreateObjectExpression)
            {
                CehckCreateObjectExpression((CreateObjectExpression)expression, context);
            }
            else if (expression is CreateArrayExpression)
            {
                CheckCreateArrayExpression((CreateArrayExpression)expression, context);
            }
            else if (expression is BinaryExpression)
            {
                CheckBinaryExpression((BinaryExpression)expression, context);
            }
            else if (expression is VariableExpression)
            {
                CheckVariableExpression((VariableExpression)expression, context);
            }
            else if (expression is MethodCallExpression)
            {
                CheckMethodCallExpression((MethodCallExpression)expression, context);
            }
            else if (expression is FunctionCallExpression)
            {
                CheckFunctionCallExpression((FunctionCallExpression)expression, context);
            }
            else if (expression is IndexingExpression)
            {
                CheckIndexingExpression((IndexingExpression)expression, context);
            }
            else if (expression is FieldExpression)
            {
                CheckFieldExpression((FieldExpression)expression, context);
            }
            else if (expression is LambdaExpression)
            {
                CheckLambdaExpression((LambdaExpression)expression, context);
            }
            else if (expression is ConstantExpression)
            {
                CheckConstantExpression((ConstantExpression)expression);
            }
            else if (expression is UnaryExpression)
            {
                CheckUnaryExpression((UnaryExpression)expression, context);
            }
            else if (expression is IfElseExpression)
            {
                CheckIfElseExpression((IfElseExpression)expression, context);
            }
        }


        private void CheckCastExpression(CastExpression e, TypeCheckingContext context)
        {
            try
            {
                e.TargetType = Types.GetType(e.ClassName, LanguageService.GetImportedPackages());
            }
            catch
            {
                context.ErrorProvider.ThrowException(string.Format("{0} not defined.", e.ClassName), e);
            }


            PerformTypeChecking(e.Operand, context);

            if (e.Operand.Type == null ? !Types.IsPrimitiveType(e.TargetType) : ReflectionSnippets.CanCast(e.TargetType, e.Operand.Type))
            {
                e.Type = e.TargetType;
            }
            else
            {
                context.ErrorProvider.ThrowException(string.Format("Cannot cast from {0} to {1}.", e.Operand.Type, e.TargetType), e);
            }
        }

        private void CehckCreateObjectExpression(CreateObjectExpression e, TypeCheckingContext context)
        {
            foreach (Expression parameter in e.Parameters)
            {
                PerformTypeChecking(parameter, context);
            }

            foreach (ConstructorInfo c in e.ObjectType.GetConstructors())
            {
                bool isValid = true;

                int i = 0;
                foreach (ParameterInfo p in c.GetParameters())
                {
                    if (!ReflectionSnippets.Accepts(p.ParameterType, e.Parameters[i].Type))
                    {
                        isValid = false;
                        break;
                    }
                    i++;
                }

                if (isValid)
                {
                    e.Constructor = c;
                    break;
                }
            }

            if (e.Constructor == null)
            {
                StringBuilder b = new StringBuilder();
                foreach (Expression parameter in e.Parameters)
                {
                    b.Append(parameter.Type).Append(",");
                }

                if (e.Parameters.Length > 0)
                {
                    b.Remove(b.Length - 1, 1);
                }

                context.ErrorProvider.ThrowException(string.Format("{0}.ctor({1}) not found", e.ObjectType, b), e);
            }


            e.Type = e.ObjectType;
        }

        private void CheckCreateArrayExpression(CreateArrayExpression e, TypeCheckingContext context)
        {
            foreach (Expression element in e.Elements)
            {
                PerformTypeChecking(element, context);

                Type type = element is CondensedArrayExpression ? element.Type.GetElementType() : element.Type;

                if (!ReflectionSnippets.Accepts(e.ElementType, type))
                {
                    context.ErrorProvider.ThrowException(string.Format("{0} cannot be put into {1}[]", element.Type, e.ElementType), element);
                }
            }

            e.Type = e.ElementType.MakeArrayType(0);
        }

        private void CheckBinaryExpression(BinaryExpression e, TypeCheckingContext context)
        {
            PerformTypeChecking(e.Right, context);

            if (context.CreateVariableOnAssign && e.Operator == BinaryOperator.Assign)
            {
                context.RightOperandType = e.Right.Type;
            }
            else
            {
                context.RightOperandType = null;
            }

            PerformTypeChecking(e.Left, context);
            context.RightOperandType = null;


            CheckBinaryExpresionType(e, context);
        }

        protected virtual void CheckBinaryExpresionType(BinaryExpression e, TypeCheckingContext context)
        {
            Type type = null;

            bool isValid = true;

            if (e.Left.Type == null && e.Right.Type == null)
            {
                isValid = false;
            }
            else if (e.Operator == BinaryOperator.Assign)
            {
                if (!(e.Left is VariableExpression || e.Left is IndexingExpression))
                {
                    context.ErrorProvider.ThrowException("Can only assign to a variable.", e);
                    return;
                }
            }
            else if (e.Operator == BinaryOperator.Add)
            {
                if (e.Left.Type == typeof(char) || e.Right.Type == typeof(char))
                {
                    type = typeof(string);
                }
                else if (e.Left.Type == typeof(string) || e.Right.Type == typeof(string))
                {
                    type = typeof(string);
                }
                else if (Types.IsNumberType(e.Left.Type) && Types.IsNumberType(e.Right.Type))
                {
                    int index = Types.ResolvePrimitiveTypeIndex(e.Left.Type, e.Right.Type);
                    type = Types.GetPrimitiveType(index);
                }
                else
                {
                    isValid = false;
                }
            }
            else if (e.Operator == BinaryOperator.Subtract ||
                    e.Operator == BinaryOperator.Multiply ||
                    e.Operator == BinaryOperator.Divide)
            {
                if (Types.IsNumberType(e.Left.Type) && Types.IsNumberType(e.Right.Type))
                {
                    int index = Types.ResolvePrimitiveTypeIndex(e.Left.Type, e.Right.Type);
                    type = Types.GetPrimitiveType(index);
                }
                else if (e.Left.Type.Name == "Matrix" && e.Right.Type.Name == "Matrix")
                {
                    type = e.Left.Type;
                }
                else
                {
                    isValid = false;
                }
            }
            else if (e.Operator == BinaryOperator.LessThan ||
                    e.Operator == BinaryOperator.LessEqualThan ||
                    e.Operator == BinaryOperator.GreaterThan ||
                    e.Operator == BinaryOperator.GreaterEqualThan)
            {
                if (e.Left.Type == typeof(string) && e.Right.Type == typeof(string))
                {
                    type = typeof(bool);
                }
                else if (Types.IsNumberType(e.Left.Type) && Types.IsNumberType(e.Right.Type))
                {
                    type = typeof(bool);
                }
                else
                {
                    isValid = false;
                }
            }
            else if (e.Operator == BinaryOperator.Equal ||
                    e.Operator == BinaryOperator.NotEqual)
            {
                if (e.Left.Type == typeof(string) && e.Right.Type == typeof(string))
                {
                    type = typeof(bool);
                }
                else if (Types.IsNumberType(e.Left.Type) && Types.IsNumberType(e.Right.Type))
                {
                    type = typeof(bool);
                }
                else if (Types.GetPrimitiveTypeIndex(e.Left.Type) == 7 && Types.GetPrimitiveTypeIndex(e.Right.Type) == 7)
                {
                    type = typeof(bool);
                }
                else if (e.Left is ConstantExpression &&
                        ((ConstantExpression)e.Left).Value == null &&
                        !Types.IsPrimitiveType(e.Right.Type))
                {
                    type = typeof(bool);
                }
                else if (e.Right is ConstantExpression &&
                        ((ConstantExpression)e.Right).Value == null &&
                        !Types.IsPrimitiveType(e.Left.Type))
                {
                    type = typeof(bool);
                }
                else
                {
                    isValid = false;
                }
            }
            else if (e.Operator == BinaryOperator.And ||
                    e.Operator == BinaryOperator.Or)
            {
                if (e.Left.Type == typeof(bool) || e.Right.Type == typeof(bool))
                {
                    type = typeof(bool);
                }
                else
                {
                    isValid = false;
                }
            }

            if (isValid)
            {
                e.Type = type;
            }
            else if (context != null)
            {
                context.ErrorProvider.ThrowException(string.Format("Operator {0} cannot be applied to types of {1} and {2}.", ExpressionSnippets.SerializeBinaryOperator(e.Operator), e.Left.Type, e.Right.Type), e);
            }
        }

        private void CheckVariableExpression(VariableExpression e, TypeCheckingContext context)
        {
            LambdaVariable variable = context.LambdaContext.GetVariable(e.VariableName);

            if (variable != null)
            {
                e.Type = context.LambdaContext.GetVariable(variable.Name).Type;
            }
            else if (context.VariableContext.HasVariable(e.VariableName))
            {
                e.Type = context.VariableContext.GetType(e.VariableName);
            }
            else if (context.CreateVariableOnAssign && context.RightOperandType != null)
            {
                context.VariableContext.Set(e.VariableName, context.CreateAutoCreatedVariableValue(context.RightOperandType));
                e.Type = context.VariableContext.GetType(e.VariableName);
            }
            else
            {
                context.ErrorProvider.ThrowException(string.Format("{0} not defined.", e.VariableName), e);
            }
        }

        private void CheckMethodCallExpression(MethodCallExpression e, TypeCheckingContext context)
        {
            for (int i = 0; i <= e.Parameters.Length - 1; i++)
            {
                PerformTypeChecking(e.Parameters[i], context);
            }

            if (e.IsStatic)
            {
                CheckStatic(e, context);
            }
            else
            {
                CheckNonStatic(e, context);
            }


            e.Type = e.Method.ReturnType;


            context.LambdaContext.PushCallerMethod(e);

            for (int i = 0; i <= e.Parameters.Length - 1; i++)
            {
                PerformTypeChecking(e.Parameters[i], context);
            }

            context.LambdaContext.PopCallerMethod();
        }

        private void CheckStatic(MethodCallExpression e, TypeCheckingContext context)
        {
            Type[] parameterClasses = e.Parameters.Select(i => i.Type).ToArray();


            e.Method = ReflectionSnippets.FindMethod(e.OwnerType, e.MethodName, parameterClasses);

            if (e.Method == null)
            {
                StringBuilder paramList = new StringBuilder();
                foreach (Type type in parameterClasses)
                {
                    paramList.Append(type).Append(",");
                }

                if (parameterClasses.Length > 0)
                {
                    paramList.Remove(paramList.Length - 1, 1);
                }


                StringBuilder locations = new StringBuilder();
                locations.Append(e.OwnerType).Append("\r\n");

                context.ErrorProvider.ThrowException(string.Format("Method {0}.{1}({2}) cannot be found.\r\nSearched the following location:\r\n{3}",
                        e.OwnerType,
                        e.MethodName,
                        paramList,
                        locations), e);
            }
        }

        private void CheckNonStatic(MethodCallExpression e, TypeCheckingContext context)
        {
            PerformTypeChecking(e.Operand, context);

            Type[] parameterClasses = e.Parameters.Select(i => i.Type).ToArray();


            e.Method = ReflectionSnippets.FindMethod(e.Operand.Type, e.MethodName, parameterClasses);

            if (e.Method == null && context.VariableContext != null)
            {
                e.Method = context.VariableContext.SearchMethod(e.MethodName, e.Operand.Type, parameterClasses);
                e.IsExtension = true;
            }
            if (e.Method == null)
            {
                StringBuilder list1 = new StringBuilder();
                foreach (Type type in parameterClasses)
                {
                    list1.Append(type).Append(",");
                }

                if (parameterClasses.Length > 0)
                {
                    list1.Remove(list1.Length - 1, 1);
                }


                StringBuilder list2 = new StringBuilder();
                list2.Append(e.Operand.Type).Append(",");
                foreach (Type type in parameterClasses)
                {
                    list2.Append(type).Append(",");
                }

                list2.Remove(list2.Length - 1, 1);


                StringBuilder locations = new StringBuilder();
                locations.Append(e.Operand.Type).Append("\r\n");
                foreach (Type type in context.VariableContext.GetMethodExtenders())
                {
                    locations.Append(type).Append("\r\n");
                }


                context.ErrorProvider.ThrowException(string.Format("Method {0}.{1}({2}) or extension method {3}({4}) cannot be found.\r\nSearched the following locations:\r\n{5}",
                        e.Operand.Type,
                        e.MethodName,
                        list1,
                        e.MethodName,
                        list2,
                        locations), e);
            }
        }

        private void CheckFunctionCallExpression(FunctionCallExpression e, TypeCheckingContext context)
        {
            for (int i = 0; i <= e.Parameters.Length - 1; i++)
            {
                PerformTypeChecking(e.Parameters[i], context);
            }

            Type[] parameterClasses = e.Parameters.Select(i => i.Type).ToArray();
            if (e.MethodName == "Loop" || e.MethodName.Contains("MCM") || e.MethodName.Contains("Draw_"))
            {
                var list = parameterClasses.ToList();
                list.Add(typeof(object));
                parameterClasses = list.ToArray();
            }
            e.Method = context.VariableContext.SearchMethod(e.MethodName, null, parameterClasses);



            if (e.Method == null)
            {
                var dal = new CoefficientDAL();
                var coeList = dal.GetCoefficientDetail(e.MethodName);
                if (coeList.Count > 0)
                {
                    context.VariableContext.Set(e.MethodName, coeList);
                    e.Method = this.GetType().GetMethods()[0]; //模拟一个方法
                    e.IsCustomFunc = true;
                }
                else
                {
                    e.IsCustomFunc = false;
                }
                if (!e.IsCustomFunc)
                {
                    StringBuilder list = new StringBuilder();
                    foreach (Type type in parameterClasses)
                    {
                        list.Append(type).Append(",");
                    }

                    list.Remove(list.Length - 1, 1);


                    StringBuilder locations = new StringBuilder();
                    foreach (Type type in context.VariableContext.GetMethodExtenders())
                    {
                        locations.Append(type).Append("\r\n");
                    }
                    context.ErrorProvider.ThrowException(string.Format("Method {0}({1}) cannot be found.\r\nSearched the following locations:\r\n{2}",
                            e.MethodName,
                            list,
                            locations), e);
                }
            }
            e.Type = e.Method.ReturnType;
            context.LambdaContext.PushCallerMethod(e);
            for (int i = 0; i <= e.Parameters.Length - 1; i++)
            {
                PerformTypeChecking(e.Parameters[i], context);
            }

            context.LambdaContext.PopCallerMethod();
        }


        private void CheckIndexingExpression(IndexingExpression e, TypeCheckingContext context)
        {
            PerformTypeChecking(e.Operand, context);
            PerformTypeChecking(e.Indexer, context);

            if (!e.Operand.Type.IsArray)
            {
                context.ErrorProvider.ThrowException("Only array types can use [] operator.", e);
            }
            else if (!ReflectionSnippets.Accepts(typeof(int), e.Indexer.Type))
            {
                context.ErrorProvider.ThrowException("Indexer must be convertiable to int.", e);
            }
            else
            {
                e.Type = e.Operand.Type.GetElementType();
            }
        }

        private void CheckFieldExpression(FieldExpression e, TypeCheckingContext context)
        {
            if (!e.IsStatic)
            {
                PerformTypeChecking(e.Operand, context);
            }


            e.Field = (e.IsStatic ? e.OwnerType : e.Operand.Type).GetField(e.FieldName);
            if (e.Field != null)
            {
                e.Type = e.Field.FieldType;
            }
            else
            {
                e.Property = (e.IsStatic ? e.OwnerType : e.Operand.Type).GetProperty(e.FieldName);
                if (e.Property != null)
                {
                    e.Type = e.Property.PropertyType;
                }
                else
                {
                    context.ErrorProvider.ThrowException(string.Format("Field or property {0}.{1} not found.", e.Operand.Type, e.FieldName), e);
                }
            }
        }

        private void CheckLambdaExpression(LambdaExpression e, TypeCheckingContext context)
        {
            e.Type = typeof(LambdaExpression);

            if (e.Level != context.LambdaContext.GetCallerStackSize())
            {
                return;
            }


            context.LambdaContext.PushVariables(e.VariableNames);

            e.VariableTypes = new Type[e.VariableNames.Length];
            int index = 0;
            foreach (string variableName in e.VariableNames)
            {
                LambdaVariable variable = context.LambdaContext.GetVariable(variableName);


                Expression lambdaOperand;
                string methodName;

                if (variable.CallerMethod is MethodCallExpression)
                {
                    MethodCallExpression mc = variable.CallerMethod as MethodCallExpression;
                    lambdaOperand = mc.IsExtension ? mc.Operand : mc.Parameters[0];

                    methodName = mc.MethodName;
                }
                else
                {
                    FunctionCallExpression fc = variable.CallerMethod as FunctionCallExpression;
                    lambdaOperand = fc.Parameters[0];

                    methodName = fc.MethodName;
                }


                Type type = ResolveLambdaParameterType(lambdaOperand.Type, index);

                if (type == null)
                {
                    context.ErrorProvider.ThrowException(string.Format("Cannot apply lambda parameter {0} to method {1}", variableName, methodName), e);
                }

                e.VariableTypes[index] = type;
                context.LambdaContext.GetVariable(variableName).Type = type;


                index++;
            }

            PerformTypeChecking(e.Body, context);

            context.LambdaContext.PopVariables(e.VariableNames.Length);
        }

        protected virtual Type ResolveLambdaParameterType(Type operandType, int index)
        {
            if (operandType.IsArray)
            {
                if (index == 0)
                {
                    return operandType.GetElementType();
                }
            }


            return null;
        }

        private void CheckConstantExpression(ConstantExpression e)
        {
            if (e.Value != null)
            {
                e.Type = e.Value.GetType();
            }
        }

        private void CheckUnaryExpression(UnaryExpression e, TypeCheckingContext context)
        {
            PerformTypeChecking(e.Operand, context);


            Type type = null;

            bool isValid = true;


            if (e.Operator == UnaryOperator.Negation)
            {
                if (Types.IsNumberType(e.Operand.Type))
                {
                    type = e.Operand.Type;
                }
                else
                {
                    isValid = false;
                }
            }
            else if (e.Operator == UnaryOperator.Not)
            {
                if (Types.GetPrimitiveTypeIndex(e.Operand.Type) == 7)
                {
                    type = typeof(bool);
                }
                else
                {
                    isValid = false;
                }
            }
            else if (e.Operator == UnaryOperator.Identity)
            {
                if (Types.IsNumberType(e.Operand.Type))
                {
                    type = e.Operand.Type;
                }
                else
                {
                    isValid = false;
                }
            }


            if (isValid)
            {
                e.Type = type;
            }
            else
            {
                context.ErrorProvider.ThrowException(string.Format("Operator {0} cannot be applied to type {1}.", ExpressionSnippets.SerializeUnaryOperator(e.Operator), e.Operand.Type), e);
            }
        }

        private void CheckIfElseExpression(IfElseExpression e, TypeCheckingContext context)
        {
            PerformTypeChecking(e.Condition, context);
            PerformTypeChecking(e.PositiveBranch, context);
            PerformTypeChecking(e.NegativeBranch, context);


            if (e.Condition.Type != typeof(bool))
            {
                context.ErrorProvider.ThrowException(string.Format("Condition cannot be {0}", e.Condition.GetType()), e);
            }


            Type positiveType = e.PositiveBranch.Type;
            Type negativeType = e.NegativeBranch.Type;

            if (positiveType == negativeType)
            {
                e.Type = positiveType;
            }
            else
            {
                context.ErrorProvider.ThrowException(string.Format("{0} and {1} are not the same type.", positiveType, negativeType), e);
            }
        }
    }
}
