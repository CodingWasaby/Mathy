using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mathy.Graphing
{
    public class LineRenderer : GraphRenderer
    {
        public override void Draw(Graphics g, IEnumerable<GraphPoint> points, SerialStyle style)
        {
            GraphicsPath path = new GraphicsPath();

            GraphPoint prevPoint = null;

            foreach (GraphPoint point in points)
            {
                if (prevPoint == null)
                {
                    prevPoint = point;
                }
                else
                {
                    path.AddLine(new Point((int)prevPoint.X, (int)prevPoint.Y[0]), new Point((int)point.X, (int)point.Y[0]));
                    prevPoint = point;
                }
            }


            g.DrawPath(DataSource.Pen, path);


            path.Dispose();
        }
    }
}
