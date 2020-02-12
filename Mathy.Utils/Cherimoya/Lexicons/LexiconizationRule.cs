namespace Cherimoya.Lexicons
{
    public class LexiconizationRule
    {
        public char[] StringEnclosers { get; set; }

        public char[] CharEnclosers { get; set; }

        public Punctuations Punctuations { get; set; }

        public EscapeRule EscapeRule { get; set; }
    }
}