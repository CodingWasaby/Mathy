using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class MatrixExpression : Expression
    {
        public Expression[][] Rows { get; set; }


        private float[] maxCellWidth;
        private float[] maxCellTopHeight;
        private float[] maxCellBottomHeight;


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            if (Rows.Length == 0)
            {
                SetDesiredSize(0, 0);
                return;
            }

            int rowCount = Rows.Length;
            int columnCount = Rows[0].Length;

            float spacing = 10;

            MeasureSpec cellWidth = MeasureSpec.MakeUnspecified();
            MeasureSpec cellHeight = MeasureSpec.MakeUnspecified();


            if (widthSpec.Mode == MeasureSpecMode.Fixed || widthSpec.Mode == MeasureSpecMode.AtMost)
            {
                cellWidth = MeasureSpec.MakeAtMost((widthSpec.Size - (columnCount + 1) * spacing) / columnCount);
            }

            if (heightSpec.Mode == MeasureSpecMode.Fixed || heightSpec.Mode == MeasureSpecMode.AtMost)
            {
                cellHeight = MeasureSpec.MakeAtMost((heightSpec.Size - (rowCount + 1) * spacing) / rowCount);
            }


            maxCellWidth = new float[columnCount];
            maxCellTopHeight = new float[rowCount];
            maxCellBottomHeight = new float[rowCount];

            int rowIndex = 0;

            foreach (Expression[] row in Rows)
            {
                int columnIndex = 0;

                foreach (Expression item in row)
                {
                    item.Measure(cellWidth, cellHeight, g, font, style);

                    if (maxCellWidth[columnIndex] < item.DesiredWidth)
                    {
                        maxCellWidth[columnIndex] = item.DesiredWidth;
                    }


                    float topHeight = item.FindMaxTop();
                    float bottomHeight = item.FindMaxBottom();

                    if (maxCellTopHeight[rowIndex] < topHeight)
                    {
                        maxCellTopHeight[rowIndex] = topHeight;
                    }

                    if (maxCellBottomHeight[rowIndex] < bottomHeight)
                    {
                        maxCellBottomHeight[rowIndex] = bottomHeight;
                    }


                    columnIndex++;
                }

                rowIndex++;
            }


            SetDesiredSize(
                maxCellWidth.Sum() + (columnCount + 1) * spacing,
                maxCellTopHeight.Sum() + maxCellBottomHeight.Sum() + (rowIndex + 1) * spacing);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            int rowCount = Rows.Length;
            int columnCount = Rows[0].Length;

            float spacing = 10;

            float y = spacing;

            for (int row = 0; row <= rowCount - 1; row++)
            {
                float x = spacing;

                for (int column = 0; column <= columnCount - 1; column++)
                {
                    Expression item = Rows[row][column];
                    item.Layout(this, x + (maxCellWidth[column] - item.DesiredWidth) / 2, y + maxCellTopHeight[row] - item.FindMaxTop(), item.DesiredWidth, item.DesiredHeight, g, font, style);

                    x += maxCellWidth[column] + spacing;
                }

                y += maxCellTopHeight[row] + maxCellBottomHeight[row] + spacing;
            }
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            VectorGraphics.DrawSquareBracket(new RectangleF(Left, Top, Width, Height), g, Brushes.Black);

            Pen p = new Pen(Brushes.LightGray);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            int rowCount = Rows.Length;
            int columnCount = Rows[0].Length;

            float spacing = 10;

            float y = Top + spacing;

            for (int row = 0; row <= rowCount - 1; row++)
            {
                float x = Left + spacing;

                for (int column = 0; column <= columnCount - 1; column++)
                {
                    Expression item = Rows[row][column];
                    item.Draw(g, font, style);

                    g.DrawRectangle(p, x, y, maxCellWidth[column], maxCellTopHeight[row] + maxCellBottomHeight[row]);

                    x += maxCellWidth[column] + spacing;
                }

                y += maxCellTopHeight[row] + maxCellBottomHeight[row] + spacing;
            }

            p.Dispose();
        }
    }
}
