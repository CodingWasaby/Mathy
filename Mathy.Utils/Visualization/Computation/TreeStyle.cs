using System.Drawing;

namespace Mathy.Visualization.Computation
{
    public class TreeStyle
    {
        public int LeftMargin { get; set; }

        public int TopMargin { get; set; }

        public int RightMargin { get; set; }

        public int BottomMargin { get; set; }

        public int HorizontalNodeSpacing { get; set; }

        public int VerticalNodeSpacing { get; set; }

        public int JointSpacing { get; set; }

        public Font Font { get; set; }

        public Font ValueFont { get; set; }

        public Font TitleFont { get; set; }

        public int TitleTopMargin { get; set; }

        public int TitleBottomMargin { get; set; }

        public Color VariableBorderColor { get; set; }

        public Color VariableFillColor { get; set; }

        public Color VariableArrowColor { get; set; }

        public Color ExpressionBorderColor { get; set; }

        public Color ExpressionFillColor { get; set; }

        public Color ExpressionArrowColor { get; set; }


        public static TreeStyle CreateDefault()
        {
            return new TreeStyle()
            {
                LeftMargin = 10,
                TopMargin = 10,
                RightMargin = 10,
                BottomMargin = 10,
                HorizontalNodeSpacing = 25,
                VerticalNodeSpacing = 75,
                JointSpacing = 5,
                Font = new Font("宋体", 12),
                ValueFont = new Font(new Font("宋体", 12), FontStyle.Bold),
                TitleFont = new Font("宋体", 20),
                TitleTopMargin = 10,
                TitleBottomMargin = 10,
                VariableBorderColor = Color.DarkOliveGreen,
                VariableFillColor = Color.LightGreen,
                VariableArrowColor = Color.DarkOliveGreen,
                ExpressionBorderColor = Color.DarkGray,
                ExpressionFillColor = Color.LightGray,
                ExpressionArrowColor = Color.DarkGray
            };
        }
    }
}
