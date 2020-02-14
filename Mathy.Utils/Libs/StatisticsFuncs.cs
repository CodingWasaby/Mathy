using Cherimoya.Expressions;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using Mathy.Language;
using Mathy.Maths;
using Mathy.Maths.Table;
using Mathy.Planning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Libs
{
    public class StatisticsFuncs
    {
        public static double average(double[] items)
        {
            return items.Average();
        }

        public static double sum(double[] items)
        {
            return items.Sum();
        }

        public static double sum1(double[] items)
        {
            return items.Sum();
        }

        public static double sqrvar(double[] items)
        {
            double avg = items.Average();

            return items.Sum(i => (i - avg) * (i - avg)) / (items.Length - 1);
        }

        public static double stdvar(double[] items)
        {
            return Math.Sqrt(sqrvar(items));
        }

        public static double max(double[] items)
        {
            return items.Max();
        }

        public static double min(double[] items)
        {
            return items.Min();
        }

        public static Tuple<double, double> slreg(Matrix m)
        {
            double[] x = m.Rows.First();
            double[] y = m.Rows.Skip(1).First();

            double xavg = x.Average();
            double yavg = y.Average();


            double b11 = 0;

            for (int i = 0; i <= x.Length - 1; i++)
            {
                b11 += (x[i] - xavg) * (y[i] - yavg);
            }


            double b12 = 0;

            for (int i = 0; i <= x.Length - 1; i++)
            {
                b12 += (x[i] - xavg) * (x[i] - xavg);
            }


            double b1 = b11 / b12;
            double b0 = yavg - b1 * xavg;


            return new Tuple<double, double>(b0, b1);
        }

        // http://blog.sina.com.cn/s/blog_648868460100hevs.html
        public static Tuple<double, double, double> lsp(Matrix m)
        {
            double[] x = m.Rows.First();
            double[] y = m.Rows.Skip(1).First();

            double xy = 0;
            for (int i = 0; i <= m.ColumnCount - 1; i++)
            {
                xy += x[i] * y[i];
            }

            double a = (x.Sum(i => i * i) * y.Sum() - x.Sum() * xy) / (m.ColumnCount * x.Sum(i => i * i) - Math.Pow(x.Sum(), 2));
            double b = (m.ColumnCount * xy - x.Sum() * y.Sum()) / (m.ColumnCount * x.Sum(i => i * i) - Math.Pow(x.Sum(), 2));

            double xavg = x.Average();
            double yavg = y.Average();

            double s1 = 0;
            double s2 = 0;
            double s3 = 0;
            double s4 = 0;
            for (int i = 0; i <= m.ColumnCount - 1; i++)
            {
                s1 += x[i] - xavg;
                s2 += y[i] - yavg;
                s3 += Math.Pow(x[i] - xavg, 2);
                s4 += Math.Pow(y[i] - yavg, 2);
            }

            double r = (s1 * s2) / (Math.Sqrt(s3) * Math.Sqrt(s4));


            return new Tuple<double, double, double>(a, b, r);
        }

        public static double tgrubbs(int n, int alpha)
        {
            return Grubbs.GetValue(alpha, n);
        }

        public static double[] grubbsd(double[] items, int alpha)
        {
            int n = items.Length;

            if (!(n >= 3 || n <= 100))
            {
                throw new Exception("Grubbs function: it is required that count of samples be within [3, 100].");
            }

            if (!(alpha == 1 || alpha == 5))
            {
                throw new Exception("Grubbs function: it is required that alpha be 1 or 5.");
            }


            double grubbs = Grubbs.GetValue(alpha, n);
            double average = items.Average();
            double s = stdvar(items);


            return items.Where(i => Math.Abs(i - average) > grubbs * s).ToArray();
        }

        public static double[] grubbs(double[] items, int alpha)
        {
            double[] removedItems = grubbsd(items, alpha);
            return items.Where(i => !removedItems.Contains(i)).ToArray();
        }

        public static double[] sort(double[] items)
        {
            return items.OrderBy(m => m).ToArray();
        }

        public static double[] sortd(double[] items)
        {
            return items.OrderByDescending(m => m).ToArray();
        }

        public static Vector tovector(double[] items)
        {
            return new Vector(items.Count(), items);
        }

        public static double tdixon(int n, int alpha)
        {
            return Dixon.GetValue(alpha, n);
        }

        public static Tuple<double[], double, double, double> dixond(double[] items, int alpha)
        {
            int n = items.Length;

            if (!(n >= 3 || n <= 30))
            {
                throw new Exception("Dixon function: it is required that count of samples be within [3, 30].");
            }

            if (!(alpha == 1 || alpha == 5))
            {
                throw new Exception("Dixon function: it is required that alpha be 1 or 5.");
            }


            double dixon = Dixon.GetValue(alpha, n);
            double[] x = items.OrderBy(i => i).ToArray();


            double r1;
            double rn;

            if (n <= 7)
            {
                r1 = (x[1] - x[0]) / (x[x.Length - 1] - x[0]);
                rn = (x[x.Length - 1] - x[x.Length - 2]) / (x[x.Length - 1] - x[0]);
            }
            else if (n <= 10)
            {
                r1 = (x[1] - x[0]) / (x[x.Length - 2] - x[0]);
                rn = (x[x.Length - 1] - x[x.Length - 2]) / (x[x.Length - 1] - x[1]);
            }
            else if (n <= 13)
            {
                r1 = (x[2] - x[0]) / (x[x.Length - 2] - x[0]);
                rn = (x[x.Length - 1] - x[x.Length - 3]) / (x[x.Length - 1] - x[1]);
            }
            else
            {
                r1 = (x[2] - x[0]) / (x[x.Length - 3] - x[0]);
                rn = (x[x.Length - 1] - x[x.Length - 3]) / (x[x.Length - 1] - x[2]);
            }


            bool removeFirst = r1 > rn && r1 > dixon;
            bool removeLast = rn > r1 && rn > dixon;


            List<double> removedItems = new List<double>();

            if (removeFirst)
            {
                removedItems.Add(x.First());
            }

            if (removeLast)
            {
                removedItems.Add(x.Last());
            }


            return new Tuple<double[], double, double, double>(removedItems.ToArray(), r1, rn, dixon);
        }

        public static Tuple<double[], double, double, double> dixon(double[] items, int alpha)
        {
            Tuple<double[], double, double, double> d = dixond(items, alpha);
            return new Tuple<double[], double, double, double>(items.Where(i => !d.Item1.Contains(i)).ToArray(), d.Item2, d.Item3, d.Item4);
        }

        public static double tcochran(int row, int column, int alpha)
        {
            return Cochran.GetValue(row, column, alpha);
        }

        public static Tuple<Matrix, double, double> cochran(Matrix m, int alpha)
        {
            if (!(alpha == 1 || alpha == 5))
            {
                throw new Exception("Cochran function: it is required that alpha be 1 or 5.");
            }


            double[] sqrvars = m.Rows.Select(i => sqrvar(i)).ToArray();
            double c = sqrvars.Max() / sqrvars.Sum();
            double cochran = Cochran.GetValue(m.RowCount, m.ColumnCount, alpha);

            Matrix result;

            if (c <= cochran)
            {
                result = m;
            }
            else
            {
                int index = 0;
                double max = sqrvars[0];

                for (int i = 1; i <= sqrvars.Length - 1; i++)
                {
                    if (max < sqrvars[i])
                    {
                        max = sqrvars[i];
                        index = i;
                    }
                }

                result = m.RemoveRow(index);
            }


            return new Tuple<Matrix, double, double>(result, c, cochran);
        }

        public static double tshapiroa(int n1, int n2)
        {
            return Shapiro.GetAlpha(n1, n2);
        }

        public static double tshapirow(int n, int p)
        {
            return Shapiro.GetW(n, p);
        }

        public static bool shapiro(double[] items, int p)
        {
            if (!(items.Length >= 2 || items.Length <= 50))
            {
                throw new Exception("Shapiro function: it is required that count of samples be within [2, 50].");
            }

            if (!(p == 95 || p == 99))
            {
                throw new Exception("Shapiro function: it is required that p be 95 or 99.");
            }


            items = items.OrderBy(i => i).ToArray();

            int n = (items.Length % 2) == 0 ? items.Length / 2 : (items.Length - 1) / 2;

            double s = 0;
            for (int k = 1; k <= n; k++)
            {
                s += Shapiro.GetAlpha(k, items.Length) * (items[items.Length - k] - items[k - 1]);
            }

            double avarage = items.Average();
            double w = Math.Pow(s, 2) / items.Sum(i => Math.Pow(i - avarage, 2));

            return w > Shapiro.GetW(items.Length, p);
        }

        public static Tuple<double, double> tdagostino(int n, int alpha)
        {
            Tuple<float, float> result = Dagostino.GetValue(alpha, n);
            return new Tuple<double, double>(result.Item1, result.Item2);
        }

        public static bool dagostino(double[] items, int alpha)
        {
            if (!(items.Length >= 1 && items.Length <= 1000))
            {
                throw new Exception("Dagostino function: it is required that count of samples be within [1, 1000].");
            }

            if (!(alpha == 95 || alpha == 99))
            {
                throw new Exception("Dagostino function: it is required that p be 95 or 99.");
            }


            items = items.OrderBy(i => i).ToArray();

            int n = (items.Length % 2) == 0 ? items.Length / 2 : (items.Length - 1) / 2;


            double sum = 0;

            for (int k = 1; k <= n; k++)
            {
                sum += ((items.Length + 1) / 2 - k) * (items[items.Length - k] - items[k - 1]);
            }

            double average = items.Average();
            double m = items.Sum(i => Math.Pow(i - average, 2)) / items.Length;
            double y = Math.Sqrt(items.Length) * (sum / (Math.Pow(items.Length, 2) * Math.Sqrt(m)) - 0.28209479) / 0.02998598;

            Tuple<float, float> range = Dagostino.GetValue(alpha, items.Length);
            return y >= range.Item1 && y <= range.Item2;
        }

        public static double tf(int n1, int n2, int alpha)
        {
            return F.GetValue(n1, n2, alpha);
        }

        public static Tuple<bool, double, double, double, double, double> f(Matrix m, int alpha)
        {
            if (!(alpha == 1 || alpha == 5))
            {
                throw new Exception("F function: it is required that p be 1 or 5.");
            }


            int[] np = m.Rows.Select(i => i.Count(j => !double.IsNaN(j))).ToArray();
            int n = np.Sum();

            double x11 = m.GetSum() / n;

            double[] x = m.Rows.Select(i => i.Where(j => !double.IsNaN(j)).Average()).ToArray();


            double q1 = 0;
            for (int i = 0; i <= m.RowCount - 1; i++)
            {
                q1 += np[i] * Math.Pow(x[i] - x11, 2);
            }

            double q2 = 0;
            for (int i = 0; i <= m.RowCount - 1; i++)
            {
                double[] row = m.GetRow(i);
                for (int j = 0; j <= row.Length - 1; j++)
                {
                    q2 += Math.Pow(row[j] - x[i], 2);
                }
            }


            int v1 = m.RowCount - 1;
            int v2 = n - m.RowCount;


            double f = (q1 / v1) / (q2 / v2);
            double fValue = F.GetValue(v2, v1, alpha);
            return new Tuple<bool, double, double, double, double, double>(f < fValue, q1, q2, v1, v2, fValue);
        }

        public static Tuple<bool, double, double> t(double[] items1, double[] items2, int alpha)
        {
            if (!(alpha == 1 || alpha == 5))
            {
                throw new Exception("T function: it is required that p be 1 or 5.");
            }


            double average1 = items1.Average();
            double average2 = items2.Average();

            double s1 = 0;
            for (int i = 0; i <= items1.Length - 1; i++)
            {
                s1 += Math.Pow(items1[i] - average1, 2);
            }

            double s2 = 0;
            for (int i = 0; i <= items2.Length - 1; i++)
            {
                s2 += Math.Pow(items2[i] - average2, 2);
            }


            int n1 = items1.Length;
            int n2 = items2.Length;

            double t = (average1 - average2) / Math.Sqrt((s1 + s2) / (n1 + n2 - 2) * ((n1 + n2) * 1.0 / (n1 * n2)));


            double value = Math.Abs(t);
            double t1 = T.GetValue(alpha, n1 + n2 - 2);
            return new Tuple<bool, double, double>(value < t1, value, t1);
        }

        public static double anovau(Matrix m)
        {
            int[] np = m.Rows.Select(i => i.Count(j => !double.IsNaN(j))).ToArray();
            int n = np.Sum();

            double x11 = m.GetSum() / n;

            double[] x = m.Rows.Select(i => i.Where(j => !double.IsNaN(j)).Average()).ToArray();


            double q1 = 0;
            for (int i = 0; i <= m.RowCount - 1; i++)
            {
                q1 += np[i] * Math.Pow(x[i] - x11, 2);
            }

            double q2 = 0;
            for (int i = 0; i <= m.RowCount - 1; i++)
            {
                double[] row = m.GetRow(i);
                for (int j = 0; j <= row.Length - 1; j++)
                {
                    q2 += Math.Pow(row[j] - x[i], 2);
                }
            }


            int v1 = m.RowCount - 1;
            int v2 = n - m.RowCount;


            double s12 = n * (m.RowCount - 1) / (Math.Pow(n, 2) - np.Sum(i => Math.Pow(i, 2))) * (q1 / v1 - q2 / v2);
            return Math.Sqrt(s12);
        }

        public static VariableContextExpression ucomb(VariableContextExpression func, Matrix r, Vector u)
        {
            int count = func.Variables.Length;
            VariableContextExpression[] diffs = func.Variables.Select(i => ExpressionFuncs.diff(func, i.Name)).ToArray();


            Expression e1 = null;

            for (int i = 0; i <= count - 1; i++)
            {
                Expression right = BinaryExpression.Create(BinaryOperator.Multiply,
                            ConstantExpression.create(u[i] * u[i], 0, 0),
                            new FunctionCallExpression("pow",
                                new Expression[]
                                {
                                    diffs[i].Expression,
                                    ConstantExpression.create(2, 0, 0)
                                },
                                0,
                                0));

                e1 = e1 == null ? right : BinaryExpression.Create(BinaryOperator.Add, e1, right);
            }


            Expression e2 = null;

            for (int i = 1; i <= count - 1; i++)
            {
                for (int j = i + 1; j <= count; j++)
                {
                    Expression right = BinaryExpression.Create(BinaryOperator.Multiply,
                                ConstantExpression.create(u[i - 1] * u[j - 1] * r[i - 1, j - 1], 0, 0),
                                BinaryExpression.Create(BinaryOperator.Multiply,
                                    diffs[i - 1].Expression,
                                    diffs[j - 1].Expression));

                    e2 = e2 == null ? right : BinaryExpression.Create(BinaryOperator.Add, e2, right);
                }
            }

            e2 = BinaryExpression.Create(BinaryOperator.Multiply,
                    ConstantExpression.create(2, 0, 0),
                    e2);


            Expression e = BinaryExpression.Create(BinaryOperator.Add,
                e1,
                e2);


            VariableContext c = func.VariableContext;
            TypeCheckingContext context = new MathyLanguageService().CreateTypeCheckingContext(c);

            foreach (VariableInfo variable in func.Variables)
            {
                c.Set(variable.Name, context.CreateAutoCreatedVariableValue(variable.Type));
            }

            VariableContextExpression result = new VariableContextExpression(e, func.Variables, 0, 0) { VariableContext = func.VariableContext };
            new MathyLanguageService().CreateTypeChecker().PerformTypeChecking(result, context);

            result = new VariableContextExpression(new Cherimoya.Reduction.ExpressionReductor(new MathyLanguageService()).Reduce(e), func.Variables, 0, 0) { VariableContext = func.VariableContext };
            new MathyLanguageService().CreateTypeChecker().PerformTypeChecking(result, context);

            foreach (VariableInfo variable in func.Variables)
            {
                context.VariableContext.Remove(variable.Name);
            }


            return result;
        }

        //public static double tinv(object arg1, object arg2)
        //{
        //    double a, b;
        //    try
        //    {
        //        a = Convert.ToDouble(arg1);
        //        b = Convert.ToDouble(arg2);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    Excel.Application excel = new Excel.Application();
        //    var m = excel.WorksheetFunction.TInv(a, b);
        //    return m;
        //}

        //public static double finv(object arg1, object arg2, object arg3)
        //{
        //    double a, b, c;
        //    try
        //    {
        //        a = Convert.ToDouble(arg1);
        //        b = Convert.ToDouble(arg2);
        //        c = Convert.ToDouble(arg3);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    Excel.Application excel = new Excel.Application();
        //    var m = excel.WorksheetFunction.FInv(a, b, c);
        //    return m;
        //}

        //public static double xinv(object arg1, object arg2)
        //{
        //    double a, b;
        //    try
        //    {
        //        a = Convert.ToDouble(arg1);
        //        b = Convert.ToDouble(arg2);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    Excel.Application excel = new Excel.Application();
        //    var m = excel.WorksheetFunction.ChiInv(a, b);
        //    return m;
        //}

        //public static double quartile(object arg1, double[] arg2)
        //{
        //    double a;
        //    try
        //    {
        //        a = Convert.ToDouble(arg1);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    Excel.Application excel = new Excel.Application();
        //    var m = excel.WorksheetFunction.Quartile(arg2, Convert.ToInt32(a / 0.25));
        //    return m;
        //}

        public static bool odd(object a)
        {
            if (a is int)
            {
                var y = (int)a % 2;
                return y == 0;

            }
            else
            {
                return false;
            }
        }
        //etas的查表值
        private static double[] etas = { 1.645, 1.517, 1.444, 1.395, 1.359, 1.332, 1.310, 1.292, 1.277, 1.264 };
        //xi的查表值
        private static double[] xis = { 1.097, 1.054, 1.039, 1.032, 1.027, 1.024, 1.021, 1.019, 1.018, 1.017 };

        /// <summary>
        /// 稳健算法Qn
        /// </summary>
        /// <param name="array">为要传入的数组</param>
        /// <returns></returns>
        public static double RobustAnalysisS(double[] array, int type, int resultNum = 0, double eps = 1e-7)
        {
            int v = type == 0 ? 1 : resultNum - 1; //自由度
            double eta = etas[v - 1]; // η
            double xi = xis[v - 1]; //ξ
            double w_init = SortedArrayStatistics.Median(array);
            double w_t = w_init;
            double w_t_old;
            double psi;//ψ
            double w_t_sum;
            int step = 0;
            do
            {
                double[] tempArray = array.Clone() as double[];
                step++;
                //Console.WriteLine(step);

                w_t_sum = 0;
                psi = eta * w_t;
                // WriteLine(psi); // 输出ψ
                for (int i = 0; i < tempArray.Length; i++)
                {
                    if (tempArray[i] > psi)
                    {
                        tempArray[i] = psi;
                    }
                    w_t_sum += tempArray[i] * tempArray[i];
                }
                w_t_old = w_t;
                w_t = xi * Math.Sqrt(w_t_sum / tempArray.Length); // 计算 w*

            }
            while (Math.Abs(w_t - w_t_old) > eps); // 比较这次w*的变化

            // WriteLine(w_t); // 输出w*

            return w_t;
        }

        /// <summary>
        /// 稳健算法Qn
        /// </summary>
        /// <param name="array">为要传入的数组</param>
        /// <returns></returns>
        public static double RobustAlgorithmQn(double[] array)
        {
            int arrayLenth = array.Length;
            double[] differenceArray = new double[arrayLenth * (arrayLenth - 1) / 2];
            int counter = 0;
            for (int i = 0; i < arrayLenth; i++)
            {
                for (int j = i + 1; j < arrayLenth; j++)
                {
                    differenceArray[counter++] = Math.Abs(array[i] - array[j]);
                }
            }
            //foreach (double dd in d) {
            //    Write(dd + " ");
            //}
            //Write("\n");

            Array.Sort(differenceArray);

            int h = arrayLenth % 2 == 0 ? arrayLenth / 2 : (arrayLenth - 1) / 2;
            int k = h * (h - 1) / 2;

            double r_p = arrayLenth % 2 == 0 ? 1.0 / arrayLenth * (1.6019 + 1.0 / arrayLenth * (-2.128 - 5.172 / arrayLenth)) : 1.0 / arrayLenth * (3.6756 + 1.0 / arrayLenth * (1.965 + 1.0 / arrayLenth * (6.987 - 77.0 / arrayLenth)));
            double b_p = 1 / (r_p + 1);
            double Qn = 2.2219 * differenceArray[k - 1] * b_p; // d从0开始
            return Qn;
        }

        public static double[] OneWayANOVA(bool isBalanced, params double[][] datas)
        {
            var arrays = new ArrayList();
            foreach (var n in datas)
            {
                arrays.Add(n);
            }
            return _OneWayANOVA(arrays, isBalanced);
        }

        /// <summary>
        /// 单因素方差分析
        /// </summary>
        /// <param name="arrays">输入为多组数据 每组数据的个数不同</param>
        /// <param name="isBalanced">数据是否平衡</param>
        /// <returns>{SSe, SSa, MSe, MSa, se_2, sa_2}</returns>
        private static double[] _OneWayANOVA(ArrayList arrays, bool isBalanced = true)
        {
            double SSe, SSa, meanMean, MSe, MSa, se_2, sa_2;
            if (isBalanced)
            { // 平衡
                int arraysLength = arrays.Count; // 数据组数 a

                SSe = 0;
                double[] means = new double[arraysLength]; // 记录每组的均值
                int[] arrayLengths = new int[arraysLength]; // 记录每组的数据个数

                for (int i = 0; i < arraysLength; i++)
                {
                    var array = arrays[i];
                    double mean = (array as double[]).Mean();
                    means[i] = mean;
                    arrayLengths[i] = (array as double[]).Length;
                    foreach (var a in array as double[])
                    {
                        SSe += Math.Pow(a - mean, 2);
                    }
                }
                meanMean = means.Mean();

                SSa = 0;
                for (int i = 0; i < arraysLength; i++)
                {
                    SSa += arrayLengths[i] * Math.Pow(means[i] - meanMean, 2);
                }

                MSe = SSe / (arraysLength * (arrayLengths.Sum() - 1)); // 这里的 n 看作为 Σn_i  参考 https://baike.baidu.com/item/%E5%8D%95%E5%9B%A0%E7%B4%A0%E6%96%B9%E5%B7%AE%E5%88%86%E6%9E%90
                MSa = SSa / (arraysLength - 1);
                se_2 = MSe;
                sa_2 = (MSa - MSe) / arraysLength;
            }
            else
            { // 不平衡
                int arraysLength = arrays.Count; // 数据组数 a

                SSe = 0;
                double[] means = new double[arraysLength]; // 记录每组的均值
                int[] arrayLengths = new int[arraysLength]; // 记录每组的数据个数

                for (int i = 0; i < arraysLength; i++)
                {
                    var array = arrays[i];
                    double mean = (array as double[]).Mean();
                    means[i] = mean;
                    arrayLengths[i] = (array as double[]).Length;
                    foreach (var a in array as double[])
                    {
                        SSe += a * a - arrayLengths[i] * mean * mean;
                    }
                }
                meanMean = means.Mean();

                SSa = 0;
                for (int i = 0; i < arraysLength; i++)
                {
                    SSa += means[i] * means[i] - arrayLengths[i] * meanMean * meanMean;
                }

                // 不平衡里的MSe和MSa当作不变
                MSe = SSe / (arraysLength * (arrayLengths.Sum() - 1)); // 这里的 n 看作为 Σn_i  参考 https://baike.baidu.com/item/%E5%8D%95%E5%9B%A0%E7%B4%A0%E6%96%B9%E5%B7%AE%E5%88%86%E6%9E%90
                MSa = SSa / (arraysLength - 1);
                se_2 = MSe;
                double n2_sum = 0;
                double n_sum = 0;
                foreach (var a in arrayLengths)
                {
                    n2_sum += a * a;
                    n_sum += a;
                }
                double n0 = 1.0 / (arraysLength - 1) * (n_sum - n2_sum / n_sum);
                sa_2 = (MSa - MSe) / n0;
            }

            return new double[] { SSe, SSa, MSe, MSa, se_2, sa_2 };
        }

        public static double[] MLE(bool isBalanced, params double[][] datas)
        {
            var arrays = new ArrayList();
            foreach (var n in datas)
            {
                arrays.Add(n);
            }
            return _MLE(arrays, isBalanced);
        }

        /// <summary>
        /// 极大似然估计 （参数和方差分析一致）
        /// </summary>
        /// <param name="arrays">输入为多组数据 每组数据的个数不同</param>
        /// <param name="isBalanced">数据是否平衡</param>
        /// <returns>sigma_e_2, sigma_a_2</returns>
        public static double[] _MLE(ArrayList arrays, bool isBalanced = true)
        {
            int arraysLength = arrays.Count; // 数据组数 a
            int[] arrayLengths = new int[arraysLength];
            for (int i = 0; i < arraysLength; i++)
            {
                var array = arrays[i];
                arrayLengths[i] = (array as double[]).Length;
            }
            int n = arrayLengths.Sum();

            double[] rets = _OneWayANOVA(arrays, isBalanced); // 先进行单因素方差分析
            double SSe = rets[0], SSa = rets[1], MSe = rets[2], MSa = rets[3]; // 获取 SSe, SSa, MSe, MSa
            double sigma_e_2, sigma_a_2;

            if ((1 - 1.0 / arraysLength) * MSa >= MSe)
            {
                sigma_e_2 = MSe;
                sigma_a_2 = ((1 - 1.0 / arraysLength) * MSa - MSe) / n;

            }
            else
            {
                sigma_e_2 = (SSa + SSe) / (arraysLength * n);
                sigma_a_2 = 0;
            }

            return new double[] { sigma_e_2, sigma_a_2 };
        }

        public static double[] REML(bool isBalanced, double _sigma_a_R, params double[][] datas)
        {
            var arrays = new ArrayList();
            foreach (var n in datas)
            {
                arrays.Add(n);
            }
            return _REML(arrays, isBalanced, _sigma_a_R);
        }

        /// <summary>
        /// 限制最大似然估计（REML）
        /// </summary>
        /// <param name="arrays">输入为多组数据 每组数据的个数不同</param>
        /// <param name="isBalanced">数据是否平衡</param>
        /// <param name="_sigma_a_R">REML的中的参数，决定了最后估计值的计算方式</param>
        /// <returns></returns>
        private static double[] _REML(ArrayList arrays, bool isBalanced, double _sigma_a_R)
        {
            int arraysLength = arrays.Count; // 数据组数 a
            double sum = 0;
            int[] arrayLengths = new int[arraysLength];
            for (int i = 0; i < arraysLength; i++)
            {
                var array = arrays[i];
                arrayLengths[i] = (array as double[]).Length;
                sum += (array as double[]).Sum();
            }
            int n = arrayLengths.Sum();
            double meanALL = sum / n;

            double[] rets = _OneWayANOVA(arrays, isBalanced); // 先进行单因素方差分析
            double SSe = rets[0], SSa = rets[1], MSe = rets[2], MSa = rets[3]; // 获取 SSe, SSa, MSe, MSa
            double sigma_e_2, sigma_a_2, SST = 0;

            foreach (var array in arrays)
            { // 计算SST
                foreach (var num in array as double[])
                {
                    SST += (num - meanALL) * (num - meanALL);
                }
            }


            if (_sigma_a_R >= 0)
            {
                sigma_e_2 = MSe;
                sigma_a_2 = 1.0 / n * (MSa - MSe);
            }
            else
            {
                sigma_e_2 = SST / (arraysLength * n - 1);
                sigma_a_2 = 0;
            }
            return new double[] { sigma_e_2, sigma_a_2 };
        }

        public static double[] BayesEstimation(bool isBalanced, params double[][] datas)
        {
            var arrays = new ArrayList();
            foreach (var n in datas)
            {
                arrays.Add(n);
            }
            return _BayesEstimation(arrays, isBalanced);
        }

        /// <summary>
        /// 贝叶斯估计
        /// </summary>
        /// <param name="arrays">输入为多组数据 每组数据的个数不同</param>
        /// <param name="isBalanced">数据是否平衡</param>
        /// <returns></returns>
        public static double[] _BayesEstimation(ArrayList arrays, bool isBalanced)
        {
            int arraysLength = arrays.Count; // 数据组数 a
            int[] arrayLengths = new int[arraysLength];
            for (int i = 0; i < arraysLength; i++)
            {
                var array = arrays[i];
                arrayLengths[i] = (array as double[]).Length;
            }
            int n = arrayLengths.Sum();

            double[] rets = _OneWayANOVA(arrays, isBalanced); // 先进行单因素方差分析
            double SSe = rets[0], SSa = rets[1], MSe = rets[2], MSa = rets[3]; // 获取 SSe, SSa, MSe, MSa
            double sigma_e_2, sigma_a_2, Ve, Va, r;

            Ve = arraysLength * (n - 1);
            Va = arraysLength - 1;
            r = SSe / (SSe + SSa);

            sigma_e_2 = SSe / (Ve - 2) * SpecialFunctions.BetaRegularized(0.5 * Va, 0.5 * Ve - 1, r) / SpecialFunctions.BetaRegularized(0.5 * Va, 0.5 * Ve, r); // 参考 https://numerics.mathdotnet.com/Functions.html#Regularized-Beta
            sigma_a_2 = 1.0 / n * (
                SSa / (Va - 2) * SpecialFunctions.BetaRegularized(0.5 * Va - 1, 0.5 * Ve, r) / SpecialFunctions.BetaRegularized(0.5 * Va, 0.5 * Ve, r)
                - SSe / (Ve - 2) * SpecialFunctions.BetaRegularized(0.5 * Va, 0.5 * Ve - 1, r) / SpecialFunctions.BetaRegularized(0.5 * Va, 0.5 * Ve, r)
                );
            return new double[] { sigma_e_2, sigma_a_2 };
        }


        /// <summary>
        /// 蒙特卡洛模拟计算不确定度过程 报告结果部分 1)
        /// </summary>
        /// <param name="simulateNum">模拟次数</param>
        /// <param name="func">自定义输入量与输出量之间的函数关系</param>
        /// <param name="distribution">自定义分布</param>
        /// <returns></returns>
        public static double[] MCM_1(int simulateNum, double p, string text, object ec)
        {
            EvaluationContext e = ec as EvaluationContext;
            var textEC = GetEvaluationContext(text, e);
            textEC.Settings = e.Settings;
            var vList = e.SourceVariables;
            double[] results = new double[simulateNum];

            for (int i = 0; i < simulateNum; i++) // 进行多次模拟
            {
                foreach (var n in vList)
                {
                    if (n.Value is double[])
                    {
                        var v = (double[])n.Value;
                        if (v.Count() > i)
                        {
                            textEC.SetValueAcrossSteps(n.Key, v[i]);
                        }
                    }
                }
                textEC.Update();
                var r = textEC.SourceVariables.Last(m => !e.Variables.Contains(m.Key));
                var rv = (double)r.Value;
                if (double.IsNaN(rv) || double.IsInfinity(rv))
                {
                    // rv = 0;
                }
                else
                {
                    results[i] = rv;
                }
            }
            Array.Sort(results); // 顺序递增排序
            double mean = results.Mean(); // 平均值 作为 估计值
            double standardDeviation = results.StandardDeviation(); // 标准差作为不确定度
            mean = PandZero(mean, e.Settings.DecimalDigitCount);
            standardDeviation = PandZero(standardDeviation, e.Settings.DecimalDigitCount);
            e.SetValueAcrossSteps("MCM", results);
            double[] ySection = getSection(false, p, simulateNum, true, results); // 包含区间获取
            double yLow = ySection[0], yHigh = ySection[1]; // 包含区间的左右端点
            yLow = PandZero(yLow, e.Settings.DecimalDigitCount);
            yHigh = PandZero(yHigh, e.Settings.DecimalDigitCount);
            return new double[] { mean, standardDeviation, yLow, yHigh };
        }

        /// <summary>
        /// 蒙特卡洛模拟计算不确定度过程 报告结果部分 2)
        /// </summary>
        /// <returns></returns>
        public static double[] MCM_2(double p, int n_dig, string text, object ec)
        {
            double J, M;
            J = Math.Floor(100 / (1 - p));
            M = J > 1e4 ? J : 1e4;
            int h = 1;
            int simulateNum = (int)M;

            List<double[]> resultsList = new List<double[]>();
            List<double> meanList = new List<double>();
            List<double> standardDeviationList = new List<double>();
            List<double> yLowList = new List<double>();
            List<double> yHighList = new List<double>();

            while (true)
            {
                EvaluationContext e = ec as EvaluationContext;
                var textEC = GetEvaluationContext(text, e);
                textEC.Settings = e.Settings;
                var vList = e.SourceVariables;
                double[] results = new double[simulateNum];

                for (int i = 0; i < simulateNum; i++) // 进行多次模拟
                {
                    foreach (var n in vList)
                    {
                        if (n.Value is double[])
                        {
                            var v = (double[])n.Value;
                            if (v.Count() > i)
                            {
                                textEC.SetValueAcrossSteps(n.Key, v[i]);
                            }
                        }
                    }
                    textEC.Update();
                    var r = textEC.SourceVariables.Last(m => !e.Variables.Contains(m.Key));
                    var rv = (double)r.Value;
                    if (double.IsNaN(rv) || double.IsInfinity(rv))
                    {
                        // rv = 0;
                    }
                    else
                    {
                        results[i] = rv;
                    }
                }


                Array.Sort(results); // 顺序递增排序
                double mean = results.Mean(); // 平均值 作为 估计值
                double standardDeviation = results.StandardDeviation(); // 标准差作为不确定度
                double[] ySection = getSection(false, p, simulateNum, true, results); // 包含区间获取
                double yLow = ySection[0], yHigh = ySection[1]; // 包含区间的左右端点

                resultsList.Add(results);
                meanList.Add(mean);
                standardDeviationList.Add(standardDeviation);
                yLowList.Add(yLow);
                yHighList.Add(yHigh);


                if (h == 1) { h++; continue; } // h为1就加一

                double meanStd = meanList.StandardDeviation(); //估计值的标准差
                double standardDeviationStd = standardDeviationList.StandardDeviation(); // 不确定度的标准差
                double yLowStd = yLowList.StandardDeviation(); // 左端点的标准差
                double yHighStd = yHighList.StandardDeviation(); // 右端点的标准差

                double sum = 0;
                int cnt = 0;
                foreach (var a in resultsList)
                {
                    sum += a.Sum();
                    cnt += a.Length;
                }
                double resultsMean = sum / cnt;
                // 获取z的数值容差
                double ret = getNumLimit(resultsMean, n_dig);
                if (2 * meanStd >= ret || 2 * standardDeviationStd >= ret || 2 * yLowStd >= ret || 2 * yHighStd >= ret)
                {
                    h++; continue;
                }
                e.SetValueAcrossSteps("MCM", results);

                mean = PandZero(mean, e.Settings.DecimalDigitCount);
                resultsMean = PandZero(resultsMean, e.Settings.DecimalDigitCount);
                yLow = PandZero(yLow, e.Settings.DecimalDigitCount);
                yHigh = PandZero(yHigh, e.Settings.DecimalDigitCount);

                return new double[] { mean, resultsMean, yLow, yHigh };
            }

        }

        public static double PandZero(double d, int digits)
        {
            return Math.Round(Convert.ToDouble(d.ToString("f" + digits)), digits);
        }

        /// <summary>
        /// （待完善）获取包含区间 
        /// </summary>
        /// <returns></returns>
        private static double[] getSection(bool isDiscrete, double p, int M, bool isSymmetrical, double[] results)
        {
            if (isDiscrete)
            { // TODO 离散分布可以直接根据分布函数的离散表示来确定
                return new double[] { 0, 0 };
            }
            double qDouble = p * M;
            int qInt;
            if (qDouble - (int)qDouble == 0)
            { // 4.7.2 判断是否为整数
                qInt = (int)qDouble;
            }
            else
            {
                qInt = (int)(qDouble + 0.5);
            }

            double rDouble = (M - qInt) / 2.0;
            int rInt;
            if (rDouble - (int)rDouble == 0)
            { // 4.7.3 判断整数
                rInt = (int)rDouble;
            }
            else
            {
                rInt = (int)((M - qInt + 1.0) / 2.0);
            }

            if (!isSymmetrical)
            {
                // 4.7.4 不对称 采用最短包含区间
                double min = results[qInt - 1] - results[0]; // 最小值初始化
                int r_min = 1;
                for (int r = 1; r <= M - qInt; r++)
                {
                    if (results[r + qInt] - results[r] <= min)
                    {
                        min = results[r + qInt] - results[r];
                        r_min = r;
                    }
                }
                return new double[] { results[r_min], results[r_min + qInt] };

            }
            else
            {
                // 4.7.5 对称
                return new double[] { results[rInt], results[rInt + qInt] };

            }
        }

        /// <summary>
        /// 获取z的数值容差
        /// </summary>
        private static double getNumLimit(double z, int n_dig)
        {
            int l = (int)Math.Floor(Math.Log10(z));
            l -= n_dig - 1;
            return 0.5 * Math.Pow(10, l);
        }


        public static EvaluationContext GetEvaluationContext(string text, EvaluationContext ec)
        {
            var plan = new Plan();
            plan.Variables = ec.Variables.Select(m => new SourceVariable { Name = m, Type = DataType.Number }).ToArray();
            var e = new SourceExpression
            {
                Expression = text
            };
            plan.Expressions = new SourceExpression[1] { e };
            return plan.CreateEvaluationContext();
        }

    }
}
