using System.Collections.Generic;
using System.Drawing;

namespace Mathy.Graphing
{
    public class ScatteredPointRenderer : GraphRenderer
    {
        public override void Draw(Graphics g, IEnumerable<GraphPoint> points, SerialStyle style)
        {
            foreach (GraphPoint point in points)
            {
                PointRenderer.DrawPoint(g, (int)point.X, (int)point.Y[0], style.PointRadius, style.PointRotateAngle, DataSource.Pen, DataSource.Brush, style.PointStyle);
            }
        }
    }
}
