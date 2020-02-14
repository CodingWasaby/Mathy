using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Petunia
{
    class ArticleParser
    {
        public static Article Parse(string str)
        {
            List<IArticleItem> items = new List<IArticleItem>();
            foreach (var n in str.Split("\n"))
            {
                IArticleItem item = ParseLine(n);
                if (item != null)
                {
                    items.Add(item);
                }
            }
            return new Article() { Items = items.ToArray() };
        }


        private static bool isInCode;
        
        
        private static StringBuilder code;

        private static IArticleItem ParseLine(string line)
        {
            if (isInCode)
            {
                if (line == "@>")
                {
                    isInCode = false;
                    return new Code() { Text = code.ToString() };
                }
                else
                {
                    code.AppendLine(line);
                    return null;
                }
            }
            else if (line.StartsWith("@"))
            {
                if (line.StartsWith("@{.}"))
                {
                    return new Bullet() { Text = line.Substring(5) };
                }
                else if (line == "@{ex}")
                {
                    return new Example();
                }
                else if (line == "@<")
                {
                    isInCode = true;
                    code = new StringBuilder();
                    return null;
                }
                else if (line.StartsWith("@{image}"))
                {
                    return new Image() { FileName = line.Substring(8).Trim() };
                }
                else
                {
                    return new Code() { Text = line.Substring(1).Trim() };
                }
            }
            else if (string.IsNullOrEmpty(line))
            {
                return new EmptyLine();
            }
            else
            {
                return new Paragraph() { Text = line };
            }
        }
    }
}
