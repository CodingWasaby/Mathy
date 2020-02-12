using System;
using System.Reflection;

namespace Cherimoya.Expressions
{
    public class FieldExpression : Expression
    {
        public FieldExpression(Expression operand, Type ownerType, string fieldName, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            FieldName = fieldName;
            Operand = operand;
            OwnerType = ownerType;
            IsStatic = ownerType != null;
        }


        public string FieldName { get; private set; }

        public Type OwnerType { get; private set; }

        public Expression Operand { get; private set; }

        public FieldInfo Field { get; set; }

        public PropertyInfo Property { get; set; }

        public bool IsStatic { get; private set; }


        public override bool IsConstantExpression()
        {
            return Operand.IsConstantExpression();
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            yield return Operand;
        }
    }
}