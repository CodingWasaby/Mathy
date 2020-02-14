using Cherimoya.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Language
{
    public class DictionaryExpression : Expression
    {
        public DictionaryExpression(Dictionary<string, Expression> dictionary, int fromPosition, int toPosition)
            : base(fromPosition, toPosition)
        {
            Dictionary = dictionary;
        }

        public Dictionary<string, Expression> Dictionary { get; private set; }

        public override bool IsConstantExpression()
        {
            return Dictionary.Values.All(i => i.IsConstantExpression());
        }

        public override IEnumerable<Expression> GetChildren()
        {
            foreach (Expression value in Dictionary.Values)
            {
                yield return value;
            }
        }
    }
}