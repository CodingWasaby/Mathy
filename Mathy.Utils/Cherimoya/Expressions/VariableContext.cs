using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cherimoya.Expressions
{
    public class VariableContext
    {
        private Dictionary<string, object> variables = new Dictionary<string, object>();


        public VariableContext()
        {
        }

        public VariableContext(Variable[] variables)
        {
            foreach (Variable variable in variables)
            {
                Set(variable.Name, variable.Value);
            }
        }


        private List<Type> methodExtenders = new List<Type>();
        public Type[] GetMethodExtenders()
        {
            return methodExtenders.ToArray();
        }

        public void Set(string variableName, object value)
        {
            if (variables.ContainsKey(variableName))
            {
                variables[variableName] = value;
            }
            else
            {
                variables.Add(variableName, value);
            }
        }

        public bool HasVariable(string variableName)
        {
            return variables.ContainsKey(variableName);
        }

        public object GetValue(string variableName)
        {
            return variables[variableName];
        }

        public string[] GetAllVariables()
        {
            return variables.Keys.ToArray();
        }

        public Type GetType(string variableName)
        {
            return HasVariable(variableName) ? GetValue(variableName).GetType() : null;
        }

        public void AddMethodExtender(Type methodExtenderClass)
        {
            methodExtenders.Add(methodExtenderClass);
        }

        public MethodInfo SearchMethod(string methodName, Type instanceClass, Type[] parameterClasses)
        {

            List<Type> classes = new List<Type>();

            if (instanceClass != null)
            {
                classes.Add(instanceClass);
            }

            foreach (Type clazz in parameterClasses)
            {
                classes.Add(clazz);
            }


            Type[] array = classes.ToArray();


            MethodInfo method = SearchMethod(methodName, array);
            if (method != null)
            {
                return method;
            }

            array[0] = typeof(object);
            method = SearchMethod(methodName, array);
            if (method != null)
            {
                return method;
            }


            return null;
        }



        private MethodInfo SearchMethod(string methodName, Type[] parameterClasses)
        {
            foreach (Type extenderClass in methodExtenders)
            {
                MethodInfo method = ReflectionSnippets.FindMethod(extenderClass, methodName, parameterClasses);
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        public void ForTest()
        {

        }

        public void Remove(string variableName)
        {
            variables.Remove(variableName);
        }
    }
}