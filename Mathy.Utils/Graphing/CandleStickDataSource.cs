using Mathy.Maths;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Graphing
{
    public class CandleStickDataSource : GraphDataSource
    {
        public Matrix Matrix { get; set; }


        public override void GetCoordinationBounds(out double x1, out double y1, out double x2, out double y2)
        {
            x1 = 1;
            x2 = Matrix.RowCount;
            y1 = Matrix.Rows.Min(i => i[1]);
            y2 = Matrix.Rows.Max(i => i[2]);
        }

        public override IEnumerable<GraphPoint> GetPoints()
        {
            return Matrix.Rows.Select((i, index) => new GraphPoint() { X = index + 1, Y = new float[] { (float)i[0], (float)i[1], (float)i[2] } });
        }

        protected override IEnumerable<GraphRenderer> GetPreferredRenderers()
        {
            yield return new CandleStickRenderer() { DataSource = this };
        }
    }
}
