namespace Mathy.Visualization.Expressions
{
    public class MeasureSpec
    {
        public MeasureSpecMode Mode { get; set; }

        public float Size { get; set; }


        public static MeasureSpec MakeFixed(float size)
        {
            return new MeasureSpec() { Mode = MeasureSpecMode.Fixed, Size = size };
        }

        public static MeasureSpec MakeAtMost(float size)
        {
            return new MeasureSpec() { Mode = MeasureSpecMode.AtMost, Size = size };
        }

        public static MeasureSpec MakeUnspecified()
        {
            return new MeasureSpec() { Mode = MeasureSpecMode.Unspecified };
        }
    }
}
