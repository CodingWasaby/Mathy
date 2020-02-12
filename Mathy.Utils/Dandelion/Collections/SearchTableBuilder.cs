// Dandelion.Collections.SearchTableBuilder<TRow,TColumn,TElement>
using Mathy.Utils.Dandelion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mathy.Utils.Dandelion.Collections
{
    internal class SearchTableBuilder<TRow, TColumn, TElement>
    {
        private TRow[] rows;

        private TColumn[] columns;

        private TElement[] items;

        public SearchTable<TRow, TColumn, TElement> Table
        {
            get;
            private set;
        }

        public void Build(string filePath)
        {
            List<TRow> list = new List<TRow>();
            List<TElement> list2 = new List<TElement>();
            using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                string text = streamReader.ReadLine();
                columns = (from i in text.Split('\t').Skip(1)
                           select Types.ConvertValue<TColumn>(i)).ToArray();
                while (!streamReader.EndOfStream)
                {
                    string[] array = streamReader.ReadLine().Split('\t');
                    list.Add(Types.ConvertValue<TRow>(array[0]));
                    foreach (string item in array.Skip(1))
                    {
                        list2.Add(Types.ConvertValue<TElement>(item));
                    }
                }
            }
            rows = list.ToArray();
            items = list2.ToArray();
            SortColumns();
            SortRows();
            Table = new SearchTable<TRow, TColumn, TElement>(rows, columns, items);
        }

        private void SortColumns()
        {
            List<TColumn> list = columns.ToList();
            TColumn[] array = columns.ToArray();
            Array.Sort(array);
            TElement[] array2 = new TElement[items.Length];
            for (int i = 0; i <= columns.Length - 1; i++)
            {
                int num = i;
                int num2 = list.IndexOf(array[num]);
                for (int j = 0; j <= rows.Length - 1; j++)
                {
                    array2[j * columns.Length + num] = items[j * columns.Length + num2];
                }
            }
            columns = array.ToArray();
            items = array2.ToArray();
        }

        private void SortRows()
        {
            List<TRow> list = rows.ToList();
            TRow[] array = rows.ToArray();
            Array.Sort(array);
            TElement[] array2 = new TElement[items.Length];
            for (int i = 0; i <= rows.Length - 1; i++)
            {
                int num = i;
                int num2 = list.IndexOf(array[num]);
                for (int j = 0; j <= columns.Length - 1; j++)
                {
                    array2[num * columns.Length + j] = items[num2 * columns.Length + j];
                }
            }
            rows = array.ToArray();
            items = array2.ToArray();
        }
    }
}
