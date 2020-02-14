using System.Collections.Generic;
using System.Drawing;

namespace Mathy.Graphing
{
    public abstract class GraphDataSource
    {
        public GraphDataSource()
        {
            Style = SerialStyle.Default;
            Renderers = new List<GraphRenderer>();
            Renderers.AddRange(GetPreferredRenderers());
        }


        public List<GraphRenderer> Renderers { get; set; }

        public SerialStyle Style { get; set; }


        private Color color;
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                Pen = new Pen(Color);
                Brush = new SolidBrush(Color);
            }
        }

        public Pen Pen { get; private set; }

        public Brush Brush { get; private set; }


        public abstract void GetCoordinationBounds(out double x1, out double y1, out double x2, out double y2);

        public abstract IEnumerable<GraphPoint> GetPoints();

        protected abstract IEnumerable<GraphRenderer> GetPreferredRenderers();
    }
}
