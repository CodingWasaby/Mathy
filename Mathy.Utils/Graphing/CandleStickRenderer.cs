using System.Collections.Generic;
using System.Drawing;

namespace Mathy.Graphing
{
    public class CandleStickRenderer : GraphRenderer
    {
        public override void Draw(Graphics g, IEnumerable<GraphPoint> points, SerialStyle style)
        {
            foreach (GraphPoint point in points)
            {
                g.DrawLine(DataSource.Pen, new Point((int)point.X, (int)point.Y[1]), new Point((int)point.X, (int)point.Y[2]));
                g.FillEllipse(DataSource.Brush, new Rectangle((int)(point.X - 3), (int)(point.Y[0] - 3), 6, 6));

                g.DrawLine(DataSource.Pen, new Point((int)point.X - 3, (int)point.Y[1]), new Point((int)point.X + 3, (int)point.Y[1]));
                g.DrawLine(DataSource.Pen, new Point((int)point.X - 3, (int)point.Y[2]), new Point((int)point.X + 3, (int)point.Y[2]));
            }
        }
    }
}
