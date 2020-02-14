using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class SuperscriptExpression : Expression
    {
        public Expression Body { get; set; }

        public Expression Superscript { get; set; }

        public bool HasBracket { get; set; }


        private int bracketSize = 5;


        public override void FindMaxTop(float[] height)
        {
            float h = Body.FindMaxTop() + Superscript.DesiredHeight - 5;

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = Body.FindMaxBottom();

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            bracketSize = HasBracket ? 5 : 0;

            Body.Measure(widthSpec, heightSpec, g, font, style);
            Superscript.Measure(widthSpec, heightSpec, g, font, style);

            SetDesiredSize(Body.DesiredWidth + Superscript.DesiredWidth + bracketSize * 2 + 1, Body.DesiredHeight + Superscript.DesiredHeight - 5);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            Body.Layout(this, bracketSize, 5, Body.DesiredWidth, Body.DesiredHeight, g, font, style);
            Superscript.Layout(this, Body.DesiredWidth + bracketSize * 2 + 1, 0, Body.DesiredWidth, Body.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            Body.Draw(g, font, style);
            Superscript.Draw(g, font, style);

            if (HasBracket)
            {
                VectorGraphics.DrawRoundBracket(new RectangleF(Left, Top, Width - Superscript.DesiredWidth, Height), g, Brushes.Black);
            }
        }
    }
}
