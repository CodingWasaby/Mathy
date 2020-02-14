using System.Drawing;
namespace Roselle
{
    public class DocumentBuilder
    {
        public Document Document
        {
            get;
            private set;
        }

        public DocumentBuilder()
        {
            Document = new Document();
        }

        public DocumentBuilder Title(string text, int level)
        {
            Document.Elements.Add(new Title(text, level));
            return this;
        }

        public DocumentBuilder T0(string text)
        {
            return Title(text, 0);
        }

        public DocumentBuilder T1(string text)
        {
            return Title(text, 1);
        }

        public DocumentBuilder T2(string text)
        {
            return Title(text, 2);
        }

        public DocumentBuilder P(string text, params TextStyle[] styles)
        {
            Document.Elements.Add(new Paragraph(text)
            {
                Styles = styles
            });
            return this;
        }

        public DocumentBuilder Br()
        {
            Document.Elements.Add(new EmptyLine());
            return this;
        }

        public DocumentBuilder Pb()
        {
            Document.Elements.Add(new PageBreak());
            return this;
        }

        public DocumentBuilder Center(string text, FontSize fontSize, params TextStyle[] styles)
        {
            Document.Elements.Add(new Paragraph(text)
            {
                IsCentered = true,
                FontSize = fontSize,
                Styles = styles
            });
            return this;
        }

        public TableBuilder StartTable()
        {
            return new TableBuilder(Document, this);
        }

        public DocumentBuilder Image(Bitmap bitmap, string name)
        {
            Document.Elements.Add(new Roselle.Image(bitmap, name));
            return this;
        }
    }

}