namespace Mathy
{
    class MathyContext
    {
        public static PathResolver PathResolver { get; private set; }


        public static void Init(string rootPath)
        {
            PathResolver = new PathResolver(rootPath);
        }
    }
}
