using System;
using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class FunctionExpression : Expression
    {
        public string Name { get; set; }

        public Expression[] Operands { get; set; }

        public int ParantheseLevel { get; set; }

        public override void FindMaxTop(float[] height)
        {
            float h = nameSize.Height / 2;

            if (Operands.Length > 0)
            {
                h = !isMultiLine ? Operands.Max(i => i.FindMaxTop()) : nameSize.Height / 2;
            }


            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = DesiredHeight - nameSize.Height / 2;

            if (Operands.Length > 0)
            {
                h = !isMultiLine ? Operands.Max(i => i.FindMaxBottom()) : DesiredHeight - nameSize.Height / 2;
            }


            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        private bool isMultiLine;

        private SizeF nameSize;


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            float desiredWidth;

            nameSize = g.MeasureString(Name + "(", font);

            if (widthSpec.Mode != MeasureSpecMode.Unspecified)
            {
                float width = g.MeasureString(Name + "(", font).Width;

                float index = 0;

                foreach (Expression operand in Operands)
                {
                    operand.Measure(MeasureSpec.MakeAtMost(widthSpec.Size), heightSpec, g, font, style);
                    float puncWidth = g.MeasureString(index == Operands.Length - 1 ? ")" : ",", font).Width;

                    if (width + operand.DesiredWidth + puncWidth > widthSpec.Size)
                    {
                        operand.Measure(MeasureSpec.MakeAtMost(widthSpec.Size - width), heightSpec, g, font, style);
                        break;
                    }


                    width += operand.DesiredWidth + puncWidth;

                    index++;
                }

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
                desiredWidth = g.MeasureString(Name + "(", font).Width;

                float index = 0;

                foreach (Expression operand in Operands)
                {
                    operand.Measure(widthSpec, heightSpec, g, font, style);

                    float puncWidth = g.MeasureString(index == Operands.Length - 1 ? ")" : ",", font).Width;
                    desiredWidth += operand.DesiredWidth + puncWidth;

                    index++;
                }
            }


            if (desiredWidth <= 300)
            {
                SetDesiredSize(desiredWidth, Math.Max(g.MeasureString(Name, font).Height, Operands.Length == 0 ? 0 : Operands.Max(i => i.FindMaxTop()) + Operands.Max(i => i.FindMaxBottom())));
            }
            else
            {
                SetDesiredSize(Operands.Max(i => i.DesiredWidth) + g.MeasureString(",", font).Width + 50, g.MeasureString(Name + "(", font).Height + g.MeasureString(")", font).Height + Operands.Sum(i => i.DesiredHeight));
                isMultiLine = true;
            }
        }


        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            if (!isMultiLine)
            {
                float x = g.MeasureString(Name + "(", font).Width;

                float index = 0;

                float baseLine = height - Math.Max(g.MeasureString(Name + "(", font).Height / 2, Operands.Length == 0 ? 0 : Operands.Max(i => i.FindMaxBottom()));

                foreach (Expression operand in Operands)
                {
                    operand.Layout(this, x, baseLine - operand.FindMaxTop(), operand.DesiredWidth, operand.DesiredHeight, g, font, style);

                    float puncWidth = g.MeasureString(index == Operands.Length - 1 ? ")" : ",", font).Width;
                    x += operand.DesiredWidth + puncWidth;

                    index++;
                }
            }
            else
            {
                float y = g.MeasureString(Name + "(", font).Height;

                foreach (Expression operand in Operands)
                {
                    operand.Layout(this, 50, y, operand.DesiredWidth, operand.DesiredHeight, g, font, style);

                    y += operand.DesiredHeight;
                }
            }
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            Brush brush = style.LevelContext.GetBrush(ParantheseLevel);

            if (!isMultiLine)
            {
                SizeF size = g.MeasureString(Name + "(", font);

                float baseLine = Top + Height - Math.Max(size.Height / 2, Operands.Length == 0 ? 0 : Operands.Max(i => i.FindMaxBottom()));

                g.DrawString(Name, font, Brushes.Black, Left, baseLine - size.Height / 2);
                g.DrawString("(", style.BoldFont, brush, Left + g.MeasureString(Name, font).Width - 5, baseLine - size.Height / 2);


                float x = Left + size.Width;

                float index = 0;

                SizeF puncSize = g.MeasureString(",", font);

                foreach (Expression operand in Operands)
                {
                    x += operand.Width;

                    if (index < Operands.Length - 1)
                    {
                        g.DrawString(",", font, Brushes.Black, x, baseLine - puncSize.Height / 2);
                        x += puncSize.Width;
                    }

                    index++;

                    operand.Draw(g, font, style);
                }


                g.DrawString(")", style.BoldFont, brush, x, baseLine - g.MeasureString(")", font).Height / 2);
            }
            else
            {
                SizeF size = g.MeasureString(Name + "(", font);

                g.DrawString(Name, font, Brushes.Black, Left, Top);
                g.DrawString("(", style.BoldFont, brush, Left + g.MeasureString(Name, font).Width - 5, Top);


                float y = Top + size.Height;


                int index = 0;

                foreach (Expression operand in Operands)
                {
                    if (index < Operands.Length - 1)
                    {
                        g.DrawString(",", font, Brushes.Black, Left + operand.Width + 50, y + operand.Height - g.MeasureString(",", font).Height);
                    }


                    operand.Draw(g, font, style);
                    y += operand.Height;

                    index++;
                }

                g.DrawString(")", font, brush, Left, y);
            }
        }
    }
}
