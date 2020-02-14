using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class RootExpression : Expression
    {
        public RootExpression()
        {
            SymbolWidth = 20;
            SymbolHeight = 5;
            RootOverlap = 10;
        }


        public Expression Root { get; set; }

        public Expression X { get; set; }

        public float SymbolWidth { get; set; }

        public float SymbolHeight { get; set; }

        public float RootOverlap { get; set; }


        public override void FindMaxTop(float[] height)
        {
            float h;

            if (Root == null)
            {
                h = X.FindMaxTop() + SymbolHeight;
            }
            else
            {
                h = X.FindMaxTop() + Root.DesiredHeight - RootOverlap;
            }

            h += 2;


            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            X.FindMaxBottom(height);
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            MeasureSpec leftWidth = null;
            MeasureSpec rightWidth = null;


            if (widthSpec.Mode == MeasureSpecMode.Fixed || widthSpec.Mode == MeasureSpecMode.AtMost)
            {
                leftWidth = MeasureSpec.MakeFixed((widthSpec.Size - SymbolWidth) * 0.2f);
                rightWidth = MeasureSpec.MakeFixed((widthSpec.Size - SymbolWidth) * 0.8f);
            }
            else if (widthSpec.Mode == MeasureSpecMode.Unspecified)
            {
                leftWidth = widthSpec;
                rightWidth = widthSpec;
            }

            if (Root != null)
            {
                Root.Measure(leftWidth, heightSpec, g, font, style);
            }

            X.Measure(rightWidth, heightSpec, g, font, style);


            SetDesiredSize(
                (Root == null ? 0 : Root.DesiredWidth) + X.DesiredWidth + SymbolWidth,
                2 + (Root == null ? X.DesiredHeight + SymbolHeight : Root.DesiredHeight + X.DesiredHeight - RootOverlap));
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            if (Root == null)
            {
                X.Layout(this, SymbolWidth, height - X.DesiredHeight, X.DesiredWidth, X.DesiredHeight, g, font, style);
            }
            else
            {
                Root.Layout(this, 0, 0, Root.DesiredWidth, Root.DesiredHeight, g, font, style);
                X.Layout(this, Root.DesiredWidth + SymbolWidth, height - X.DesiredHeight, X.DesiredWidth, X.DesiredHeight, g, font, style);
            }
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            if (Root != null)
            {
                Root.Draw(g, font, style);
            }


            X.Draw(g, font, style);

            float x = Left + (Root == null ? 0 : Root.Width);
            float y = Top + Height - X.Height - 2;

            VectorGraphics.DrawRootSymbol(new RectangleF(x, y, Left + Width - x, X.Height - 1), SymbolWidth, g, Brushes.Black);
        }
    }
}
