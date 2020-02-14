namespace Mathy.Templates
{
    public class TemplateCollection
    {
        public string Name { get; set; }

        public Template[] Templates { get; set; }

        public TemplateCollection[] SubCollections { get; set; }


        public static TemplateCollection Load(string folderPath)
        {
            return TemplateCollectionLoader.Load(folderPath);
        }
    }
}
