using Mathy.Maths;
using Mathy.Shared.Draw;
using Mathy.Planning;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Libs
{
    public class DrawFuncs
    {
        /// <summary>
        ///  HK
        ///  每行是一组，每列为一个柱
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="H"></param>
        /// <param name="L"></param>
        /// <param name="ec"></param>
        public static void Draw_HK(Matrix matrix, double H, double L, object ec)
        {
            EvaluationContext e = ec as EvaluationContext;
            matrix.SetDecimalDigitCount(e.Settings.DecimalDigitCount);
            e.SetValueAcrossSteps("Draw_HK" + Guid.NewGuid().ToString().Replace("-", ""), new HK
            {
                Matrix = matrix,
                H = H,
                L = L
            });
        }

        /// <summary> 
        ///  比对
        /// 每行为一个点，列1 mean，列2 standardDeviation
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="ec"></param>
        public static void Draw_Comparison(Matrix matrix, object ec)
        {
            EvaluationContext e = ec as EvaluationContext;
            var com = new Comparison();
            com.Datas = new List<string>();
            foreach (var r in matrix.Rows)
            {
                com.Datas.Add(r[0] + ":" + r[1]);
            }
            e.SetValueAcrossSteps("Draw_Comparison" + Guid.NewGuid().ToString().Replace("-", ""), com);
        }

        /// <summary>
        /// 均值
        /// 数组
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="ec"></param>
        public static void Draw_MeanValue(double[] datas, double m, double h, double l, object ec)
        {
            EvaluationContext e = ec as EvaluationContext;
            datas = (datas as double[]).Where(j => !double.IsNaN(j)).Select(j => Math.Round(j, e.Settings.DecimalDigitCount)).ToArray();
            e.SetValueAcrossSteps("Draw_MeanValue" + Guid.NewGuid().ToString().Replace("-", ""), new MeanValue
            {
                Datas = string.Join(",", datas),
                H = Math.Round(h, e.Settings.DecimalDigitCount),
                L = Math.Round(l, e.Settings.DecimalDigitCount),
                M = Math.Round(m, e.Settings.DecimalDigitCount),
            });
        }


        /// <summary>
        /// 线性回归
        /// 数组
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="ec"></param>
        public static void Draw_LinearRegression(Matrix matrix, double k, double b, object ec)
        {
            var points = new List<Tuple<double, double>>();
            for (int i = 0; i < matrix.RowCount; i++)
            {
                points.Add(new Tuple<double, double>(matrix[i, 0], matrix[i, 1]));
            }
            EvaluationContext e = ec as EvaluationContext;
            LinearRegression line = new LinearRegression()
            {
                Datas = points,
                K = Math.Round(k, e.Settings.DecimalDigitCount),
                B = Math.Round(b, e.Settings.DecimalDigitCount)
            };
            e.SetValueAcrossSteps("Draw_LinearRegression" + Guid.NewGuid().ToString().Replace("-", ""), line);
        }


        /// <summary>
        ///  直方图
        /// 每行为一条线。第一条线有阴影
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="ec"></param>
        public static void Draw_Histogram(double[] datas, object ec)
        {
            EvaluationContext e = ec as EvaluationContext;
            var part = GetPartNum(datas.Count());
            var his = Draw_Histogram(datas, e.Settings.DecimalDigitCount, part);
            e.SetValueAcrossSteps("Draw_Histogram" + Guid.NewGuid().ToString().Replace("-", ""), his);
        }

        public static Histogram Draw_Histogram(double[] datas, int ddc, int partCount)
        {
            var his = new Histogram();
            his.Datas = new List<Tuple<double, double, double>>();
            var dataTemp = datas.ToList();
            var part = GetPartNum(datas.Count());
            var l = dataTemp.Min() * 0.95;
            var h = dataTemp.Max() * 1.05;
            var pand = ddc == 0 ? 1 : 1 / (10 * ddc);
            var partDis = (h - l) / part;
            for (var i = 0; i < part; i++)
            {
                var p = i > 0 ? 1 : 0;
                var _b = Math.Round(l + i * partDis, ddc) + p * pand;
                var _e = Math.Round(l + (i + 1) * partDis, ddc);
                var count = dataTemp.Where(m => m > _b && m < _e).Count();
                var rate = Math.Round((double)count / dataTemp.Count, ddc);
                his.Datas.Add(new Tuple<double, double, double>(_b, _e, rate));
            }
            return his;
        }

        private static int GetPartNum(int dataCount)
        {
            return dataCount > 12 ? 12 : dataCount;
            ////30以内
            //if (dataCount <= 30)
            //{
            //    return dataCount > 10 ? 10 : dataCount;
            //}
            ////30-999
            //else if (dataCount > 30 && dataCount < 10000)
            //{
            //    return 20;
            //}
            //else if (dataCount >= 10000)
            //{
            //    return 30;
            //}
            //return dataCount;
        }
    }
}