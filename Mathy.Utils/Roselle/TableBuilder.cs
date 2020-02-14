using Roselle;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Roselle
{
    public class TableBuilder
    {
        private Document document;

        private DocumentBuilder builder;

        private List<string[]> table = new List<string[]>();

        private bool hasHeader;

        internal TableBuilder(Document document, DocumentBuilder builder)
        {
            this.document = document;
            this.builder = builder;
        }

        public TableBuilder Header(params object[] cells)
        {
            hasHeader = true;
            return Row(cells);
        }

        public TableBuilder Row(params object[] cells)
        {
            table.Add(cells.Select((object i) => (i == null) ? string.Empty : i.ToString()).ToArray());
            return this;
        }

        public TableBuilder Rows(IEnumerable<IEnumerable> rows)
        {
            foreach (IEnumerable row in rows)
            {
                List<object> list = new List<object>();
                foreach (object item in row)
                {
                    list.Add(item);
                }
                Row(list.Select((object i) => (i == null) ? string.Empty : i.ToString()).ToArray());
            }
            return this;
        }

        public DocumentBuilder EndTable()
        {
            if (table.Count > 0 && table.Max((string[] i) => i.Length) > 0)
            {
                int count = table.Count;
                int num = table.Max((string[] i) => i.Length);
                string[][] array = new string[count][];
                for (int j = 0; j <= count - 1; j++)
                {
                    string[] array2 = new string[num];
                    for (int k = 0; k <= num - 1; k++)
                    {
                        array2[k] = ((k > table[j].Length - 1) ? string.Empty : table[j][k]);
                    }
                    array[j] = array2;
                }
                document.Elements.Add(new Table(count, num)
                {
                    HasHeader = hasHeader,
                    Cells = array
                });
            }
            return builder;
        }
    }
}
