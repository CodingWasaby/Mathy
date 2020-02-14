using System;
using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class MultiIndexingExpression : Expression
    {
        public Expression Operand { get; set; }

        public Expression[] Indexers { get; set; }


        public override void FindMaxTop(float[] height)
        {
            float h = Operand.FindMaxTop();

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = Operand.FindMaxBottom() + Indexers.Max(i => i.DesiredHeight) - indexOffset;

            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        private float indexOffset;

        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            float commaWidth = (float)g.MeasureString(",", style.SmallFont).Width;

            Operand.Measure(widthSpec, heightSpec, g, font, style);

            foreach (Expression indexer in Indexers)
            {
                indexer.Measure(widthSpec, heightSpec, g, style.SmallFont, style);
            }


            indexOffset = Math.Min(10, Indexers.Max(i => i.FindMaxTop()));

            SetDesiredSize(
                Operand.DesiredWidth + Indexers.Sum(i => i.DesiredWidth) + (Indexers.Length - 1) * commaWidth,
                Operand.FindMaxTop() + Operand.FindMaxBottom() + Indexers.Max(i => i.DesiredHeight) - indexOffset);
        }


        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            float baselineY = Operand.FindMaxTop() + Operand.FindMaxBottom() + Indexers.Max(i => i.FindMaxTop()) - indexOffset;


            Operand.Layout(this, 0, 0, Operand.DesiredWidth, Operand.DesiredHeight, g, font, style);

            float x = Operand.DesiredWidth;
            float commaWidth = g.MeasureString(",", style.SmallFont).Width;

            foreach (Expression indexer in Indexers)
            {
                indexer.Layout(this, x, baselineY - indexer.FindMaxTop(), indexer.DesiredWidth, indexer.DesiredHeight, g, style.SmallFont, style);
                x += indexer.DesiredWidth + commaWidth;
            }
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            SizeF commaSize = g.MeasureString(",", style.SmallFont);

            float baselineY = Top + Operand.Height;


            Operand.Draw(g, font, style);

            float x = Left + Operand.DesiredWidth;

            float index = 0;

            foreach (Expression indexer in Indexers)
            {
                indexer.Draw(g, style.SmallFont, style);

                if (index < Indexers.Length - 1)
                {
                    g.DrawString(",", style.SmallFont, Brushes.Black, x + indexer.DesiredWidth, baselineY - commaSize.Height / 2);
                    x += indexer.DesiredWidth + commaSize.Width;
                }

                index++;
            }
        }
    }
}
