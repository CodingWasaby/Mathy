using System.Drawing;

namespace Mathy.Visualization.Text
{
    class NormalText : DecoratedText
    {
        public NormalText(string text)
        {
            this.text = text;
        }

        private string text;


        public override void Measure(Graphics g, Font font, Font smallFont)
        {
            Size = g.MeasureString(text, font);
        }

        public override void Draw(Graphics g, Font font, Font smallFont, Brush brush, float x, float y)
        {
            g.DrawString(text, font, brush, x, y);
        }

        public override double GetTopHeight()
        {
            return Size.Height / 2;
        }

        public override double GetBottomHeight()
        {
            return Size.Height / 2;
        }
    }
}
