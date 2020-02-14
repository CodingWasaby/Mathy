using System.Drawing;

namespace Mathy.Visualization.Expressions
{
    public class LevelContext
    {
        private static Brush[] brushes = new Brush[] { Brushes.Black, Brushes.Red, Brushes.DarkViolet, Brushes.DarkGreen, Brushes.Blue };


        private int index;

        public Brush GetBrush(int level)
        {
            return brushes[level % brushes.Length];
        }
    }
}
