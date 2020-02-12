namespace Cherimoya.Lexicons
{
    public class CharLexicon : Lexicon
    {
        internal CharLexicon(char value)
        {
            Value = value;
        }


        public char Value { get; private set; }

        public override string ToString()
        {
            return "char:" + Value;
        }
    }
}