using Mathy.Language;
using Mathy.Maths;
using System;
using System.Drawing;

namespace Mathy.Visualization.Computation
{
    abstract class Node
    {
        public double Left { get; set; }

        public double Top { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }


        public double NodeLeft { get; set; }

        public double NodeTop { get; set; }

        public double NodeWidth { get; set; }

        public double NodeHeight { get; set; }


        public string Description { get; set; }


        protected string GetValueString(object value)
        {
            if (value == null)
            {
                return "空值";
            }
            else if (value.GetType().IsArray)
            {
                return string.Format("长度为{0}的数组", ((Array)value).Length);
            }
            else if (value.Equals(double.PositiveInfinity))
            {
                return MathyConstants.GetSymbol("posinf");
            }
            else if (value.Equals(double.NegativeInfinity))
            {
                return MathyConstants.GetSymbol("neginf");
            }
            else if (value is Matrix)
            {
                Matrix m = value as Matrix;
                return string.Format("{0}*{1}矩阵", m.RowCount, m.ColumnCount);
            }
            else if (value is Bitmap)
            {
                return "图片";
            }
            else if (value is VariableContextExpression)
            {
                return "表达式";
            }
            else
            {
                return value.ToString();
            }
        }

        public abstract void Measure(Graphics g, TreeStyle style);

        public abstract void Layout(TreeStyle style, double left, double top);

        public virtual void PostLayout(TreeStyle style)
        {
        }

        public abstract void Draw(Graphics g, TreeStyle style);
    }
}
