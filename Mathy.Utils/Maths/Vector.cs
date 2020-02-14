using Mathy.Utils.Dandelion.Text;
using System;
using System.Linq;
using System.Text;

namespace Mathy.Maths
{
    public class Vector
    {
        public Vector(int size)
        {
            Size = size;
            Items = new double[Size];
        }

        public Vector(int size, double[] items)
        {
            Size = size;
            this.Items = items;
        }


        public int Size { get; private set; }

        public double[] Items { get; private set; }


        public double this[int index]
        {
            get
            {
                return Items[index];
            }
            set
            {
                Items[index] = value;
            }
        }

        public Vector Pow(double p)
        {
            return new Vector(Size, Items.Select(i => Math.Pow(i, p)).ToArray());
        }

        public Vector Add(double p)
        {
            return new Vector(Size, Items.Select(i => i + p).ToArray());
        }

        public Vector Subtract(double p)
        {
            return new Vector(Size, Items.Select(i => i - p).ToArray());
        }

        public Vector Multiply(double p)
        {
            return new Vector(Size, Items.Select(i => i * p).ToArray());
        }

        public Vector ElementInvert()
        {
            return new Vector(Size, Items.Select(i => 1 / i).ToArray());
        }

        public Vector GetNegative()
        {
            return new Vector(Size, Items.Select(i => -i).ToArray());
        }


        public void SetDecimalDigitCount(int decimalDigitCount)
        {
            Items = Items.Select(i => Math.Round(i, decimalDigitCount)).ToArray();
        }

        public Vector Clone()
        {
            return new Vector(Size, Items.ToArray());
        }


        public string GetString(int decimalDigitCount)
        {
            StringBuilder b = new StringBuilder();
            b.Append("[");

            for (int i = 0; i <= Size - 1; i++)
            {

                double value = Items[i];

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


                b.Append(valueString).Append(", ");
            }


            string s = b.ToString();

            if (s.EndsWith(", "))
            {
                s = s.Substring(0, s.Length - 2);
            }


            return s + "]";
        }

        public override string ToString()
        {
            return GetString(-1);
        }
    }
}
