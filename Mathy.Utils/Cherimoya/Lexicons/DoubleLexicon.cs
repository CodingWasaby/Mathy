namespace Cherimoya.Lexicons
{
    public class DoubleLexicon : Lexicon
    {
        internal DoubleLexicon(double value)
        {
            Value = value;
        }

        public double Value { get; private set; }

        public override string ToString()
        {
            return "double:" + Value;
        }
    }
}