using Mathy.Planning;

namespace Mathy.Client.Model
{
    public class Strings
    {
        public static string Of(SourceVariable variable)
        {
            DataType type = variable.Type;

            if (type == DataType.Number)
            {
                return "数值";
            }
            else if (type == DataType.String)
            {
                return "字符串";
            }
            else if (type == DataType.Matrix)
            {
                return "矩阵";
            }
            else if (type == DataType.Vector)
            {
                string s = "向量";
            }


            return null;
        }
    }
}