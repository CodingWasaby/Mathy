using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mathy.Graphing
{
    class PointRenderer
    {
        private static Dictionary<PointRenderStyle, GraphicsPath> prototypes = new Dictionary<PointRenderStyle, GraphicsPath>();

        static PointRenderer()
        {
            prototypes.Add(PointRenderStyle.Circle, CreateCirclePath());
            prototypes.Add(PointRenderStyle.Square, CreateSquarePath());
            prototypes.Add(PointRenderStyle.Cross, CreateCrossPath());
        }


        private static GraphicsPath CreateCirclePath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(-10, -10, 20, 20);
            return path;
        }

        private static GraphicsPath CreateSquarePath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new Rectangle(-10, -10, 20, 20));
            return path;
        }

        private static GraphicsPath CreateCrossPath()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(-10, 0, 10, 0);
            path.AddLine(0, -10, 0, 10);
            return path;
        }


        public static void DrawPoint(Graphics g, int x, int y, int radius, int rotateAngle, Pen pen, Brush brush, PointRenderStyle style)
        {
            GraphicsState state = g.Save();

            g.ScaleTransform((float)radius / 10, (float)radius / 10);
            g.RotateTransform(rotateAngle, MatrixOrder.Append);
            g.TranslateTransform(x, y, MatrixOrder.Append);

            GraphicsPath path = prototypes[style];

            if (pen != null)
            {
                g.DrawPath(pen, path);
            }

            if (brush != null)
            {
                g.FillPath(brush, path);
            }


            g.Restore(state);
        }
    }
}
