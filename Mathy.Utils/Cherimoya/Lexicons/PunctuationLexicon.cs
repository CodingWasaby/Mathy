namespace Cherimoya.Lexicons
{
    public class PunctuationLexicon : Lexicon
    {
        internal PunctuationLexicon(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }
    }
}