using System.IO;
namespace Roselle
{
	public class DocumentWriter
	{
		public Document Document
		{
			get;
			set;
		}

		public IDocumentExporter Exporter
		{
			get;
			set;
		}

		public void Write(Stream stream)
		{
			Exporter.Init(stream);
			ExportDocument();
		}

		public void Write(string filePath)
		{
			FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
			Exporter.Init(fileStream);
			ExportDocument();
			fileStream.Flush();
			fileStream.Close();
		}

		private void ExportDocument()
		{
			foreach (IDocumentElement element in Document.Elements)
			{
				if (element is Title)
				{
					Exporter.WriteTitle(element as Title);
				}
				else if (element is Paragraph)
				{
					Exporter.WriteParagraph(element as Paragraph);
				}
				else if (element is EmptyLine)
				{
					Exporter.WriteEmptyLine();
				}
				else if (element is PageBreak)
				{
					Exporter.WritePageBreak();
				}
				else if (element is Table)
				{
					Exporter.WriteTable(element as Table);
				}
				else if (element is Image)
				{
					Exporter.WriteImage(element as Image);
				}
			}
			Exporter.Close();
		}
	}
}
