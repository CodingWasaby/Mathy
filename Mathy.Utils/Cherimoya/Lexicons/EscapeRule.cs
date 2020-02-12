namespace Cherimoya.Lexicons
{
    public abstract class EscapeRule
    {
        public abstract char ReadEscapedChar(string s, int startPosition, int[] endPositionm, LexiconizationErrorProvider errorProvider);
    }
}