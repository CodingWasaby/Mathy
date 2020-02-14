

namespace Mathy.Utils.Dandelion.Text
{

	public abstract class NumberFormatter
	{
		public abstract string ToText(double d);

		public static NumberFormatter ForDecimalDigits(int count)
		{
			DecimalDigitNumberFormatter decimalDigitNumberFormatter = new DecimalDigitNumberFormatter();
			decimalDigitNumberFormatter.DigitCount = count;
			return decimalDigitNumberFormatter;
		}
	}
}
