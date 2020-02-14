using System.IO;

namespace Mathy
{
    class PathResolver
    {
        public PathResolver(string rootPath)
        {
            this.rootPath = rootPath;
        }


        private string rootPath;


        public string GetTablesFilePath(string fileName)
        {
            return Path.Combine(rootPath, "Tables", fileName);
        }
    }
}
