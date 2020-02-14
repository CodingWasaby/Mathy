using Mathy.Tree;
using System.Collections.Generic;
using System.Drawing;

namespace Mathy.Visualization.Tree
{
    public class ExpressionTreeVisualizer
    {
        public static Bitmap VisulizeAsBitmap(ExpressionTree expressionTree, Dictionary<string, object> variables)
        {
            Bitmap bitmap = new Bitmap(expressionTree.Width, expressionTree.Height);

            Graphics g = Graphics.FromImage(bitmap);
            Font font = new Font("宋体", 12);

            RenderBranch primary = RenderTreeConverter.Convert(expressionTree, variables, g, font);
            primary.Draw(g, font);

            font.Dispose();
            g.Dispose();


            return bitmap;
        }
    }
}
