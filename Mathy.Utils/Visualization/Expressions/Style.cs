using System.Drawing;

namespace Mathy.Visualization.Expressions
{
    public class Style
    {
        public Style()
        {
            Font = new Font("宋体", 12);
            BoldFont = new Font(Font, FontStyle.Bold);
            SmallFont = new Font("宋体", 9);


            LevelContext = new LevelContext();
        }

        public Font Font { get; set; }

        public Font BoldFont { get; set; }

        public Font SmallFont { get; set; }

        public LevelContext LevelContext { get; set; }
    }
}
