using Mathy.Visualization.Text;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class TextExpression : Expression
    {
        public TextExpression(string text, bool isItalic)
        {
            this.text = DecoratedText.Create(text);
            this.isItalic = isItalic;
        }


        private DecoratedText text;

        private bool isItalic;


        public override void FindMaxTop(float[] height)
        {
            float h = (float)text.GetTopHeight();

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = (float)text.GetBottomHeight();

            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            if (!isItalic)
            {
                text.Measure(g, font, style.SmallFont);
            }
            else
            {
                Font f1 = new Font(font, FontStyle.Italic);
                Font f2 = new Font(style.SmallFont, FontStyle.Italic);

                text.Measure(g, f1, f2);

                f1.Dispose();
                f2.Dispose();
            }

            SetDesiredSize(
                GetChildSize(widthSpec, text.Size.Width),
                GetChildSize(heightSpec, text.Size.Height));
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            if (!isItalic)
            {
                text.Draw(g, font, style.SmallFont, Brushes.Black, Left, Top);
            }
            else
            {
                Font f1 = new Font(font, FontStyle.Italic);
                Font f2 = new Font(style.SmallFont, FontStyle.Italic);

                text.Draw(g, f1, f2, Brushes.Black, Left, Top);

                f1.Dispose();
                f2.Dispose();
            }
        }
    }
}
