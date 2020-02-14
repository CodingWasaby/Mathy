using System.Drawing;

namespace Mathy.Visualization.Text
{
    class SubscriptedText : DecoratedText
    {
        public SubscriptedText(string text)
        {
            int index = text.LastIndexOf('_');

            primaryName = text.Substring(0, index);
            subscript = text.Substring(index + 1);
        }


        private string primaryName { get; set; }

        private string subscript { get; set; }


        private SizeF size1;


        public override void Measure(Graphics g, Font font, Font smallFont)
        {
            size1 = g.MeasureString(primaryName.ToString(), font);


            SizeF size2 = new SizeF();
            int d = 0;

            if (subscript != null)
            {
                size2 = g.MeasureString(subscript, smallFont);
                d = (int)smallFont.Size;
            }


            Size = new SizeF(
                (float)(size1.Width + size2.Width - 5),
                (float)(size1.Height + 5));
        }

        public override void Draw(Graphics g, Font font, Font smallFont, Brush brush, float x, float y)
        {
            SizeF size1 = g.MeasureString(primaryName, font);
            SizeF size2 = g.MeasureString(subscript, smallFont);

            g.DrawString(primaryName, font, Brushes.Black, x, y);
            g.DrawString(subscript, smallFont, Brushes.Black, x + size1.Width - 5, y + Size.Height - size2.Height);
        }

        public override double GetTopHeight()
        {
            return size1.Height / 2;
        }

        public override double GetBottomHeight()
        {
            return Size.Height - size1.Height / 2;
        }
    }
}
