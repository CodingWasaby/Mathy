using Mathy.Utils.Dandelion.Collections;

namespace Mathy.Maths.Table
{
    static class Shapiro
    {
        static Shapiro()
        {
            tableAlpha = SearchTable<int, int, float>.Load(MathyContext.PathResolver.GetTablesFilePath("shapiro-alpha.txt"));
            tableW = SearchTable<int, int, float>.Load(MathyContext.PathResolver.GetTablesFilePath("shapiro-w.txt"));
        }


        private static SearchTable<int, int, float> tableAlpha;
        private static SearchTable<int, int, float> tableW;

        public static float GetAlpha(int row, int column)
        {
            return tableAlpha[row, column];
        }

        public static float GetW(int n, int p)
        {
            return tableW[n, p];
        }
    }
}
