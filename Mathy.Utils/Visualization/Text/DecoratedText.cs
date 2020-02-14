using Mathy.Language;
using System.Drawing;

namespace Mathy.Visualization.Text
{
    abstract class DecoratedText
    {
        public SizeF Size { get; protected set; }


        public abstract void Measure(Graphics g, Font font, Font smallFont);

        public abstract void Draw(Graphics g, Font font, Font smallFont, Brush brush, float x, float y);

        public abstract double GetTopHeight();

        public abstract double GetBottomHeight();


        public static DecoratedText Create(string text)
        {
            if (text.Contains("gk_"))
            {
                foreach (string greekLetter in Funcs.GetGreekLetters())
                {
                    text = text.Replace("gk_" + greekLetter, Funcs.GetGreekLetter(greekLetter));
                }
            }


            if (text.StartsWith("#"))
            {
                return new NormalText(MathyConstants.GetSymbol(text.Substring(1)));
            }
            else if (text.IndexOf('_') == -1)
            {
                return new NormalText(text);
            }
            else if (text.IndexOf("__") == -1)
            {
                return new SubscriptedText(text);
            }
            else
            {
                return new SuperscriptedText(text);
            }
        }
    }
}
