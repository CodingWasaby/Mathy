namespace Mathy.Utils.Dandelion.Maths
{
	public class MathsFuncs
	{
		public static double C(int n, int m)
		{
			double num = 1.0;
			for (int i = m + 1; i <= n; i++)
			{
				num *= (double)i;
			}
			double num2 = 1.0;
			for (int i = 1; i <= n - m; i++)
			{
				num2 *= (double)i;
			}
			return num / num2;
		}
	}
}
