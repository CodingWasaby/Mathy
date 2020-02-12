using Cherimoya.Lexicons;
using System;
using System.Collections.Generic;

namespace Cherimoya.Expressions
{
    class LexiconizationRuleTable
    {
        private static Dictionary<Type, LexiconizationRule> rules = new Dictionary<Type, LexiconizationRule>();

        public static LexiconizationRule GetRule(ExpressionCompiler compiler)
        {

            Type c = compiler.GetType();

            if (!rules.ContainsKey(c))
            {
                rules.Add(c, compiler.GetLexiconizationRule());
            }

            return rules[c];
        }
    }
}