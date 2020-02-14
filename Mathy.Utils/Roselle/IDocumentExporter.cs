namespace Roselle
{

	public interface IDocumentExporter
	{
		void Init(Stream stream);

		void Close();

		void WriteTable(Table table);

		void WriteEmptyLine();

		void WriteParagraph(Paragraph paragraph);

		void WritePageBreak();

		void WriteTitle(Title title);

		void WriteImage(Image image);
	}
}
