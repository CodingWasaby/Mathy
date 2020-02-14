using System;
using System.Collections.Generic;

namespace Mathy.Language
{
    class MathyConstants
    {
        class MathSymbol
        {
            public string Symbol { get; set; }

            public object Value { get; set; }
        }

        private static Dictionary<string, MathSymbol> values = new Dictionary<string, MathSymbol>();

        static MathyConstants()
        {
            AddConstant("pi", Math.PI, "\u03c0");
            AddConstant("posinf", double.PositiveInfinity, "\u221e");
            AddConstant("neginf", double.NegativeInfinity, "-\u221e");
            AddConstant("e", Math.E, "e");
        }

        private static void AddConstant(string name, object value, string symbol)
        {
            values.Add(name, new MathSymbol() { Value = value, Symbol = symbol });
        }


        public static object GetValue(string name)
        {
            return values.ContainsKey(name) ? values[name].Value : null;
        }

        public static string GetSymbol(string name)
        {
            return values.ContainsKey(name) ? values[name].Symbol : null;
        }
    }
}
