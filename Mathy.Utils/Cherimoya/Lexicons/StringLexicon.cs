namespace Cherimoya.Lexicons
{
    public class StringLexicon : Lexicon
    {
        internal StringLexicon(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return string.Format("\"{0}\"", Value);
        }
    }
}