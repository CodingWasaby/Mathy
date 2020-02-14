using System;
using System.Collections.Generic;

namespace Mathy.Graphing
{
    public abstract class GridLineComputeMethod
    {
        public Inset Margin { get; set; }

        public abstract double[] GetCoordinateAlignedPositions(double from, double to, int width, int height, bool isXAxis, CoordinationConverter converter);
    }


    public class AutoGridLineComputeMethod : GridLineComputeMethod
    {
        public override double[] GetCoordinateAlignedPositions(double from, double to, int width, int height, bool isXAxis, CoordinationConverter converter)
        {
            int power = (int)Math.Floor(Math.Max(Math.Log10(Math.Abs(from)), Math.Log10(Math.Abs(to)))) - 1;
            double p = Math.Pow(10, power);

            double startX = Math.Floor(from / p) * p;
            double endX = to;

            int pixelLength = isXAxis ? width - Margin.Left - Margin.Right : height - Margin.Top - Margin.Bottom;
            int gridCount = (int)Math.Ceiling((double)pixelLength / 75);

            double gridLength = Math.Ceiling((endX - startX) / gridCount / p) * p;


            double max = isXAxis ? converter.ToMathsX(width - Margin.Right) : converter.ToMathsY(Margin.Top);

            List<double> positions = new List<double>();
            double position = startX;
            while (position <= max)
            {
                positions.Add(position);
                position += gridLength;
            }


            return positions.ToArray();
        }
    }

    public class SpecifiedGridLineComputeMethod : GridLineComputeMethod
    {
        public double From { get; set; }

        public double To { get; set; }

        public double GridLength { get; set; }


        public override double[] GetCoordinateAlignedPositions(double from, double to, int width, int height, bool isXAxis, CoordinationConverter converter)
        {
            List<double> positions = new List<double>();
            double position = From;
            while (position <= To)
            {
                positions.Add(position);
                position += GridLength;
            }


            return positions.ToArray();
        }
    }
}
