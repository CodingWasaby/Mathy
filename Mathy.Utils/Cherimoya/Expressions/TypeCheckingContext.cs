using System;
using System.Collections.Generic;
using System.Linq;

namespace Cherimoya.Expressions
{
    public class TypeCheckingContext
    {
        public bool CreateVariableOnAssign { get; set; }

        public Type RightOperandType { get; set; }

        public VariableContext VariableContext { get; set; }

        public LambdaContext LambdaContext { get; set; }

        public CompileErrorProvider ErrorProvider { get; set; }


        public object CreateAutoCreatedVariableValue(Type type)
        {
            if (type == typeof(double))
            {
                return (double)0;
            }
            else if (type == typeof(float))
            {
                return (float)0;
            }
            else if (type == typeof(long))
            {
                return (long)0;
            }
            else if (type == typeof(int))
            {
                return (int)0;
            }
            else if (type == typeof(short))
            {
                return (short)0;
            }
            else if (type == typeof(char))
            {
                return (char)0;
            }
            else if (type == typeof(bool))
            {
                return false;
            }
            else if (type == typeof(DateTime))
            {
                return new DateTime();
            }
            else if (type == typeof(object[]))
            {
                return new object[] { };
            }
            else if (type == typeof(int[]))
            {
                return new int[] { };
            }
            else if (type == typeof(double[]))
            {
                return new double[] { };
            }
            else if (type == typeof(byte[]))
            {
                return new byte[] { };
            }
            else if (type == typeof(string))
            {
                return "";
            }
            else if (type.Name.Contains("Tuple"))
            {
                return Activator.CreateInstance(type, type.GenericTypeArguments.Select(i => CreateAutoCreatedVariableValue(i)).ToArray());
            }
            else if (values.ContainsKey(type))
            {
                return values[type];
            }


            return null;
        }


        private Dictionary<Type, object> values = new Dictionary<Type, object>();

        public void AddAutoCreatedValue(Type type, object value)
        {
            values.Add(type, value);
        }
    }
}