using Mathy.Maths;
using System;
using System.Linq;

namespace Mathy.Libs
{
    public class MatrixFuncs
    {
        public static Matrix tomatrix(double[] a)
        {
            Matrix m = new Matrix(1, a.Length);
            for (int i = 0; i <= a.Length - 1; i++)
            {
                m[0, i] = a[i];
            }

            return m;
        }

        public static Matrix pow(Matrix m, double p)
        {
            return m.Pow(p);
        }

        public static Vector pow(Vector v, double p)
        {
            return v.Pow(p);
        }

        public static double med(double[] arr)
        {
            var a = arr.OrderBy(m => m).ToArray();
            if (a.Count() == 0)
                return 0;
            var center = a.Count() % 2;
            if (center > 0)
            {
                return a[(a.Count() - 1) / 2];
            }
            else
            {
                return (a[a.Count() / 2 - 1] + a[a.Count() / 2]) / 2;
            }
        }

        public static double med(Vector v)
        {
            return med(v.Items);
        }

        public static int size(Matrix m)
        {
            return m.RowCount * m.ColumnCount;
        }

        public static int size(Vector v)
        {
            return v.Size;
        }

        public static double[] darray(object[] items)
        {
            double[] values = new double[items.Length];

            for (int i = 0; i <= items.Length - 1; i++)
            {
                values[i] = (double)items[i];
            }


            return values;
        }

        public static double size(double[] items)
        {
            return items.Length;
        }

        public static double rowCount(Matrix m)
        {
            return m.RowCount;
        }

        public static double columnCount(Matrix m)
        {
            return m.ColumnCount;
        }

        public static Matrix transpose(Matrix m)
        {
            return m.Transpose();
        }

        public static double[] row(Matrix m, int index)
        {
            return m.GetRow(index);
        }

        public static double[] column(Matrix m, int index)
        {
            return m.GetColumn(index);
        }

        public static double[] array(Matrix m)
        {
            return m.Items.Where(i => !double.IsNaN(i)).ToArray();
        }

        public static double[] array(Vector v)
        {
            return v.Items.Where(i => !double.IsNaN(i)).ToArray();
        }

        public static Matrix concatColumn(Matrix[] matrixList)
        {
            if (matrixList.Length == 0)
            {
                throw new Exception("Array must contain at list one matrix.");
            }


            Matrix m = new Matrix(matrixList[0].RowCount, matrixList.Sum(i => i.ColumnCount));

            for (int row = 0; row <= matrixList[0].RowCount - 1; row++)
            {
                int column = 0;

                foreach (Matrix item in matrixList)
                {
                    for (int i = 0; i <= item.ColumnCount - 1; i++)
                    {
                        m[row, column] = item[row, i];
                        column++;
                    }
                }
            }


            return m;
        }
    }
}
