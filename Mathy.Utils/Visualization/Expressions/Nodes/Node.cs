using System;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public abstract class Node
    {
        public float DesiredWidth { get; private set; }

        public float DesiredHeight { get; private set; }


        public float Left { get; private set; }

        public float Top { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }


        public void Measure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            OnMeasure(widthSpec, heightSpec, g, font, style);
        }

        public void Layout(Node parentNode, float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            Left = (parentNode == null ? 0 : parentNode.Left) + left;
            Top = (parentNode == null ? 0 : parentNode.Top) + top;
            Width = width;
            Height = height;
            OnLayout(left, top, width, height, g, font, style);
        }

        public void Draw(Graphics g, Font font, Style style)
        {
            // g.Clip = new Region(new Rectangle(Left, Top, Width, Height));
            OnDraw(g, font, style);
            //   g.Clip = new Region();
        }


        protected abstract void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style);

        protected abstract void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style);

        protected abstract void OnDraw(Graphics g, Font font, Style style);


        protected void SetDesiredSize(float width, float height)
        {
            DesiredWidth = width;
            DesiredHeight = height;
        }


        protected static float GetChildSize(MeasureSpec spec, float size)
        {
            if (spec.Mode == MeasureSpecMode.Fixed)
            {
                return spec.Size;
            }
            else if (spec.Mode == MeasureSpecMode.AtMost)
            {
                return Math.Min(size, spec.Size);
            }
            else
            {
                return size;
            }
        }
    }
}
