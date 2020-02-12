using Cherimoya.Expressions;
using Cherimoya.Language;
using Cherimoya.Reduction.Rules;
using System;
using System.Collections.Generic;

namespace Cherimoya.Reduction
{
    public class ExpressionReductor
    {
        public ExpressionReductor(LanguageService languageService)
        {
            InitRules();
            LanguageService = languageService;
        }

        private void InitRules()
        {
            rules.Add(new ReduceConstantExpression());
            rules.Add(new ReduceFunctionCallExpression());
            rules.Add(new PromoteLeftRight());
            rules.Add(new ReduceBinaryExpression());
        }


        private List<ReductionRule> rules = new List<ReductionRule>();


        public LanguageService LanguageService { get; private set; }

        public Expression Reduce(Expression root)
        {
            try
            {
                Expression current = root;

                foreach (ReductionRule rule in rules)
                {
                    current = rule.Reduce(current, this);
                }


                return current;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
