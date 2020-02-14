using Roselle;
using System.Drawing;
namespace Roselle
{
	public class Image : IDocumentElement
	{
		public Bitmap Bitmap
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public Image(Bitmap bitmap, string name)
		{
			Bitmap = bitmap;
			Name = name;
		}
	}
}
