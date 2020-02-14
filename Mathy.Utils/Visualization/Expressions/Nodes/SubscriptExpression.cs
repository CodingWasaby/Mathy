using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class SubscriptExpression : Expression
    {
        public Expression Body { get; set; }

        public Expression Subscript { get; set; }

        public bool HasBracket { get; set; }

        public float ShiftOffset { get; set; }


        private int bracketSize;


        public override void FindMaxTop(float[] height)
        {
            float h = Body.FindMaxTop();

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = Body.FindMaxBottom() + Subscript.DesiredHeight - 10;

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            bracketSize = HasBracket ? 5 : 0;

            Body.Measure(widthSpec, heightSpec, g, font, style);
            Subscript.Measure(widthSpec, heightSpec, g, font, style);

            SetDesiredSize(Body.DesiredWidth + Subscript.DesiredWidth + bracketSize * 2 + 1 - ShiftOffset + 5, Body.DesiredHeight + Subscript.DesiredHeight - 10);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            Body.Layout(this, bracketSize, 0, Body.DesiredWidth, Body.DesiredHeight, g, font, style);
            Subscript.Layout(this, Body.DesiredWidth + bracketSize * 2 + 1 - ShiftOffset, DesiredHeight - Subscript.DesiredHeight, Body.DesiredWidth, Body.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            Body.Draw(g, font, style);
            Subscript.Draw(g, style.Font, style);

            if (HasBracket)
            {
                VectorGraphics.DrawRoundBracket(new RectangleF(Left, Top, Width - Subscript.DesiredWidth, Height), g, Brushes.Black);
            }
        }
    }
}
