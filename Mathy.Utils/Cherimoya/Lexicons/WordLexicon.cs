namespace Cherimoya.Lexicons
{
    public class WordLexicon : Lexicon
    {
        internal WordLexicon(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }

        public override string ToString()
        {
            return Text;
        }
    }
}