using System.Drawing;
using System.Text;

namespace Mathy.Visualization.Text
{
    class SuperscriptedText : DecoratedText
    {
        public SuperscriptedText(string text)
        {
            int index = text.LastIndexOf("__");

            primaryName = text.Substring(0, index);
            superscript = GetSuperscript(text.Substring(index + 2));
        }


        private string primaryName { get; set; }

        private string superscript { get; set; }


        private SizeF size1;


        private string GetSuperscript(string s)
        {
            bool isL = true;
            foreach (char c in s)
            {
                if (c != 'l')
                {
                    isL = false;
                    break;
                }
            }

            if (isL)
            {
                StringBuilder b = new StringBuilder();
                for (int i = 0; i <= s.Length - 1; i++)
                {
                    b.Append("'");
                }

                return b.ToString();
            }

            return s;
        }


        public override void Measure(Graphics g, Font font, Font smallFont)
        {
            size1 = g.MeasureString(primaryName.ToString(), font);


            SizeF size2 = new SizeF();
            int d = 0;

            if (superscript != null)
            {
                size2 = g.MeasureString(superscript, smallFont);
                d = (int)smallFont.Size;
            }


            Size = new SizeF(
                (float)(size1.Width + size2.Width),
                (float)(size1.Height + 5));
        }

        public override void Draw(Graphics g, Font font, Font smallFont, Brush brush, float x, float y)
        {
            SizeF size1 = g.MeasureString(primaryName, font);
            SizeF size2 = g.MeasureString(superscript, smallFont);

            g.DrawString(primaryName, font, Brushes.Black, x, y);
            g.DrawString(superscript, smallFont, Brushes.Black, x + size1.Width, y);
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
