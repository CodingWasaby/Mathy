using System;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class PowExpression : Expression
    {
        public Expression Pow { get; set; }

        public Expression X { get; set; }


        public override void FindMaxTop(float[] height)
        {
            float h = Math.Max(X.FindMaxTop(), Pow.DesiredHeight);

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            X.FindMaxBottom(height);
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            MeasureSpec leftWidth = null;
            MeasureSpec rightWidth = null;


            if (widthSpec.Mode == MeasureSpecMode.Fixed || widthSpec.Mode == MeasureSpecMode.AtMost)
            {
                leftWidth = MeasureSpec.MakeFixed(widthSpec.Size * 0.8f);
                rightWidth = MeasureSpec.MakeFixed(widthSpec.Size * 0.2f);
            }
            else if (widthSpec.Mode == MeasureSpecMode.Unspecified)
            {
                leftWidth = widthSpec;
                rightWidth = widthSpec;
            }


            X.Measure(leftWidth, heightSpec, g, font, style);
            Pow.Measure(rightWidth, heightSpec, g, font, style);

            SetDesiredSize(
                X.DesiredWidth + Pow.DesiredWidth,
                Pow.DesiredHeight + X.FindMaxTop() + X.FindMaxBottom() - 10);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            float baseLine = Math.Max(X.FindMaxTop(), Pow.DesiredHeight);

            X.Layout(this, 0, baseLine - X.FindMaxTop(), X.DesiredWidth, X.DesiredHeight, g, font, style);
            Pow.Layout(this, X.DesiredWidth, baseLine - Pow.DesiredHeight, Pow.DesiredWidth, Pow.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            X.Draw(g, font, style);
            Pow.Draw(g, font, style);
        }
    }
}
