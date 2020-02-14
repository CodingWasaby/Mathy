using Mathy.Visualization.Text;
using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Computation
{
    class VariableNode : Node
    {
        public DecoratedText Text { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }


        public override void Measure(Graphics g, TreeStyle style)
        {
            Font f = new Font(style.Font.Name, 9);
            Text.Measure(g, style.Font, f);
            f.Dispose();

            SizeF[] lines = new SizeF[]
            {
                g.MeasureString(Description, style.Font),
                Text.Size,
                g.MeasureString(GetValueString(Value), style.ValueFont)
            };

            NodeWidth = lines.Max(i => i.Width);
            NodeHeight = lines.Sum(i => i.Height);

            Width = NodeWidth;
            Height = NodeHeight;
        }

        public override void Layout(TreeStyle style, double left, double top)
        {
            NodeLeft = left;
            NodeTop = top;
            Left = NodeLeft;
            Top = NodeTop;
        }

        public override void Draw(Graphics g, TreeStyle style)
        {
            if (style.VariableFillColor != null)
            {
                Brush b = new SolidBrush(style.VariableFillColor);
                g.FillRectangle(b, (float)NodeLeft, (float)NodeTop, (float)NodeWidth, (float)NodeHeight);
                b.Dispose();
            }

            Pen p = new Pen(new SolidBrush(style.VariableBorderColor));
            g.DrawRectangle(p, (float)NodeLeft, (float)NodeTop, (float)NodeWidth, (float)NodeHeight);
            p.Dispose();


            double y = NodeTop;

            g.DrawString(Description, style.Font, Brushes.Black, (float)NodeLeft, (float)y);
            y += g.MeasureString(Description, style.Font).Height;

            Font f = new Font(style.Font.Name, 9);
            Text.Draw(g, style.Font, f, Brushes.Black, (int)NodeLeft, (int)y);
            f.Dispose();


            y += Text.Size.Height;

            g.DrawString(GetValueString(Value), style.ValueFont, Brushes.Black, (float)NodeLeft, (float)y);
        }
    }
}
