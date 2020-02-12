using Mathy.Utils.Dandelion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cherimoya.Expressions
{
    public class ReflectionSnippets
    {
        class MethodInfoComparer : IComparer<MethodInfo>
        {
            public int Compare(MethodInfo x, MethodInfo y)
            {
                if (x.ReturnType == y.ReturnType)
                {
                    return 0;
                }
                else if (y.ReturnType.IsAssignableFrom(x.ReturnType))
                {
                    return -1;
                }
                else if (y.ReturnType.IsAssignableFrom(x.ReturnType))
                {
                    return 1;
                }
                else
                {
                    return y.ReturnType.Name.CompareTo(y.ReturnType.Name);
                }
            }
        }

        public static bool CanCast(Type to, Type from)
        {
            if (Types.IsNumberType(to) && Types.IsNumberType(from))
            {
                return true;
            }
            else
            {
                return Accepts(to, from);
            }
        }

        public static bool Accepts(Type to, Type from)
        {
            if (Types.IsNumberType(to) && Types.IsNumberType(from))
            {
                return Types.GetPrimitiveTypeIndex(to) <= Types.GetPrimitiveTypeIndex(from);
            }
            else if (to.IsAssignableFrom(from))
            {
                return true;
            }
            else
            {
                // return from == typeof(LambdaExpression) && to.getAnnotation(FunctionalInterface.class) != null;

                return false;
            }
        }

        public static MethodInfo FindMethod(Type clazz, string methodName, Type[] parameterTypes)
        {
            foreach (MethodInfo method in GetSortedMethods(clazz))
            {
                ParameterInfo[] parameters = method.GetParameters();


                int parameterCount = parameters.Length > 0 && parameters.Last().ParameterType == typeof(VariableContext) ? parameters.Length - 1 : parameters.Length;
                if (method.Name != methodName)
                {
                    continue;
                }

                if (parameterCount != parameterTypes.Length)
                {
                    if (!parameters.Any(m => m.CustomAttributes.Any(n => n.AttributeType == typeof(System.ParamArrayAttribute))))
                        continue;
                }

                bool found = true;
                int i = 0;
                foreach (ParameterInfo parameter in method.GetParameters().Where(j => j.ParameterType != typeof(VariableContext)))
                {
                    if (parameter.CustomAttributes.Any(m => m.AttributeType == typeof(System.ParamArrayAttribute)))
                    {
                        var tp = GetArrayElementType(parameter.ParameterType);
                        var restparameterTypes = parameterTypes.Skip(i).Take(parameterTypes.Count() - i).ToList();
                        if (restparameterTypes.TrueForAll(m => Accepts(tp, m)))
                        {
                            break;
                        }
                    }
                    bool match = Accepts(parameter.ParameterType, parameterTypes[i]);

                    if (!match)
                    {
                        found = false;
                        break;
                    }
                    i++;
                }

                if (found)
                {
                    return method;
                }
            }

            return null;
        }

        private static Type GetArrayElementType(Type t)
        {
            if (!t.IsArray) return null;
            string tName;
            if (t.FullName.Contains("[][]"))
            {
                tName = t.FullName.Replace("[][]", "[]");
            }
            else
            {
                tName = t.FullName.Replace("[]", string.Empty);
            }
            Type elType = t.Assembly.GetType(tName);
            return elType;
        }


        private static List<MethodInfo> GetSortedMethods(Type type)
        {
            List<MethodInfo> methods = type.GetMethods().ToList();
            methods.Sort(new MethodInfoComparer());
            return methods;
        }
    }
}