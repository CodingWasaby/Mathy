using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class UpperlineExpression : Expression
    {
        public Expression Body { get; set; }


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
            float h = Body.FindMaxBottom();

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            Body.Measure(widthSpec, heightSpec, g, font, style);

            SetDesiredSize(Body.DesiredWidth, Body.DesiredHeight);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            Body.Layout(this, 0, 0, Body.DesiredWidth, Body.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            Body.Draw(g, font, style);


            Pen p = new Pen(Color.Black);
            g.DrawLine(p, Left, Top, Left + Width, Top);
            p.Dispose();
        }
    }
}
