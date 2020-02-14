using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class ArrayExpression : Expression
    {
        public Expression[] Items { get; set; }


        private float maxTop;

        private float maxBottom;


        public override void FindMaxTop(float[] height)
        {
            float h = Items.Max(i => i.FindMaxTop());

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = Items.Max(i => i.FindMaxBottom());

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            SizeF bracketSize = g.MeasureString("[", font);
            SizeF separatorSize = g.MeasureString(",", font);

            float maxTop = bracketSize.Height / 2;
            float maxBottom = bracketSize.Height / 2;

            foreach (Expression item in Items)
            {
                item.Measure(widthSpec, heightSpec, g, font, style);

                float top = item.FindMaxTop();
                float bottom = item.FindMaxBottom();

                if (maxTop < top)
                {
                    maxTop = top;
                }

                if (maxBottom < bottom)
                {
                    maxBottom = bottom;
                }
            }


            this.maxTop = maxTop;
            this.maxBottom = maxBottom;


            float width = Items.Sum(i => i.DesiredWidth) + bracketSize.Width * 2;
            if (Items.Length > 0)
            {
                width += (Items.Length - 1) * separatorSize.Width;
            }


            SetDesiredSize(width, maxTop + maxBottom);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            SizeF bracketSize = g.MeasureString("[", font);
            SizeF separatorSize = g.MeasureString(",", font);

            float baseLine = DesiredHeight - maxBottom;

            float x = bracketSize.Width;

            foreach (Expression item in Items)
            {
                item.Layout(this, x, baseLine - item.FindMaxTop(), item.DesiredWidth, item.DesiredHeight, g, font, style);
                x += item.DesiredWidth + separatorSize.Width;
            }
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {

            VectorGraphics.DrawSquareBracket(new RectangleF(Left + 3, Top, Width - 6, Height), g, Brushes.Black);

            SizeF bracketSize = g.MeasureString("[", font);
            SizeF separatorSize = g.MeasureString(",", font);


            float baseLine = Top + Height - maxBottom;

            float x = Left + bracketSize.Width;


            int index = 0;

            foreach (Expression item in Items)
            {
                item.Draw(g, font, style);

                x += item.DesiredWidth;

                if (index < Items.Length - 1)
                {
                    g.DrawString(",", font, Brushes.Black, x, baseLine - separatorSize.Height / 2);
                    x += separatorSize.Width;
                }


                index++;
            }
        }
    }
}
