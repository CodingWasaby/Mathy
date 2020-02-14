using System.Collections.Generic;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class ConstantExpression : Expression
    {
        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                this.value = CreateMultiLineText(value);
            }
        }

        private static string CreateMultiLineText(string text)
        {
            List<string> lines = new List<string>();

            int pos = 0;

            while (text.Length - pos > 40)
            {
                lines.Add(text.Substring(pos, 40));
                pos += 40;
            }

            lines.Add(text.Substring(pos));

            return string.Join("\r\n", lines.ToArray());
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            SizeF size = g.MeasureString(Value, font);

            SetDesiredSize(GetChildSize(widthSpec, size.Width), GetChildSize(heightSpec, size.Height));
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            g.DrawString(Value, font, Brushes.Black, new PointF(Left, Top));
        }
    }
}
