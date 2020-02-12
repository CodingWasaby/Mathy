namespace Cherimoya.Lexicons
{
    public class IntegerLexicon : Lexicon
    {
        internal IntegerLexicon(int value)
        {
            this.Value = value;
        }

        public int Value { get; private set; }

        public override string ToString()
        {
            return "int:" + Value;
        }
    }
}