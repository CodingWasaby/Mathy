using System.Drawing.Drawing2D;

namespace Mathy.Visualization.Computation
{
    class Branch
    {
        public Node Node { get; set; }

        public double LineX1 { get; set; }

        public double LineY1 { get; set; }

        public double LineX2 { get; set; }

        public double LineY2 { get; set; }

        public GraphicsPath ArrowPath { get; set; }
    }
}
