using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class BinaryOperatorNode : Node
    {
        public BinaryOperator Operator { get; set; }

        private static string GetOperatorString(BinaryOperator op)
        {
            if (op == BinaryOperator.Add)
            {
                return "+";
            }
            else if (op == BinaryOperator.Subtract)
            {
                return "-";
            }
            else if (op == BinaryOperator.Multiply)
            {
                return "×";
            }
            else if (op == BinaryOperator.Assign)
            {
                return ":=";
            }
            else if (op == BinaryOperator.LessThan)
            {
                return "<";
            }
            else if (op == BinaryOperator.LessEqualThan)
            {
                return "≤";
            }
            else if (op == BinaryOperator.GreaterThan)
            {
                return ">";
            }
            else if (op == BinaryOperator.GreaterEqualThan)
            {
                return "≥";
            }
            else if (op == BinaryOperator.Equal)
            {
                return "=";
            }
            else if (op == BinaryOperator.NotEqual)
            {
                return "≠";
            }


            return null;
        }

        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            if (Operator == BinaryOperator.None)
            {
                SetDesiredSize(0, 0);
            }
            else
            {
                SizeF size = g.MeasureString(GetOperatorString(Operator), font);

                SetDesiredSize(GetChildSize(widthSpec, size.Width), GetChildSize(heightSpec, size.Height));
            }
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            if (Operator == BinaryOperator.None)
            {
            }
            else if (Operator != BinaryOperator.Divide)
            {
                g.DrawString(GetOperatorString(Operator), font, Brushes.Black, new PointF(Left, Top));
            }
            else
            {
                Pen p = new Pen(Color.Black);
                g.DrawLine(p, Left + 3, Top + (Height - 1) / 2, Left + Width - 3, Top + (Height - 1) / 2);
                p.Dispose();
            }
        }
    }
}
