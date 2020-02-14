using Mathy.Visualization.Expressions.Nodes;
using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Expressions
{
    public class NodeVisualizer
    {
        public NodeVisualizer(Node[] nodes)
        {
            this.nodes = nodes;
        }


        private Node[] nodes;

        public Bitmap VisulizeAsBitmap()
        {
            Style style = new Style();

            PerformLayout(style.Font, style);


            bool drawIndex = nodes.Length > 1;


            Bitmap b = new Bitmap((int)(nodes.Max(i => i.Width) + (drawIndex ? 40 : 0)), (int)(nodes.Sum(i => i.Height) + (nodes.Length - 1) * 10));
            Graphics g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            if (drawIndex)
            {
                g.TranslateTransform(40, 0, System.Drawing.Drawing2D.MatrixOrder.Append);
            }


            int index = 0;

            foreach (Node node in nodes)
            {
                node.Draw(g, style.Font, style);

                if (drawIndex)
                {
                    index++;
                    g.DrawString(string.Format("({0})", index), style.Font, Brushes.Black, -40, 0);
                }

                g.TranslateTransform(0, node.Height + 10, System.Drawing.Drawing2D.MatrixOrder.Append);
            }

            g.Dispose();

            return b;
        }

        private void PerformLayout(Font font, Style style)
        {
            Bitmap b = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(b);

            foreach (Node node in nodes)
            {
                node.Measure(MeasureSpec.MakeUnspecified(), MeasureSpec.MakeUnspecified(), g, font, style);
                node.Layout(null, 0, 0, node.DesiredWidth, node.DesiredHeight, g, font, style);
            }

            g.Dispose();
            b.Dispose();
        }
    }
}
