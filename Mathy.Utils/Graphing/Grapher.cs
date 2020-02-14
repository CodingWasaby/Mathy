using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Mathy.Graphing
{
    public class Grapher
    {
        public GraphDataSource[] DataSources { get; set; }

        public Inset Margin { get; set; }

        public Inset Padding { get; set; }

        public GraphStyle Style { get; set; }

        public GridLineComputeMethod XGridLineComputeMethod { get; set; }

        public GridLineComputeMethod YGridLineComputeMethod { get; set; }


        public Bitmap Draw(int width, int height)
        {
            double x1;
            double y1;
            double x2;
            double y2;

            DataSources[0].GetCoordinationBounds(out x1, out y1, out x2, out y2);

            foreach (GraphDataSource dataSource in DataSources.Skip(1))
            {
                double tx1;
                double ty1;
                double tx2;
                double ty2;

                dataSource.GetCoordinationBounds(out tx1, out ty1, out tx2, out ty2);

                if (x1 > tx1)
                {
                    x1 = tx1;
                }

                if (y1 > ty1)
                {
                    y1 = ty1;
                }

                if (x2 < tx2)
                {
                    x2 = tx2;
                }

                if (y2 < ty2)
                {
                    y2 = ty2;
                }
            }


            int left = Margin.Left + Padding.Left;
            int top = Margin.Top + Padding.Top;
            int right = width - Margin.Right - Padding.Right;
            int bottom = height - Margin.Bottom - Padding.Bottom;

            CoordinationConverter converter = new CoordinationConverter(x1, y1, x2, y2, left, top, right, bottom);


            foreach (GraphDataSource dataSource in DataSources)
            {
                if (dataSource is ContinousDataSource)
                {
                    ContinousDataSource cds = dataSource as ContinousDataSource;
                    cds.Left = left;
                    cds.Right = right;
                    cds.Converter = converter;
                }
            }


            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            DrawCoordinationSystem(g, converter, width, height, x1, y1, x2, y2);

            foreach (GraphDataSource dataSource in DataSources)
            {
                foreach (GraphRenderer renderer in dataSource.Renderers)
                {
                    renderer.Draw(g, dataSource.GetPoints().Select(i => new GraphPoint() { X = converter.ToPixelX(i.X), Y = i.Y.Select(j => (float)converter.ToPixelY(j)).ToArray() }), dataSource.Style);
                }
            }

            g.Dispose();


            return bitmap;
        }

        private void DrawCoordinationSystem(Graphics g, CoordinationConverter converter, int width, int height, double x1, double y1, double x2, double y2)
        {
            Rectangle rect = new Rectangle(Margin.Left, Margin.Top, width - Margin.Left - Margin.Right, height - Margin.Top - Margin.Bottom);

            Brush backgroundBrush = new LinearGradientBrush(new Point(rect.Left, rect.Top), new Point(rect.Right, rect.Bottom), Style.StartColor, Style.EndColor);
            g.FillRectangle(backgroundBrush, rect);
            backgroundBrush.Dispose();

            Font font = new Font("宋体", 10);


            Pen p = new Pen(Brushes.LightGray);

            g.DrawRectangle(p, rect);


            XGridLineComputeMethod.Margin = Margin;
            YGridLineComputeMethod.Margin = Margin;

            foreach (double x in XGridLineComputeMethod.GetCoordinateAlignedPositions(x1, x2, width, height, true, converter))
            {
                string s = x.ToString();
                int left = (int)converter.ToPixelX(x);
                g.DrawLine(p, left, Margin.Top, left, height - Margin.Bottom);
                g.DrawString(s, font, Brushes.Black, left - g.MeasureString(s, font).Width / 2, height - Margin.Bottom);
            }

            foreach (double y in YGridLineComputeMethod.GetCoordinateAlignedPositions(y1, y2, width, height, false, converter))
            {
                string s = y.ToString();
                SizeF size = g.MeasureString(s, font);
                int top = (int)converter.ToPixelY(y);
                g.DrawLine(p, Margin.Left, top, width - Margin.Right, top);
                g.DrawString(s, font, Brushes.Black, Margin.Left - size.Width, top - size.Height / 2);
            }

            p.Dispose();
        }
    }
}
