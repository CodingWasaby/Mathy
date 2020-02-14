using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class DictionaryExpression : Expression
    {
        public Dictionary<string, Expression> Dictionary { get; set; }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            if (Dictionary.Count == 0)
            {
                SetDesiredSize(25, 25);
                return;
            }


            foreach (Expression value in Dictionary.Values)
            {
                value.Measure(widthSpec, heightSpec, g, font, style);
            }


            float width = Dictionary.Keys.Max(i => Math.Max(40, g.MeasureString(i, font).Width)) + Dictionary.Values.Max(i => i.DesiredWidth);
            float height = Dictionary.Sum(i => Math.Max(g.MeasureString(i.Key, font).Height, i.Value.DesiredHeight));

            SetDesiredSize(width, height);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            if (Dictionary.Count == 0)
            {
                return;
            }


            float y = 0;
            float column1X = Math.Max(40, (float)Dictionary.Max(i => g.MeasureString(i.Key, font).Width));

            foreach (string key in Dictionary.Keys)
            {
                Expression value = Dictionary[key];

                SizeF keySize = g.MeasureString(key, font);

                value.Layout(this, column1X, y, value.DesiredWidth, value.DesiredHeight, g, font, style);
                y += Math.Max(keySize.Height, value.DesiredHeight);
            }
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            if (Dictionary.Count == 0)
            {
                Pen p = new Pen(Color.DarkBlue);
                g.DrawRectangle(p, Left + 2, Top + 2, 20, 20);
                p.Dispose();
                return;
            }


            float y = Top;

            foreach (string key in Dictionary.Keys)
            {
                Expression value = Dictionary[key];
                value.Draw(g, font, style);

                SizeF keySize = g.MeasureString(key, font);

                g.DrawString(key, font, Brushes.DarkBlue, Left, y);


                y += Math.Max(keySize.Height, value.Height);
            }
        }
    }
}
