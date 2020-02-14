using Petunia;
using System;
using System.Linq;

namespace Mathy.Client.Model
{
    public class FuncArticleVM
    {
        public FuncArticleVM(Article article)
        {
            Paragraphs = article.Items.Select(i => ParseArticleItem(i)).ToArray();
        }


        public ParagraphVM[] Paragraphs { get; set; }


        private ParagraphVM ParseArticleItem(IArticleItem item)
        {
            bool hasBullet = false;
            bool hasBorder = false;
            string text = null;

            if (item is Bullet)
            {
                hasBullet = true;
                text = (item as Bullet).Text;
            }
            else if (item is EmptyLine)
            {
                text = "<br/>";
            }
            else if (item is Example)
            {
                text = "<b>例如：</b>";
            }
            else if (item is Image)
            {
                text = string.Format("<img src=\"/Repository/Docs/Resources/{0}\"/>", (item as Image).FileName);
            }
            else if (item is Code)
            {
                hasBorder = true;
                text = string.Join("<br/>", (item as Code).Text.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            }
            else if (item is Paragraph)
            {
                text = (item as Paragraph).Text;
            }


            return new ParagraphVM() { HasBullet = hasBullet, HasBorder = hasBorder, Text = text };
        }
    }

    public class ParagraphVM
    {
        public Boolean HasBullet { get; set; }

        public bool HasBorder { get; set; }

        public string Text { get; set; }
    }
}