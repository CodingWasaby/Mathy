// Dandelion.Finance.Rmb
using Mathy.Utils.Dandelion.Finance;
namespace Mathy.Utils.Dandelion.Finance
{
	public class Rmb : Currency
	{
		public int Yuan
		{
			get;
			private set;
		}

		public int Jiao
		{
			get;
			private set;
		}

		public int Fen
		{
			get;
			private set;
		}

		public static Rmb of(int yuan, int jiao, int fen)
		{
			Rmb rmb = new Rmb();
			rmb.Yuan = yuan;
			rmb.Jiao = jiao;
			rmb.Fen = fen;
			return rmb;
		}

		public static Rmb Of(int fen)
		{
			Rmb rmb = new Rmb();
			rmb.SetLeastUnitValue(fen);
			return rmb;
		}

		public override int GetLeastUnitValue()
		{
			return Yuan * 100 + Jiao * 10 + Fen;
		}

		public override void SetLeastUnitValue(int value)
		{
			Fen = value % 10;
			Jiao = (value - Fen) % 100 / 10;
			Yuan = (value - Fen - Jiao * 10) / 100;
		}

		public override string GetCode()
		{
			return "CNY";
		}

		public override string ToString()
		{
			return string.Format("￥%d.%d%d", Yuan, Jiao, Fen);
		}
	}
}
