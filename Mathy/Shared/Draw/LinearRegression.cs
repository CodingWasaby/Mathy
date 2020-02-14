using System;
using System.Collections.Generic;

namespace Mathy.Shared.Draw
{
    public class LinearRegression
    {
        public List<Tuple<double, double>> Datas { get; set; }
        public double K { get; set; }
        public double B { get; set; }
    }
}
