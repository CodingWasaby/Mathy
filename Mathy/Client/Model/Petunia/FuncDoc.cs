namespace Petunia
{
    public class FuncDoc
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ParameterDoc[] Parameters { get; set; }

        public ReturnDoc[] Returns { get; set; }

        public Article Article { get; set; }

        public string ArticleStr { get; set; }
    }

    public class ParameterDoc
    {
        public string Name { get; set; }

        public string[] Type { get; set; }

        public string Description { get; set; }
    }

    public class ReturnDoc
    {
        public string[] Type { get; set; }

        public string Description { get; set; }
    }
}
