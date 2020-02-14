using System;

namespace Mathy.Libs
{
    public class GeneralFuncs
    {
        public static Tuple<string, int> pr1()
        {
            return new Tuple<string, int>("r", 12);
        }

        public static double sin(double d)
        {
            return Math.Sin(d);
        }

        public static double cos(double d)
        {
            return Math.Cos(d);
        }

        public static double tan(double d)
        {
            return Math.Tan(d);
        }

        public static double ctg(double d)
        {
            return 1 / Math.Tan(d);
        }

        public static double asin(double d)
        {
            return Math.Asin(d);
        }

        public static double acos(double d)
        {
            return Math.Acos(d);
        }

        public static double atan(double d)
        {
            return Math.Atan(d);
        }

        public static double actg(double d)
        {
            return Math.Atan(1 / d);
        }

        public static double pow(double x, double y)
        {
            return Math.Pow(x, y);
        }

        public static double root(double x, double y)
        {
            return Math.Pow(x, 1 / y);
        }

        public static double abs(double x)
        {
            return Math.Abs(x);
        }

        public static double lg(double x)
        {
            return Math.Log(x, 10);
        }

        public static double ln(double x)
        {
            return Math.Log(x, Math.E);
        }

        public static double log(double b, double x)
        {
            return Math.Log(x, b);
        }
    }
}
