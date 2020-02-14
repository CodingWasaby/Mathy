namespace Mathy.Utils.Dandelion.Text
{

    internal class DecimalDigitNumberFormatter : NumberFormatter
    {
        public int DigitCount
        {
            get;
            set;
        }

        public override string ToText(double d)
        {
            string format = "{0:." + PatternBuilder.Repeat("0", DigitCount) + "}";
            string text = string.Format(format, d);
            if (text.StartsWith("."))
            {
                text = "0" + text;
            }
            else if (text.StartsWith("-."))
            {
                text = "-0" + text.Substring(1);
            }
            return text;
        }
    }
}
