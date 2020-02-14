using System.Collections.Generic;
using System.Drawing;

namespace Mathy.Graphing
{
    public abstract class GraphRenderer
    {
        public GraphDataSource DataSource { get; set; }


        public abstract void Draw(Graphics g, IEnumerable<GraphPoint> points, SerialStyle style);
    }
}
