using MathNet.Numerics.Distributions;
using System;

namespace Mathy.Libs
{
    /// <summary>
    /// 分布类 常用分布放在一个类里 方便调用和扩展
    /// </summary>
    public class Distribution
    {
        // 连续分布
        private Normal normalDis; // 正态分布
        private ContinuousUniform continuousUniformDis; // 均匀分布
        private Triangular triangularDis; // 三角分布
        private StudentT studentTDis; // T分布
        private MathNet.Numerics.Distributions.Bernoulli bernoulliDis;

        // 离散分布
        private DiscreteUniform discreteUniform; // 离散均匀分布


        private String DistributionName;

        public Distribution(String DistributionName, params double[] vs) // 构造函数 根据名字初始化分布
        {
            switch (DistributionName.ToUpper())
            {
                case "NORMAL":
                    normalDis = new Normal(vs[0], vs[1]); // double mean, double stddev
                    break;
                case "CONTINUOUS":
                    continuousUniformDis = new ContinuousUniform(vs[0], vs[1]); // int lower, int upper
                    break;
                case "TRIANGULAR":
                    triangularDis = new Triangular(vs[0], vs[1], vs[2]); //double lower, double upper, double mode  (lower ≤ mode ≤ upper)
                    break;
                case "STUDENTT":
                    studentTDis = new StudentT(vs[0], vs[1], vs[2]);//double location, double scale, double freedom
                    break;
                case "BERNOULLI":
                    bernoulliDis = new Bernoulli(vs[0]);
                    break;
                case "DISCRETEUNIFORM":
                    discreteUniform = new DiscreteUniform((int)vs[0], (int)vs[1]); // int lower, int upper
                    break;
            }
            this.DistributionName = DistributionName;
        }

        public double getSample() // 获取当前分布样本
        {
            double ret = 0;
            switch (DistributionName)
            {
                case "Normal":
                    ret = normalDis.Sample(); break;
                case "ContinuousUniform":
                    ret = continuousUniformDis.Sample(); break;
                case "Triangular":
                    ret = triangularDis.Sample(); break;
                case "StudentT":
                    ret = studentTDis.Sample(); break;
                case "DiscreteUniform":
                    ret = discreteUniform.Sample(); break;
            }
            return ret;
        }

        public double[] getSamples(int num) // 获取指定个数的样本
        {
            double[] ret = new double[num];
            int[] ret_int = new int[num];
            switch (DistributionName)
            {
                case "Normal":
                    normalDis.Samples(ret);
                    break;
                case "ContinuousUniform":
                    continuousUniformDis.Samples(ret);
                    break;
                case "Triangular":
                    triangularDis.Samples(ret);
                    break;
                case "StudentT":
                    studentTDis.Samples(ret);
                    break;
                case "DiscreteUniform":
                    discreteUniform.Samples(ret_int);
                    for (int i = 0; i < num; i++)
                    {
                        ret[i] = ret_int[i];
                    }
                    break;
            }
            return ret;
        }

        public double[] getDiscreteSection()
        { // 获取离散分布的区间
            double[] ret = new double[] { 0, 0 };
            switch (DistributionName)
            {
                case "DiscreteUniform":
                    ret = new double[] { discreteUniform.LowerBound, discreteUniform.UpperBound }; break;
            }
            return ret;
        }

        public static double[] normal(double v1, double v2, int num)
        {
            var n = new Normal(v1, v2);
            double[] ret = new double[num];
            n.Samples(ret);
            return ret;
        }

        public static double[] continuousUniform(double v1, double v2, int num)
        {
            var n = new ContinuousUniform(v1, v2);
            double[] ret = new double[num];
            n.Samples(ret);
            return ret;
        }

        public static double[] triangular(double v1, double v2, double v3, int num)
        {
            var t = new Triangular(v1, v2, v3);
            double[] ret = new double[num];
            t.Samples(ret);
            return ret;
        }

        public static double[] studentT(double v1, double v2, double v3, int num)
        {
            var t = new StudentT(v1, v2, v3);
            double[] ret = new double[num];
            t.Samples(ret);
            return ret;
        }

        public static int[] bernoulli(double v1, int num)
        {
            var t = new Bernoulli(v1);
            int[] ret = new int[num];
            t.Samples(ret);
            return ret;
        }
    }
}