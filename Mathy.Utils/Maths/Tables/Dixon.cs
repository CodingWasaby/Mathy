using Mathy.Utils.Dandelion.Collections;

namespace Mathy.Maths.Table
{
    static class Dixon
    {
        static Dixon()
        {
            table = SearchTable<int, int, float>.Load(MathyContext.PathResolver.GetTablesFilePath("dixon.txt"));
        }


        private static SearchTable<int, int, float> table;

        public static float GetValue(int alpha, int n)
        {
            return table[n, alpha];
        }
    }
}
