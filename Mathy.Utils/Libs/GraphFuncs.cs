using Mathy.Graphing;
using Mathy.Language;
using Mathy.Maths;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Mathy.Libs
{
    public class GraphFuncs
    {
        public static Color ParseColor(string s)
        {
            int r = int.Parse(s.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(s.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(s.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            return Color.FromArgb(255, r, g, b);
        }

        public static DiscreteDataSource points(Matrix matrix, string color)
        {
            KeyValuePair<double, double>[] points = new KeyValuePair<double, double>[matrix.ColumnCount];

            for (int i = 0; i <= matrix.ColumnCount - 1; i++)
            {
                points[i] = new KeyValuePair<double, double>(matrix[0, i], matrix[1, i]);
            }


            return new DiscreteDataSource() { Data = points, Color = ParseColor(color) };
        }

        public static ExpressionDataSource expression(double from, double to, VariableContextExpression expression, string color)
        {
            return new ExpressionDataSource()
            {
                From = from,
                To = to,
                Expression = expression,
                Color = ParseColor(color)
            };
        }

        public static CandleStickDataSource candleStick(Matrix m, string color)
        {
            return new CandleStickDataSource()
            {
                Matrix = m,
                Color = ParseColor(color)
            };
        }


        public static Bitmap draw(GraphDataSource[] dataSources, Dictionary<string, object> options, double width, double height)
        {
            Grapher grapher = new Grapher();
            grapher.Margin = new Inset(20);
            grapher.Padding = new Inset(20);
            grapher.DataSources = dataSources;
            grapher.Style = new GraphStyle()
            {
                StartColor = Color.LightBlue,
                EndColor = Color.LightCoral
            };


            if (options.ContainsKey("xGridLine"))
            {
                Array gridLine = options["xGridLine"] as Array;
                grapher.XGridLineComputeMethod = new SpecifiedGridLineComputeMethod()
                {
                    From = Convert.ToDouble(gridLine.GetValue(0)),
                    To = Convert.ToDouble(gridLine.GetValue(1)),
                    GridLength = Convert.ToDouble(gridLine.GetValue(2))
                };
            }
            else
            {
                grapher.XGridLineComputeMethod = new AutoGridLineComputeMethod();
            }

            if (options.ContainsKey("yGridLine"))
            {
                Array gridLine = options["yGridLine"] as Array;
                grapher.YGridLineComputeMethod = new SpecifiedGridLineComputeMethod()
                {
                    From = Convert.ToDouble(gridLine.GetValue(0)),
                    To = Convert.ToDouble(gridLine.GetValue(1)),
                    GridLength = Convert.ToDouble(gridLine.GetValue(2))
                };
            }
            else
            {
                grapher.YGridLineComputeMethod = new AutoGridLineComputeMethod();
            }

            if (options.ContainsKey("margin"))
            {
                Array margin = options["margin"] as Array;
                grapher.Margin = new Inset((int)margin.GetValue(0), (int)margin.GetValue(1), (int)margin.GetValue(2), (int)margin.GetValue(3));
            }

            if (options.ContainsKey("padding"))
            {
                Array padding = options["padding"] as Array;
                grapher.Padding = new Inset((int)padding.GetValue(0), (int)padding.GetValue(1), (int)padding.GetValue(2), (int)padding.GetValue(3));
            }

            return grapher.Draw((int)width, (int)height);
        }
    }
}
