// Dandelion.Collections.SearchTable<TRow,TColumn,TElement>
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Utils.Dandelion.Collections
{
    public class SearchTable<TRow, TColumn, TElement>
    {
        private TRow[] rows;

        private TColumn[] columns;

        private TElement[] items;

        public bool ExactMatchRow
        {
            get;
            set;
        }

        public bool ExactMatchColumn
        {
            get;
            set;
        }

        public TElement this[TRow row, TColumn column]
        {
            get
            {
                return items[ResolveIndex(row, column)];
            }
            set
            {
                items[ResolveIndex(row, column)] = value;
            }
        }

        internal SearchTable(TRow[] rows, TColumn[] columns, TElement[] items)
        {
            this.rows = rows;
            this.columns = columns;
            this.items = items;
        }

        public static SearchTable<TRow, TColumn, TElement> Load(string filePath)
        {
            SearchTableBuilder<TRow, TColumn, TElement> searchTableBuilder = new SearchTableBuilder<TRow, TColumn, TElement>();
            searchTableBuilder.Build(filePath);
            return searchTableBuilder.Table;
        }

        private int ResolveIndex(TRow row, TColumn column)
        {
            int num = Array.BinarySearch(rows, row);
            if (num < 0)
            {
                if (!ExactMatchRow)
                {
                    num = ~num;
                    if (num > 0)
                    {
                        num--;
                    }
                }
                if (num < 0 || num > rows.Length - 1)
                {
                    throw new KeyNotFoundException($"Cannot find {row} in row list.");
                }
            }
            int num2 = Array.BinarySearch(columns, column);
            if (num2 < 0)
            {
                if (!ExactMatchColumn)
                {
                    num2 = ~num2;
                    if (num2 > 0)
                    {
                        num2--;
                    }
                }
                if (num2 < 0 || num2 > columns.Length - 1)
                {
                    throw new KeyNotFoundException($"Cannot find {column} in column list.");
                }
            }
            return num * columns.Length + num2;
        }

        public SearchTable<TRow, TColumn, T> Map<T>(Func<TElement, T> mapper)
        {
            return new SearchTable<TRow, TColumn, T>(rows, columns, items.Select(mapper).ToArray());
        }
    }
}
