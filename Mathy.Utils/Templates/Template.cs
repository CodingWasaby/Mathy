using System;

namespace Mathy.Templates
{
    public class Template
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Expression { get; set; }

        public Parameter[] Parameters { get; set; }

        public DataType ReturnType { get; set; }

        public string ReturnUnit { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public string Author { get; set; }
    }
}
