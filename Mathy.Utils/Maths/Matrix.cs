using Mathy.Utils.Dandelion.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathy.Maths
{
    public class Matrix
    {
        public Matrix(int rowCount, int columnCount)
            : this(rowCount, columnCount, new double[rowCount * columnCount])
        {
        }

        public Matrix(int rowCount, int columnCount, double[] items)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            Items = items;
        }


        public int RowCount { get; private set; }

        public int ColumnCount { get; private set; }


        public double[] Items { get; private set; }


        public IEnumerable<double[]> Rows
        {
            get
            {
                for (int i = 0; i <= RowCount - 1; i++)
                {
                    double[] cells = new double[ColumnCount];

                    for (int j = 0; j <= ColumnCount - 1; j++)
                    {
                        cells[j] = this[i, j];
                    }


                    yield return cells.Where(c => !double.IsNaN(c)).ToArray();
                }
            }
        }


        public double this[int row, int column]
        {
            get
            {
                return Items[row * ColumnCount + column];
            }
            set
            {
                Items[row * ColumnCount + column] = value;
            }
        }


        public Vector ToVector()
        {
            if (RowCount == 1)
            {
                Vector vector = new Vector(ColumnCount);
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    vector[i] = this[0, i];
                }

                return vector;
            }
            else if (ColumnCount == 1)
            {
                Vector vector = new Vector(RowCount);
                for (int i = 0; i <= RowCount - 1; i++)
                {
                    vector[i] = this[i, 0];
                }

                return vector;
            }

            throw new Exception(string.Format("Cannot convert a matrix of {0}*{1} to a vector.", RowCount, ColumnCount));
        }


        public double GetAverage()
        {
            return Items.Where(i => !double.IsNaN(i)).Average();
        }

        public double GetSum()
        {
            return Items.Where(i => !double.IsNaN(i)).Sum();
        }

        public object Add(Matrix matrix)
        {
            if (RowCount != matrix.RowCount || ColumnCount != matrix.ColumnCount)
            {
                throw new Exception("Cannot add two matrixes not of the same size.");
            }


            return new Matrix(RowCount, ColumnCount)
            {
                Items = Items.Select((i, index) => i + matrix.Items[index]).ToArray()
            };
        }

        public Matrix Add(double d)
        {
            return new Matrix(RowCount, ColumnCount, Items.Select(i => i + d).ToArray());
        }

        public Matrix Subtract(double d)
        {
            return Add(-d);
        }

        public object GetNegative()
        {
            return new Matrix(RowCount, ColumnCount, Items.Select(i => -i).ToArray());
        }


        public Matrix ToNegative()
        {
            return new Matrix(RowCount, ColumnCount, Items.Select(i => -i).ToArray());
        }

        public Matrix Multiply(double d)
        {
            return new Matrix(RowCount, ColumnCount, Items.Select(i => i * d).ToArray());
        }

        public Matrix Multiply(Matrix m)
        {
            if (m.ColumnCount != this.ColumnCount || m.RowCount != this.RowCount)
            {
                throw new Exception("The two matrixes has differen size.");
            }
            else
            {
                var mt = new Matrix(RowCount, ColumnCount);
                for (int r = 0; r <= RowCount - 1; r++)
                {
                    for (int c = 0; c <= ColumnCount - 1; c++)
                    {
                        mt[r, c] = this[r, c] * m[r, c];
                    }
                }
                return mt;
            }
            //throw new Exception("The evaluation of the product of two matrixes has not been implemented.");
        }

        public Matrix Divide(Matrix m)
        {
            if (m.ColumnCount != this.ColumnCount || m.RowCount != this.RowCount)
            {
                throw new Exception("The two matrixes has differen size.");
            }
            else
            {
                var mt = new Matrix(RowCount, ColumnCount);
                for (int r = 0; r <= RowCount - 1; r++)
                {
                    for (int c = 0; c <= ColumnCount - 1; c++)
                    {
                        mt[r, c] = this[r, c] / m[r, c];
                    }
                }
                return mt;
            }
            //throw new Exception("The evaluation of the product of two matrixes has not been implemented.");
        }

        public Matrix Pow(double p)
        {
            return new Matrix(RowCount, ColumnCount, Items.Select(i => Math.Pow(i, p)).ToArray());
        }

        public Matrix ElementInvert()
        {
            return new Matrix(RowCount, ColumnCount, Items.Select(i => 1 / i).ToArray());
        }


        public string GetString(int decimalDigitCount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("[");

            for (int row = 0; row <= RowCount - 1; row++)
            {
                for (int column = 0; column <= ColumnCount - 1; column++)
                {
                    double value = this[row, column];

                    string valueString;

                    if (double.IsNaN(value))
                    {
                        valueString = "-";
                    }
                    else if (decimalDigitCount >= 0)
                    {
                        valueString = NumberFormatter.ForDecimalDigits(decimalDigitCount).ToText(value);
                    }
                    else
                    {
                        valueString = value.ToString();
                    }


                    b.Append(valueString).Append(column == ColumnCount - 1 ? "; " : ", ");
                }
            }


            string s = b.ToString();

            if (s.EndsWith("; "))
            {
                s = s.Substring(0, s.Length - 2);
            }


            return s + "]";
        }

        public override string ToString()
        {
            return GetString(-1);
        }

        public static Matrix Parse(string s)
        {
            if (s == null)
            {
                throw new Exception("Argument is null.");
            }


            s = s.Replace("\r\n", string.Empty);

            if (s.StartsWith("[") != s.EndsWith("]"))
            {
                throw new Exception("Matrix bracket does not match.");
            }

            if (s.StartsWith("["))
            {
                s = s.Substring(1, s.Length - 2);
            }


            List<List<double>> items = new List<List<double>>();

            foreach (string row in s.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                items.Add(new List<double>());

                foreach (string item in row.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()))
                {
                    double value = item == "-" ? double.NaN : double.Parse(item);

                    items.Last().Add(value);
                }
            }


            if (items.Count > 0)
            {
                if (!items.Skip(1).All(i => i.Count == items[0].Count))
                {
                    throw new Exception("Each row must have the same count of elements.");
                }
            }


            List<double> elements = new List<double>();

            foreach (List<double> row in items)
            {
                foreach (double item in row)
                {
                    elements.Add(item);
                }
            }


            return new Matrix(items.Count, items.Count == 0 ? 0 : items[0].Count, elements.ToArray());
        }

        public Matrix Transpose()
        {
            Matrix m = new Matrix(ColumnCount, RowCount);

            for (int i = 0; i <= RowCount - 1; i++)
            {
                for (int j = 0; j <= ColumnCount - 1; j++)
                {
                    m[j, i] = this[i, j];
                }
            }


            return m;
        }

        public void SetDecimalDigitCount(int decimalDigitCount)
        {
            Items = Items.Select(i => Math.Round(i, decimalDigitCount)).ToArray();
        }

        public Matrix Clone()
        {
            return new Matrix(RowCount, ColumnCount, Items.ToArray());
        }

        public double[] GetRow(int rowIndex)
        {
            double[] items = new double[ColumnCount];

            for (int columnIndex = 0; columnIndex <= ColumnCount - 1; columnIndex++)
            {
                items[columnIndex] = this[rowIndex, columnIndex];
            }


            return items.Where(i => !double.IsNaN(i)).ToArray();
        }

        public double[] GetColumn(int columnIndex)
        {
            double[] items = new double[RowCount];

            for (int rowIndex = 0; rowIndex <= RowCount - 1; rowIndex++)
            {
                items[rowIndex] = this[rowIndex, columnIndex];
            }


            return items.Where(i => !double.IsNaN(i)).ToArray();
        }

        public Matrix RemoveRow(int index)
        {
            Matrix instance = new Matrix(RowCount - 1, ColumnCount);

            int rowIndex = 0;

            for (int i = 0; i <= index - 1; i++)
            {
                for (int columnIndex = 0; columnIndex <= ColumnCount - 1; columnIndex++)
                {
                    instance[rowIndex, columnIndex] = this[i, columnIndex];
                }

                rowIndex++;
            }

            for (int i = index + 1; i <= RowCount - 1; i++)
            {
                for (int columnIndex = 0; columnIndex <= ColumnCount - 1; columnIndex++)
                {
                    instance[rowIndex, columnIndex] = this[i, columnIndex];
                }

                rowIndex++;
            }


            return instance;
        }

        public Matrix GetSubMatrix(int columnCount)
        {
            Matrix m = new Matrix(RowCount, columnCount);

            for (int i = 0; i <= RowCount - 1; i++)
            {
                for (int j = 0; j <= columnCount - 1; j++)
                {
                    m[i, j] = this[i, j];
                }
            }

            return m;
        }
    }
}
