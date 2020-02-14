using Mathy.Utils.Dandelion.Collections;

namespace Mathy.Maths.Table
{
    static class T
    {
        static T()
        {
            table = SearchTable<int, int, float>.Load(MathyContext.PathResolver.GetTablesFilePath("t.txt"));
        }


        private static SearchTable<int, int, float> table;

        public static float GetValue(int alpha, int n)
        {
            if (n > 120)
            {
                n = 121;
            }


            return table[n, alpha];
        }
    }
}
