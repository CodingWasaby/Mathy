using Mathy.Utils.Dandelion.Collections;

namespace Mathy.Maths.Table
{
    static class Grubbs
    {
        static Grubbs()
        {
            table = SearchTable<int, int, float>.Load(MathyContext.PathResolver.GetTablesFilePath("grubbs.txt"));
        }


        private static SearchTable<int, int, float> table;

        public static float GetValue(int alpha, int n)
        {
            return table[n, alpha];
        }
    }
}
