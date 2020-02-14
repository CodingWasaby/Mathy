using System;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class BinaryExpression : Expression
    {
        public BinaryOperatorNode Operator { get; set; }

        public BinaryOperatorNode ActualOperator { get; set; }

        public Expression Left { get; set; }

        public Expression Right { get; set; }


        private bool isTwoLines;


        public override void FindMaxTop(float[] height)
        {
            if (Operator.Operator == BinaryOperator.Divide)
            {
                if (height[0] < Left.DesiredHeight)
                {
                    height[0] = Left.DesiredHeight;
                }
            }
            else if (isTwoLines)
            {
                float h;

                if (Right is BinaryExpression)
                {
                    h = Left.DesiredHeight + Operator.DesiredHeight / 2;
                }
                else
                {
                    h = Left.DesiredHeight + Math.Max(Operator.DesiredHeight / 2, Right.FindMaxTop());
                }

                if (height[0] < h)
                {
                    height[0] = h;
                }
            }
            else
            {
                Left.FindMaxTop(height);
                Right.FindMaxTop(height);
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            if (Operator.Operator == BinaryOperator.Divide)
            {
                if (height[0] < Right.DesiredHeight)
                {
                    height[0] = Right.DesiredHeight;
                }
            }
            else if (isTwoLines)
            {
                float h;

                if (Right is BinaryExpression)
                {
                    h = Right.DesiredHeight >= Operator.DesiredHeight ? Right.DesiredHeight - Operator.DesiredHeight / 2 : Operator.DesiredHeight / 2;
                }
                else
                {
                    h = Math.Max(Operator.DesiredHeight / 2, Right.FindMaxBottom());
                }

                if (height[0] < h)
                {
                    height[0] = h;
                }
            }
            else
            {
                Left.FindMaxBottom(height);
                Right.FindMaxBottom(height);
            }
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            if (Operator.Operator == BinaryOperator.Divide)
            {
                OnMeasureDivide(widthSpec, heightSpec, g, font, style);
            }
            else
            {
                OnMeasureHorizontal(widthSpec, heightSpec, g, font, style);
            }
        }

        private void OnMeasureDivide(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            MeasureSpec leftHeight = null;
            MeasureSpec rightHeight = null;

            if (heightSpec.Mode == MeasureSpecMode.Fixed)
            {
                leftHeight = MeasureSpec.MakeFixed((float)((heightSpec.Size - 2) / 2));
                rightHeight = MeasureSpec.MakeFixed((float)((heightSpec.Size - 2) / 2));
            }
            else if (heightSpec.Mode == MeasureSpecMode.AtMost)
            {
                leftHeight = MeasureSpec.MakeAtMost((float)((heightSpec.Size - 2) / 2));
                rightHeight = MeasureSpec.MakeAtMost((float)((heightSpec.Size - 2) / 2));
            }
            else if (heightSpec.Mode == MeasureSpecMode.Unspecified)
            {
                leftHeight = heightSpec;
                rightHeight = heightSpec;
            }

            Left.Measure(widthSpec, leftHeight, g, font, style);
            Right.Measure(widthSpec, rightHeight, g, font, style);


            Operator.Measure(MeasureSpec.MakeFixed(Math.Max(Left.DesiredWidth, Right.DesiredWidth) + 10), MeasureSpec.MakeFixed(2), g, font, style);


            SetDesiredSize(Math.Max(Left.DesiredWidth, Right.DesiredWidth) + 10, Left.DesiredHeight + Right.DesiredHeight + Operator.DesiredHeight);
        }

        private void OnMeasureHorizontal(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            Operator.Measure(MeasureSpec.MakeUnspecified(), MeasureSpec.MakeUnspecified(), g, font, style);
            ActualOperator.Measure(MeasureSpec.MakeUnspecified(), MeasureSpec.MakeUnspecified(), g, font, style);


            MeasureSpec leftWidth = null;
            MeasureSpec rightWidth = null;

            if (widthSpec.Mode == MeasureSpecMode.Fixed)
            {
                leftWidth = MeasureSpec.MakeFixed((float)((widthSpec.Size - Operator.DesiredWidth) / 2));
                rightWidth = MeasureSpec.MakeFixed((float)((widthSpec.Size - Operator.DesiredWidth) / 2));
            }
            else if (widthSpec.Mode == MeasureSpecMode.AtMost)
            {
                leftWidth = MeasureSpec.MakeAtMost((float)((widthSpec.Size - Operator.DesiredWidth) / 2));
                rightWidth = MeasureSpec.MakeAtMost((float)((widthSpec.Size - Operator.DesiredWidth) / 2));
            }
            else if (widthSpec.Mode == MeasureSpecMode.Unspecified)
            {
                leftWidth = widthSpec;
                rightWidth = widthSpec;
            }

            Left.Measure(leftWidth, heightSpec, g, font, style);
            Right.Measure(rightWidth, heightSpec, g, font, style);

            isTwoLines = Left.DesiredWidth > 450 || Right.DesiredWidth > 450;


            if (!isTwoLines)
            {
                SetDesiredSize(
                    Left.DesiredWidth + Right.DesiredWidth + GetOperatorWidth(),
                    Math.Max(Left.FindMaxTop(), Right.FindMaxTop()) + Math.Max(Left.FindMaxBottom(), Right.FindMaxBottom()));
            }
            else
            {
                Operator = ActualOperator;
                SetDesiredSize(
                    Math.Max(Left.DesiredWidth, Right.DesiredWidth + GetOperatorWidth()),
                    Left.DesiredHeight + Math.Max(Operator.DesiredHeight, Right.DesiredHeight));
            }
        }

        private float GetOperatorWidth()
        {
            float width;

            if (Operator.Operator == BinaryOperator.None)
            {
                if (Right is ParantheseExpression ||
                    (Left is BinaryExpression) && (Left as BinaryExpression).Operator.Operator == BinaryOperator.Divide ||
                    (Right is BinaryExpression) && (Right as BinaryExpression).Operator.Operator == BinaryOperator.Divide)
                {
                    width = 0;
                }
                else
                {
                    width = -5;
                }
            }
            else
            {
                width = Operator.DesiredWidth;
            }

            return width;
        }


        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            if (Operator.Operator == BinaryOperator.Divide)
            {
                OnLayoutDivide(left, top, width, height, g, font, style);
            }
            else
            {
                OnLayoutHorizontal(left, top, width, height, g, font, style);
            }
        }

        private void OnLayoutDivide(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            Left.Layout(this, (width - Left.DesiredWidth) / 2, 0, Left.DesiredWidth, Left.DesiredHeight, g, font, style);
            Operator.Layout(this, 0, Left.DesiredHeight, Operator.DesiredWidth, Operator.DesiredHeight, g, font, style);
            Right.Layout(this, (width - Right.DesiredWidth) / 2, Left.DesiredHeight + Operator.DesiredHeight - 1, Right.DesiredWidth, Right.DesiredHeight, g, font, style);
        }

        private void OnLayoutHorizontal(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            if (!isTwoLines)
            {
                float maxBottom = Math.Max(Left.FindMaxBottom(), Right.FindMaxBottom());

                float baseLine = height - maxBottom;

                float opTop = baseLine - Operator.DesiredHeight / 2;
                float leftTop = baseLine - Left.FindMaxTop();
                float rightTop = baseLine - Right.FindMaxTop();

                Left.Layout(this, 0, leftTop, Left.DesiredWidth, Left.DesiredHeight, g, font, style);
                Operator.Layout(this, Left.DesiredWidth, opTop, Operator.DesiredWidth, Operator.DesiredHeight, g, font, style);
                Right.Layout(this, Left.DesiredWidth + GetOperatorWidth(), rightTop, Right.DesiredWidth, Right.DesiredHeight, g, font, style);
            }
            else
            {
                Left.Layout(this, 0, 0, Left.DesiredWidth, Left.DesiredHeight, g, font, style);
                Operator.Layout(this, 0, FindMaxTop() - Operator.DesiredHeight / 2, Operator.DesiredWidth, Operator.DesiredHeight, g, font, style);
                Right.Layout(this, Operator.DesiredWidth, Left.DesiredHeight + (Right.DesiredHeight >= Operator.DesiredHeight ? 0 : Operator.DesiredHeight / 2 - Right.FindMaxTop()), Right.DesiredWidth, Right.DesiredHeight, g, font, style);
            }
        }


        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            Left.Draw(g, font, style);
            Operator.Draw(g, font, style);
            Right.Draw(g, font, style);
        }
    }
}
