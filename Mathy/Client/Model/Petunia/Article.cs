using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petunia
{
    public class Article
    {
        public IArticleItem[] Items { get; set; }

        public List<ParagraphV> Paragraphs { get; set; }

        public string Memo
        {
            get
            {
                var m = Paragraphs.Where(m => !string.IsNullOrEmpty(m.Text)).Select(m => m.Text).ToList();
                var sb = new StringBuilder();
                foreach(var n in m)
                {
                    sb.AppendLine(n);
                }
                return sb.ToString();
            }
        }

        public static Article Parse(string str)
        {
            var article = ArticleParser.Parse(str);
            if (article != null)
                article.Paragraphs = article.Items.Select(i => ParseArticleItem(i)).ToList();
            else
                article.Paragraphs = new List<ParagraphV>();
            return article;
        }

        private static ParagraphV ParseArticleItem(IArticleItem item)
        {
            string PhType = "";
            string text = null;

            if (item is Bullet)
            {
                PhType = "Bullet";
                text = (item as Bullet).Text;
            }
            else if (item is EmptyLine)
            {
                PhType = "EmptyLine";
            }
            else if (item is Example)
            {
                PhType = "Example";
            }
            else if (item is Image)
            {
                PhType = "Image";
                text = (item as Image).FileName;
            }
            else if (item is Code)
            {
                PhType = "Code";
                text = (item as Code).Text;
            }
            else if (item is Paragraph)
            {
                PhType = "Paragraph";
                text = (item as Paragraph).Text;
            }

            return new ParagraphV() { PhType = PhType, Text = text };
        }
    }


    public class ParagraphV
    {
        public string PhType { get; set; }

        public string Text { get; set; }
    }

    public interface IArticleItem
    {
    }

    public class Bullet : IArticleItem
    {
        public string Text { get; set; }
    }

    public class EmptyLine : IArticleItem
    {
    }

    public class Example : IArticleItem
    {
    }

    public class Code : IArticleItem
    {
        public string Text { get; set; }
    }

    public class Image : IArticleItem
    {
        public string FileName { get; set; }
    }

    public class Paragraph : IArticleItem
    {
        public string Text { get; set; }
    }
}
