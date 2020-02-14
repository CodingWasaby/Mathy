using System.Collections.Generic;

namespace Mathy.Client
{
    public class MathyDic
    {
        private static Dictionary<string, string> nameToDisplay = new Dictionary<string, string>();
        private static Dictionary<string, string> displayToName = new Dictionary<string, string>();

        static MathyDic()
        {
            AddEntry("Number", "数值");
            AddEntry("String", "字符串");
            AddEntry("Matrix", "矩阵");
            AddEntry("Vector", "向量");
            AddEntry("Array", "数组");
            AddEntry("Expression", "表达式");
            AddEntry("Boolean", "布尔");
            AddEntry("Object", "对象");
            AddEntry("Image", "图像");
            AddEntry("Dictionary", "字典");
        }

        private static void AddEntry(string name, string displayName)
        {
            nameToDisplay.Add(name, displayName);
            displayToName.Add(displayName, name);
        }


        public static string GetDataTypeDisplayName(string name)
        {
            return nameToDisplay[name];
        }

        public static string GetDataTypeName(string displayName)
        {
            return displayToName[displayName];
        }
    }
}