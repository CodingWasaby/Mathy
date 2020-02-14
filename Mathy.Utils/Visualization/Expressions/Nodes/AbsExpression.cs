using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class AbsExpression : Expression
    {
        public Expression Operand { get; set; }


        public override void FindMaxTop(float[] height)
        {
            Operand.FindMaxTop(height);
        }

        public override void FindMaxBottom(float[] height)
        {
            Operand.FindMaxBottom(height);
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            MeasureSpec width = null;

            if (widthSpec.Mode == MeasureSpecMode.Fixed)
            {
                width = MeasureSpec.MakeFixed(widthSpec.Size - 10);
            }
            else if (widthSpec.Mode == MeasureSpecMode.AtMost)
            {
                width = MeasureSpec.MakeFixed(widthSpec.Size - 10);
            }
            else if (widthSpec.Mode == MeasureSpecMode.Unspecified)
            {
                width = widthSpec;
            }

            Operand.Measure(width, heightSpec, g, font, style);


            SetDesiredSize(widthSpec.Mode == MeasureSpecMode.Fixed ? widthSpec.Size : Operand.DesiredWidth + 10, Operand.DesiredHeight);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            Operand.Layout(this, 5, (height - Operand.DesiredHeight) / 2, Operand.DesiredWidth, Operand.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            Operand.Draw(g, font, style);
            VectorGraphics.DrawAbsSymbol(new RectangleF(Left, Top, Width, Height), g, Brushes.Black);
        }
    }
}
