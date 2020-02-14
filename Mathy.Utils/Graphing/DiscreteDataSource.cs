using System.Collections.Generic;
using System.Linq;

namespace Mathy.Graphing
{
    public class DiscreteDataSource : GraphDataSource
    {
        public KeyValuePair<double, double>[] Data { get; set; }


        public override void GetCoordinationBounds(out double x1, out double y1, out double x2, out double y2)
        {
            x1 = Data[0].Key;
            x2 = Data.Last().Key;
            y1 = Data.Min(i => i.Value);
            y2 = Data.Max(i => i.Value);
        }

        public override IEnumerable<GraphPoint> GetPoints()
        {
            return Data.Select(i => new GraphPoint() { X = (float)i.Key, Y = new float[] { (float)i.Value } });
        }

        protected override IEnumerable<GraphRenderer> GetPreferredRenderers()
        {
            yield return new ScatteredPointRenderer() { DataSource = this };
        }
    }
}
