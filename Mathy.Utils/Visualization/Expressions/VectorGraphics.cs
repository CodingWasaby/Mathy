using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mathy.Visualization.Expressions
{
    class VectorGraphics
    {
        public static void DrawRootSymbol(RectangleF rect, float handleWidth, Graphics g, Brush brush)
        {
            float x1 = rect.Left + 3;
            float x2 = rect.Left + handleWidth / 2;
            float y1 = rect.Bottom - Math.Min(40, rect.Height / 2);

            GraphicsPath path = new GraphicsPath();

            path.AddLine(rect.Left, y1, x1, y1);
            path.AddLine(x1, y1, x2, rect.Bottom);
            path.AddLine(x2, rect.Bottom, rect.Left + handleWidth, rect.Top);
            path.AddLine(rect.Left + handleWidth, rect.Top, rect.Right, rect.Top);

            Pen p = new Pen(brush);
            g.DrawPath(p, path);
            p.Dispose();

            path.Dispose();
        }

        public static void DrawAbsSymbol(RectangleF rect, Graphics g, Brush brush)
        {
            Pen p = new Pen(brush);
            g.DrawLine(p, rect.Left + 2, rect.Top + 2, rect.Left + 2, rect.Top + rect.Height - 2);
            g.DrawLine(p, rect.Left + rect.Width - 2, rect.Top + 2, rect.Left + rect.Width - 2, rect.Top + rect.Height - 2);
            p.Dispose();
        }

        public static void DrawSigmaSymbol(RectangleF rect, Graphics g, Brush brush)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(rect.Right, rect.Top + 5, rect.Right - 2, rect.Top);
            path.AddLine(rect.Right - 2, rect.Top, rect.Left, rect.Top);
            path.AddLine(rect.Left, rect.Top, rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            path.AddLine(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2, rect.Left, rect.Top + rect.Height);
            path.AddLine(rect.Left, rect.Top + rect.Height, rect.Right - 2, rect.Bottom);
            path.AddLine(rect.Right - 2, rect.Bottom, rect.Right, rect.Bottom - 5);

            Pen p = new Pen(Brushes.Black, 2);
            g.DrawPath(p, path);
            p.Dispose();

            path.Dispose();
        }

        public static void DrawArrow(RectangleF rect, Graphics g, Brush brush)
        {
            GraphicsPath path = new GraphicsPath();

            float centerY = rect.Top + rect.Height / 2;

            path.AddLine(rect.Left, centerY, rect.Right, centerY);
            path.AddLine(rect.Right - 5, centerY - 2, rect.Right, centerY);
            path.AddLine(rect.Right - 5, centerY + 2, rect.Right, centerY);

            Pen p = new Pen(brush);
            g.DrawPath(p, path);
            p.Dispose();

            path.Dispose();
        }

        public static void DrawSquareBracket(RectangleF rect, Graphics g, Brush brush)
        {
            float s = 3;

            Pen p = new Pen(brush);

            g.DrawLine(p, rect.Left, rect.Top + s, rect.Left, rect.Top + rect.Height - s);

            g.DrawLine(p, rect.Left, rect.Top + s, rect.Left + 5, rect.Top + s);
            g.DrawLine(p, rect.Left, rect.Top + rect.Height - s, rect.Left + 5, rect.Top + rect.Height - s);

            g.DrawLine(p, rect.Left + rect.Width - 1, rect.Top + s, rect.Left + rect.Width - 1, rect.Top + rect.Height - s);

            g.DrawLine(p, rect.Left + rect.Width - 5, rect.Top + s, rect.Left + rect.Width - 1, rect.Top + s);
            g.DrawLine(p, rect.Left + rect.Width - 5, rect.Top + rect.Height - s, rect.Left + rect.Width - 1, rect.Top + rect.Height - s);

            p.Dispose();

        }

        public static void DrawRoundBracket(RectangleF rect, Graphics g, Brush brush)
        {
            float s = 6;

            Pen p = new Pen(brush);

            g.DrawArc(p, new RectangleF(rect.Left, rect.Top, s, rect.Height), 90, 180);
            g.DrawArc(p, new RectangleF(rect.Right - s, rect.Top, s, rect.Height), -90, 180);

            p.Dispose();
        }
    }
}
