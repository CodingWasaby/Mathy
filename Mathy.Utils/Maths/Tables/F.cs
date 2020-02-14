using Mathy.Utils.Dandelion.Collections;
using System;

namespace Mathy.Maths.Table
{
    static class F
    {
        static F()
        {
            table1 = SearchTable<int, int, float>.Load(MathyContext.PathResolver.GetTablesFilePath("f1.txt"));
            table5 = SearchTable<int, int, float>.Load(MathyContext.PathResolver.GetTablesFilePath("f5.txt"));
        }


        private static SearchTable<int, int, float> table1;
        private static SearchTable<int, int, float> table5;

        public static float GetValue(int row, int column, int alpha)
        {
            if (row > 1000)
            {
                row = 1001;
            }

            if (column > 500)
            {
                column = 501;
            }


            if (alpha == 1)
            {
                return table1[row, column];
            }
            else if (alpha == 5)
            {
                return table5[row, column];
            }
            else
            {
                throw new Exception("Alpha must be 1 or 5.");
            }
        }
    }
}
