using System;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class LimExpression : Expression
    {
        public Expression X { get; set; }

        public Expression Limit { get; set; }

        public Expression Body { get; set; }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            SizeF limSize = g.MeasureString("lim", font);

            float width;

            if (widthSpec.Mode != MeasureSpecMode.Unspecified)
            {
                MeasureSpec spec = MeasureSpec.MakeAtMost((widthSpec.Size - 10) / 2);
                X.Measure(spec, heightSpec, g, style.SmallFont, style);
                Limit.Measure(spec, heightSpec, g, style.SmallFont, style);


                float leftWidth = Math.Max(X.DesiredWidth + Limit.DesiredWidth + 10, limSize.Width);
                Body.Measure(MeasureSpec.MakeAtMost(widthSpec.Size - leftWidth), heightSpec, g, font, style);

                if (widthSpec.Mode == MeasureSpecMode.Fixed)
                {
                    width = widthSpec.Size;
                }
                else
                {
                    width = Math.Min(leftWidth + Body.DesiredWidth, widthSpec.Size);
                }
            }
            else
            {
                X.Measure(widthSpec, heightSpec, g, font, style);
                Limit.Measure(widthSpec, heightSpec, g, font, style);
                Body.Measure(widthSpec, heightSpec, g, font, style);

                width = Math.Max(X.DesiredWidth + Limit.DesiredWidth + 10, limSize.Width) + Body.DesiredWidth;
            }

            SetDesiredSize(width, Math.Max(Math.Max(X.DesiredHeight, Limit.DesiredHeight) + limSize.Height, Body.DesiredHeight));
        }


        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            SizeF limSize = g.MeasureString("lim", font);
            float leftWidth = Math.Max(X.DesiredWidth + Limit.DesiredWidth + 10, limSize.Width);

            float x = leftWidth - (X.DesiredWidth + Limit.DesiredWidth + 10) / 2;

            float y1 = ((DesiredHeight - (limSize.Height + Math.Max(X.DesiredHeight, Limit.DesiredHeight)))) / 2 + limSize.Height;
            float y2 = y1 + Math.Max(X.DesiredHeight, Limit.DesiredHeight);

            X.Layout(this, x, y1 + ((y2 - y1) - X.DesiredHeight) / 2, X.DesiredWidth, X.DesiredHeight, g, style.SmallFont, style);

            x += X.DesiredWidth + 10;
            Limit.Layout(this, x, y1 + ((y2 - y1) - Limit.DesiredHeight) / 2, Limit.DesiredWidth, Limit.DesiredHeight, g, style.SmallFont, style);

            Body.Layout(this, leftWidth, (DesiredHeight - Body.DesiredHeight) / 2, Body.DesiredWidth, Body.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            SizeF limSize = g.MeasureString("lim", font);
            float leftWidth = Math.Max(X.DesiredWidth + Limit.DesiredWidth + 10, limSize.Width);

            float bottomHeight = Height - limSize.Height;

            g.DrawString("lim", font, Brushes.Black, Left + (leftWidth - limSize.Width) / 2, 0);


            float x1 = X.Left + X.Width;
            float x2 = x1 + 10;

            float y1 = ((Height - (limSize.Height + Math.Max(X.Height, Limit.Height)))) / 2 + limSize.Height;
            float y = y1 + Math.Max(X.Height, Limit.Height) / 2;

            Pen p = new Pen(Color.Black);
            g.DrawLine(p, x1, y, x2, y);
            g.DrawLine(p, x2 - 2, y - 2, x2, y);
            g.DrawLine(p, x2 - 2, y + 2, x2, y);
            p.Dispose();


            X.Draw(g, style.SmallFont, style);
            Limit.Draw(g, style.SmallFont, style);
            Body.Draw(g, font, style);
        }
    }
}
