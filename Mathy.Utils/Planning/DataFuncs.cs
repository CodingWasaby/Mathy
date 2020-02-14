using Mathy.Maths;

namespace Mathy.Planning
{
    class DataFuncs
    {
        public static object CreateDefaultValue(DataType dataType)
        {
            if (dataType == DataType.Number)
            {
                return (double)0;
            }
            else if (dataType == DataType.String)
            {
                return string.Empty;
            }
            else if (dataType == DataType.Vector)
            {
                return new Vector(0);
            }
            else if (dataType == DataType.Array)
            {
                return new double[0];
            }
            else
            {
                return new Matrix(0, 0);
            }
        }

        public static object DeserializeValue(DataType type, string s)
        {
            if (type == DataType.Number)
            {
                return double.Parse(s);
            }
            else if (type == DataType.String)
            {
                return s;
            }
            else if (type == DataType.Vector)
            {
                return Matrix.Parse(s).ToVector();
            }
            else if (type == DataType.Vector)
            {
                return Matrix.Parse(s).ToVector().Items;
            }
            else
            {
                return Matrix.Parse(s);
            }
        }
    }
}
