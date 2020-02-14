using Cherimoya.Evaluation;
using Cherimoya.Expressions;
using Mathy.Language;
using Mathy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Libs
{
    public class MapReduceFuncs
    {
        public static double[] mapd(int[] items, LambdaExpression mapper, VariableContext vc)
        {
            return map(items.Select(i => (object)i).ToArray(), mapper, vc).Select(i => (double)i).ToArray();
        }

        public static double[] mapd(double[] items, LambdaExpression mapper, VariableContext vc)
        {
            return map(items.Select(i => (object)i).ToArray(), mapper, vc).Select(i => (double)i).ToArray();
        }

        public static object[] map(int[] items, LambdaExpression mapper, VariableContext vc)
        {
            return map(items.Select(i => (object)i).ToArray(), mapper, vc);
        }

        public static object[] map(double[] items, LambdaExpression mapper, VariableContext vc)
        {
            return map(items.Select(i => (object)i).ToArray(), mapper, vc);
        }

        private static object[] map(object[] items, LambdaExpression mapper, VariableContext vc)
        {
            ExpressionEvaluator evaluator = new MathyLanguageService().CreateEvaluator();

            object[] result = new object[items.Length];

            for (int i = 0; i <= items.Length - 1; i++)
            {
                vc.Set(mapper.VariableNames[0], items[i]);
                result[i] = evaluator.Evaluate(mapper.Body, vc);
            }

            vc.Remove(mapper.VariableNames[0]);
            return result;
        }

        public static Matrix map(Matrix m, LambdaExpression mapper, VariableContext vc)
        {
            ExpressionEvaluator evaluator = new MathyLanguageService().CreateEvaluator();

            List<List<double>> result = new List<List<double>>();

            double[] row = new double[m.ColumnCount];

            for (int i = 0; i <= m.RowCount - 1; i++)
            {
                for (int j = 0; j <= m.ColumnCount - 1; j++)
                {
                    row[j] = m[i, j];
                }

                vc.Set(mapper.VariableNames[0], row.Where(c => !double.IsNaN(c)).ToArray());

                if (mapper.VariableNames.Length > 1)
                {
                    vc.Set(mapper.VariableNames[1], i);
                }

                double[] mappedRow;
                var obj = evaluator.Evaluate(mapper.Body, vc);
                try
                {
                    mappedRow = (double[])obj;
                }
                catch (Exception ex)
                {
                    var a = (object[])obj;
                    mappedRow = (double[])a[0];
                }


                result.Add(new List<double>());

                for (int j = 0; j <= mappedRow.Length - 1; j++)
                {
                    result[i].Add(mappedRow[j]);
                }
            }

            vc.Remove(mapper.VariableNames[0]);


            List<double> cells = new List<double>();

            foreach (List<double> resultRow in result)
            {
                foreach (double cell in resultRow)
                {
                    cells.Add(cell);
                }
            }

            return new Matrix(m.RowCount, result[0].Count, cells.ToArray());
        }
    }
}
