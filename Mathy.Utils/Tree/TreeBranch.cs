namespace Mathy.Tree
{
    public class TreeBranch
    {
        public TreeBranch Parent { get; private set; }

        private TreeBranch[] branches = new TreeBranch[] { };
        public TreeBranch[] Branches
        {
            get { return branches; }
            set
            {
                branches = value;

                foreach (TreeBranch branch in branches)
                {
                    branch.Parent = this;
                }
            }
        }

        public string Description { get; set; }

        public int Position { get; set; }

        public int Length { get; set; }

        public int Angle { get; set; }
    }
}
