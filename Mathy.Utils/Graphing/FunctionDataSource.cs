using System;

namespace Mathy.Graphing
{
    public class FunctionDataSource : ContinousDataSource
    {
        public Func<double, double> Function;

        protected override double Map(double x)
        {
            return Function.Invoke(x);
        }
    }
}
