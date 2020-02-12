namespace Cherimoya.Evaluation
{
    class MathSnippets
    {
        public static object Add(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 + (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 + (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 + (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 + (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 + (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 + (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 + (byte)t2;
            }

            return null;
        }

        public static object Subtract(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 - (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 - (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 - (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 - (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 - (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 - (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 - (byte)t2;
            }

            return null;
        }

        public static object Multiply(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 * (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 * (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 * (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 * (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 * (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 * (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 * (byte)t2;
            }

            return null;
        }

        public static object Divide(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 / (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 / (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 / (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 / (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 / (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 / (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 / (byte)t2;
            }

            return null;
        }

        public static bool LessThan(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 < (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 < (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 < (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 < (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 < (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 < (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 < (byte)t2;
            }

            return false;
        }

        public static bool LessEqualThan(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 <= (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 <= (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 <= (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 <= (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 <= (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 <= (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 <= (byte)t2;
            }

            return false;
        }

        public static bool GreaterThan(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 > (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 > (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 > (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 > (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 > (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 > (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 > (byte)t2;
            }

            return false;
        }

        public static bool GreaterEqualThan(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 >= (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 >= (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 >= (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 >= (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 >= (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 >= (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 >= (byte)t2;
            }

            return false;
        }

        public static bool Equal(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 == (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 == (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 == (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 == (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 == (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 == (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 == (byte)t2;
            }

            return false;
        }

        public static bool NotEqual(object t1, object t2, int numberClassIndex)
        {
            if (numberClassIndex == 0)
            {
                return (double)t1 != (double)t2;
            }
            else if (numberClassIndex == 1)
            {
                return (float)t1 != (float)t2;
            }
            else if (numberClassIndex == 2)
            {
                return (decimal)t1 != (decimal)t2;
            }
            else if (numberClassIndex == 3)
            {
                return (long)t1 != (long)t2;
            }
            else if (numberClassIndex == 4)
            {
                return (int)t1 != (int)t2;
            }
            else if (numberClassIndex == 5)
            {
                return (short)t1 != (short)t2;
            }
            else if (numberClassIndex == 7)
            {
                return (byte)t1 != (byte)t2;
            }

            return false;
        }
    }

}