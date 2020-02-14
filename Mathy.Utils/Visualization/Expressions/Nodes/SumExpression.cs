using System;
using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class SigmaConfig
    {
        public Expression From { get; set; }

        public Expression To { get; set; }
    }

    public class SumExpression : Expression
    {
        public SumExpression()
        {
            SigmaWidth = 20;
            SigmaHeight = 25;
        }


        public Expression Operand { get; set; }

        public float SigmaWidth { get; set; }

        public float SigmaHeight { get; set; }

        public SigmaConfig[] Sigmas { get; set; }


        public override void FindMaxTop(float[] height)
        {
            float h = Math.Max(Sigmas.Max(i => SigmaHeight / 2 + (i.To == null ? 0 : i.To.DesiredHeight)), Operand.FindMaxTop()) + 3;

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = Math.Max(Sigmas.Max(i => SigmaHeight / 2 + (i.From == null ? 0 : i.From.DesiredHeight)), Operand.FindMaxBottom()) + 3;

            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            float desiredWidth;

            if (widthSpec.Mode != MeasureSpecMode.Unspecified)
            {
                float width = SigmaWidth * Sigmas.Length + 5 * (Sigmas.Length - 1);
                Operand.Measure(MeasureSpec.MakeAtMost(widthSpec.Size - width), heightSpec, g, font, style);
                width += Operand.DesiredWidth;

                if (widthSpec.Mode == MeasureSpecMode.Fixed)
                {
                    desiredWidth = widthSpec.Size;
                }
                else
                {
                    desiredWidth = width;
                }
            }
            else
            {
                desiredWidth = 0;

                foreach (SigmaConfig sigma in Sigmas)
                {
                    if (sigma.From != null)
                    {
                        sigma.From.Measure(widthSpec, heightSpec, g, style.SmallFont, style);
                        sigma.To.Measure(widthSpec, heightSpec, g, style.SmallFont, style);
                        desiredWidth += Math.Max(SigmaWidth, Math.Max(sigma.From.DesiredWidth, sigma.To.DesiredWidth));
                    }
                }

                desiredWidth += 5 * (Sigmas.Length - 1);

                Operand.Measure(widthSpec, heightSpec, g, font, style);
                desiredWidth += Operand.DesiredWidth;
            }


            SetDesiredSize((float)desiredWidth,
                Math.Max(SigmaHeight / 2 + Sigmas.Max(i => i.From == null ? 0 : i.From.DesiredHeight), Operand.FindMaxBottom()) +
                Math.Max(SigmaHeight / 2 + Sigmas.Max(i => i.To == null ? 0 : i.To.DesiredHeight), Operand.FindMaxTop()) + 6);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            float x = 0;

            float y = GetSigmaTop();

            foreach (SigmaConfig sigma in Sigmas)
            {
                if (sigma.From != null)
                {
                    float sectionWidth = Math.Max(SigmaWidth, Math.Max(sigma.From.DesiredWidth, sigma.To.DesiredWidth));

                    sigma.To.Layout(this, x + (sectionWidth - sigma.To.DesiredWidth), (float)(y - sigma.To.DesiredHeight), sigma.To.DesiredWidth, sigma.To.DesiredHeight, g, font, style);
                    sigma.From.Layout(this, x + (sectionWidth - sigma.From.DesiredWidth), (float)(y + SigmaHeight + 1), sigma.From.DesiredWidth, sigma.From.DesiredHeight, g, font, style);

                    x += sectionWidth + 5;
                }
                else
                {
                    x += SigmaWidth + 5;
                }
            }

            Operand.Layout(this, x - 5, y + SigmaHeight / 2 - Operand.FindMaxTop(), Operand.DesiredWidth, Operand.DesiredHeight, g, font, style);
        }


        private float GetSigmaTop()
        {
            return (float)(DesiredHeight - 3 - Math.Max(SigmaHeight / 2 + Sigmas.Max(i => i.From == null ? 0 : i.From.DesiredHeight), Operand.FindMaxBottom()) - SigmaHeight / 2);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            float left = Left;
            float y = Top + GetSigmaTop();

            Operand.Draw(g, font, style);

            foreach (SigmaConfig sigma in Sigmas)
            {
                float sectionWidth = sigma.From == null ? SigmaWidth : Math.Max(SigmaWidth, Math.Max(sigma.From.DesiredWidth, sigma.To.DesiredWidth));

                VectorGraphics.DrawSigmaSymbol(new RectangleF(left + (sectionWidth - SigmaWidth) / 2, y, SigmaWidth, SigmaHeight), g, Brushes.Black);

                if (sigma.From != null)
                {
                    sigma.From.Draw(g, style.SmallFont, style);
                }

                if (sigma.To != null)
                {
                    sigma.To.Draw(g, style.SmallFont, style);
                }


                left += sectionWidth + 5;
            }
        }
    }
}
