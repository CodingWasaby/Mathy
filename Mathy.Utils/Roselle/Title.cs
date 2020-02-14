namespace Roselle
{
	public class Title : IDocumentElement
	{
		public string Text
		{
			get;
			set;
		}

		public int Level
		{
			get;
			set;
		}

		public Title(string text, int level)
		{
			Text = text;
			Level = level;
		}
	}
}
