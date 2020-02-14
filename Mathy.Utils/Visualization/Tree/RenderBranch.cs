using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mathy.Visualization.Tree
{
    class RenderBranch
    {
        public double X1 { get; set; }

        public double Y1 { get; set; }

        public double X2 { get; set; }

        public double Y2 { get; set; }

        public double ImageX { get; set; }

        public double ImageY { get; set; }

        public Bitmap Image { get; set; }

        public double DescriptionX { get; set; }

        public double DescriptionY { get; set; }

        public string Description { get; set; }

        public RenderBranch[] Branches { get; set; }

        public bool IsVariable { get; set; }

        public void Draw(Graphics g, Font font)
        {
            Brush brush = IsVariable ? Brushes.DarkOrchid : Brushes.Black;


            Pen p = new Pen(brush);
            g.DrawLine(p, (float)X1, (float)Y1, (float)X2, (float)Y2);
            p.Dispose();


            double angle = Funcs.Atan(X1, Y1, X2, Y2);
            double d = 150 * Math.PI / 180;
            double length = 6;

            GraphicsPath path = new GraphicsPath();

            path.AddLine((float)X2, (float)Y2, (float)(X2 + length * Math.Cos(angle + d)), (float)(Y2 - length * Math.Sin(angle + d)));
            path.AddLine((float)(X2 + length * Math.Cos(angle + d)), (float)(Y2 - length * Math.Sin(angle + d)), (float)(X2 + length * Math.Cos(angle - d)), (float)(Y2 - length * Math.Sin(angle - d)));
            path.AddLine((float)(X2 + length * Math.Cos(angle - d)), (float)(Y2 - length * Math.Sin(angle - d)), (float)X2, (float)Y2);

            g.FillPath(brush, path);


            g.DrawString(Description, font, brush, (float)DescriptionX, (float)DescriptionY);

            if (Image != null)
            {
                g.DrawImage(Image, (float)ImageX, (float)ImageY);
            }


            foreach (RenderBranch branch in Branches)
            {
                branch.Draw(g, font);
            }
        }
    }
}
