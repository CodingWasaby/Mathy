// Dandelion.Finance.Currency
using Mathy.Utils.Dandelion.Finance;

namespace Mathy.Utils.Dandelion.Finance
{
	public abstract class Currency
	{
		public abstract int GetLeastUnitValue();

		public abstract void SetLeastUnitValue(int value);

		public abstract string GetCode();

		public static Currency FromLeastValue(int value, string code)
		{
			Currency currency = null;
			if (code == "CNY")
			{
				currency = new Rmb();
			}
			currency.SetLeastUnitValue(value);
			return currency;
		}
	}
}
