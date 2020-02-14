using System;
using System.Collections.Generic;

namespace Mathy.Graphing
{
    public abstract class ContinousDataSource : GraphDataSource
    {
        public double From { get; set; }

        public double To { get; set; }

        internal int Left { get; set; }

        internal int Right { get; set; }

        internal CoordinationConverter Converter { get; set; }



        protected abstract double Map(double x);


        public override void GetCoordinationBounds(out double x1, out double y1, out double x2, out double y2)
        {
            x1 = From;
            x2 = To;

            y1 = Map(x1);
            y2 = Map(x1);

            for (double x = From; x <= To; x += GetDiscreteSectionLength(From, To))
            {
                double y = Map(x);

                if (y1 > y)
                {
                    y1 = y;
                }

                if (y2 < y)
                {
                    y2 = y;
                }
            }
        }

        private double GetDiscreteSectionLength(double from, double to)
        {
            int power = (int)Math.Ceiling(Math.Max(Math.Log10(from), Math.Log10(to))) - 2;
            return Math.Pow(10, power);
        }

        public override IEnumerable<GraphPoint> GetPoints()
        {
            for (int p = Left; p <= Right; p++)
            {
                double x = Converter.ToMathsX(p);
                if (x >= From && x <= To)
                {
                    yield return new GraphPoint() { X = (float)x, Y = new float[] { (float)Map(x) } };
                }
                else if (x > To)
                {
                    break;
                }
            }
        }

        protected override IEnumerable<GraphRenderer> GetPreferredRenderers()
        {
            yield return new LineRenderer() { DataSource = this };
        }
    }
}
