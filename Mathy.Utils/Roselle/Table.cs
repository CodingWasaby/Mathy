
using Roselle;
namespace Roselle
{
	public class Table : IDocumentElement
	{
		public bool HasHeader
		{
			get;
			set;
		}

		public string[][] Cells
		{
			get;
			set;
		}

		public Table(int rowCount, int columnCount)
		{
			Cells = new string[rowCount][];
			for (int i = 0; i <= rowCount - 1; i++)
			{
				Cells[i] = new string[columnCount];
			}
		}
	}
}
