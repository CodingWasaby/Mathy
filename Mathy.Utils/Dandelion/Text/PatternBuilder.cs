using System.Text;

namespace Mathy.Utils.Dandelion.Text
{

	public class PatternBuilder
	{
		public static string Repeat(string s, int times)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 1; i <= times; i++)
			{
				stringBuilder.Append(s);
			}
			return stringBuilder.ToString();
		}
	}
}
