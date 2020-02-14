using System;
using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class LambdaExpression : Expression
    {
        public Expression[] Variables { get; set; }

        public Expression Body { get; set; }


        public override void FindMaxTop(float[] height)
        {
            float h = Math.Max(variablesSize.Height / 2, Body.FindMaxTop());

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = Math.Max(variablesSize.Height / 2, Body.FindMaxBottom());

            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        private SizeF variablesSize;


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            Body.Measure(widthSpec, heightSpec, g, font, style);

            foreach (Expression variable in Variables)
            {
                variable.Measure(widthSpec, heightSpec, g, font, style);
            }


            variablesSize = new SizeF(Variables.Max(i => i.DesiredWidth), Variables.Sum(i => i.DesiredHeight));

            SetDesiredSize(
                variablesSize.Width + 20 + Body.DesiredWidth,
                Math.Max(variablesSize.Height / 2, Body.FindMaxTop()) + Math.Max(variablesSize.Height / 2, Body.FindMaxBottom()));
        }


        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            float baseLine = height - Math.Max(variablesSize.Height / 2, Body.FindMaxBottom());

            Body.Layout(this, variablesSize.Width + 20, baseLine - Body.FindMaxTop(), Body.DesiredWidth, Body.DesiredHeight, g, font, style);


            float y = baseLine - variablesSize.Height / 2;

            foreach (Expression variable in Variables)
            {
                variable.Layout(this, 0, y, variable.DesiredWidth, variable.DesiredHeight, g, font, style);
                y += variable.DesiredHeight;
            }
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {

            float baseLine = Top + Height - Math.Max(variablesSize.Height / 2, Body.FindMaxBottom());

            float y = baseLine - variablesSize.Height / 2 - 2;


            foreach (Expression variable in Variables)
            {
                variable.Draw(g, font, style);
            }


            VectorGraphics.DrawArrow(new RectangleF(Left + variablesSize.Width, baseLine - 10, 20, 20), g, Brushes.Black);


            Body.Draw(g, font, style);
        }
    }
}
