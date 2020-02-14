using System;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class DiffExpression : Expression
    {
        public DiffExpression(bool isPartial, Expression f, Expression x)
        {
            this.isPartial = isPartial;
            symbol = isPartial ? "∂" : "d";
            this.f = f;
            this.x = x;
        }


        private SizeF symbolSize;

        private string symbol;

        private bool isPartial;

        private Expression f;

        private Expression x;


        public override void FindMaxTop(float[] height)
        {
            float h = Math.Max(symbolSize.Height, f.DesiredHeight);

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = Math.Max(symbolSize.Height, x.DesiredHeight);

            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            symbolSize = g.MeasureString(symbol, font);
            symbolSize = new SizeF(symbolSize.Width - 2, symbolSize.Height);

            f.Measure(widthSpec, heightSpec, g, font, style);
            x.Measure(widthSpec, heightSpec, g, font, style);


            float padding = 2;

            SetDesiredSize(
                Math.Max(f.DesiredWidth, x.DesiredWidth) + symbolSize.Width + padding * 2,
                Math.Max(symbolSize.Height, f.DesiredHeight) + Math.Max(symbolSize.Height, x.DesiredHeight));
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            f.Layout(this,
                (DesiredWidth - (symbolSize.Width + f.DesiredWidth)) / 2 + symbolSize.Width,
                FindMaxTop() - Math.Max(symbolSize.Height / 2, f.FindMaxBottom()) - f.FindMaxTop(),
                f.DesiredWidth, f.DesiredHeight, g, font, style);


            float pos = DesiredHeight - Math.Max(symbolSize.Height, x.DesiredHeight);

            x.Layout(this,
               (DesiredWidth - (symbolSize.Width + x.DesiredWidth)) / 2 + symbolSize.Width,
               pos + FindMaxBottom() - Math.Max(symbolSize.Height / 2, x.FindMaxBottom()) - x.FindMaxTop(),
               x.DesiredWidth, x.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            float pos = DesiredHeight - Math.Max(symbolSize.Height, x.DesiredHeight);

            Pen p = new Pen(Brushes.Black);
            g.DrawLine(p, Left, Top + pos, Left + Width, Top + pos);
            p.Dispose();


            float offset = isPartial ? 2 : 0;

            g.DrawString(symbol,
                font,
                Brushes.Black,
                Left + (Width - (symbolSize.Width + f.Width)) / 2,
                offset + Top + FindMaxTop() - Math.Max(symbolSize.Height / 2, f.FindMaxBottom()) - symbolSize.Height / 2);
            g.DrawString(symbol,
               font,
               Brushes.Black,
               Left + (Width - (symbolSize.Width + x.Width)) / 2,
               offset + Top + pos + FindMaxBottom() - Math.Max(symbolSize.Height / 2, x.FindMaxBottom()) - symbolSize.Height / 2);


            f.Draw(g, font, style);
            x.Draw(g, font, style);
        }
    }
}
