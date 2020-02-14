using Mathy.Utils.Dandelion.Collections;
using System;
using System.Linq;

namespace Mathy.Maths.Table
{
    static class Dagostino
    {
        static Dagostino()
        {
            table = SearchTable<int, int, string>.Load(MathyContext.PathResolver.GetTablesFilePath("dagostino.txt")).Map(i =>
            {
                string[] sections = i.Split('~').Select(j => j.Trim()).ToArray();
                return new Tuple<float, float>(float.Parse(sections[0]), float.Parse(sections[1]));
            });
        }


        private static SearchTable<int, int, Tuple<float, float>> table;

        public static Tuple<float, float> GetValue(int alpha, int n)
        {
            return table[n, alpha];
        }
    }
}
