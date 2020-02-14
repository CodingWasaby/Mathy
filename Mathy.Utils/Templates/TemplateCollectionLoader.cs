using Mathy.Utils.Dandelion.Serialization;
using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace Mathy.Templates
{
    class TemplateCollectionLoader
    {
        public static TemplateCollection Load(string folderPath)
        {
            return new TemplateCollection()
            {
                Name = folderPath.Substring(folderPath.LastIndexOf("\\") + 1),
                Templates = Directory.GetFiles(folderPath).Select(i => ParseTemplate(File.ReadAllText(i, System.Text.Encoding.UTF8))).ToArray(),
                SubCollections = Directory.GetDirectories(folderPath).Select(i => Load(i)).ToArray()
            };
        }

        private static Template ParseTemplate(string s)
        {
            IDictionary dict = new JsonDeserializer().DeserializeString(s) as IDictionary;

            return new Template()
            {
                Author = dict["Author"] as string,
                Description = dict["Description"] as string,
                Expression = dict["Expression"] as string,
                ID = dict["ID"] as string,
                LastUpdateTime = DateTime.Parse(dict["LastUpdateTime"] as string),
                Name = dict["Name"] as string,
                Parameters = (dict["Parameters"] as IEnumerable).Cast<IDictionary>().Select(i => ParseParameter(i)).ToArray(),
                Reference = dict["References"] as string,
                ReturnType = (DataType)Enum.Parse(typeof(DataType), dict["ReturnType"] as string),
                ReturnUnit = dict["ReturnUnit"] as string
            };
        }

        private static Parameter ParseParameter(IDictionary dict)
        {
            return new Parameter()
            {
                Default = dict["Default"] == null ? null : dict["Default"].ToString(),
                Description = dict["Description"] as string,
                Name = dict["Name"] as string,
                Type = (DataType)Enum.Parse(typeof(DataType), dict["Type"] as string),
                Unit = dict["Unit"] as string
            };
        }
    }
}
