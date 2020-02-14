namespace Mathy.Graphing
{
    public class Inset
    {
        public Inset(int size)
            : this(size, size, size, size)
        {
        }

        public Inset(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Left { get; private set; }

        public int Top { get; private set; }

        public int Right { get; private set; }

        public int Bottom { get; private set; }
    }
}
