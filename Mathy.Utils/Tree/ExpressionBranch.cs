namespace Mathy.Tree
{
    public class ExpressionBranch : TreeBranch
    {
        public ExpressionBranch()
        {
            DescriptionX = 20;
            DescriptionY = 25;
        }

        public string Expression { get; set; }

        public int ImageX { get; set; }

        public int ImageY { get; set; }

        public int DescriptionX { get; set; }

        public int DescriptionY { get; set; }

        public int DescriptionOffsetX { get; set; }

        public int DescriptionOffsetY { get; set; }

        public bool IsPrimaryBranch { get; internal set; }
    }
}
