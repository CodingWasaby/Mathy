namespace Mathy.Graphing
{
    public class CoordinationConverter
    {
        private double x1;
        private double y1;
        private double x2;
        private double y2;

        private double left;
        private double top;
        private double right;
        private double bottom;

        public CoordinationConverter(double x1, double y1, double x2, double y2, double left, double top, double right, double bottom)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public int ToPixelX(double x)
        {
            return (int)(left + (right - left) * (x - x1) / (x2 - x1));
        }

        public int ToPixelY(double y)
        {
            return (int)(bottom - (bottom - top) * (y - y1) / (y2 - y1));
        }

        public double ToMathsX(int x)
        {
            return x1 + (x2 - x1) * (x - left) / (right - left);
        }

        public double ToMathsY(int y)
        {
            return y2 - (y2 - y1) * (y - top) / (bottom - top);
        }
    }
}
