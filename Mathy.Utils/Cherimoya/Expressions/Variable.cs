using System;

namespace Cherimoya.Expressions
{
    public class Variable
    {
        Variable()
        {
        }

        public static Variable Of(String name, Object value)
        {
            Variable instance = new Variable();
            instance.Name = name;
            instance.Value = value;
            return instance;
        }

        public string Name { get; private set; }

        public object Value { get; private set; }
    }
}
