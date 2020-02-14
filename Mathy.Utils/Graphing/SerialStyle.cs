using System.Drawing;

namespace Mathy.Graphing
{
    public class SerialStyle
    {
        public PointRenderStyle PointStyle { get; set; }

        public int PointRadius { get; set; }

        public int PointRotateAngle { get; set; }

        public Pen PointBorderPen { get; set; }

        public Brush PointFillBrush { get; set; }

        public Pen LinePen { get; set; }


        public static SerialStyle Default
        {
            get
            {
                return new SerialStyle()
                {
                    LinePen = new Pen(Color.Black),
                    PointBorderPen = new Pen(Color.Black),
                    PointFillBrush = Brushes.Black,
                    PointRadius = 3,
                    PointStyle = PointRenderStyle.Circle
                };
            }
        }
    }
}
