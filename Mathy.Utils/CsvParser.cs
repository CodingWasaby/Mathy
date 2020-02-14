using Mathy.Maths;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;

namespace Mathy
{
    public class CsvParser
    {
        public static Matrix Parse(string s)
        {
            s = s.Replace("[", "").Replace("]", "");
            s = s.Replace(";", "\r\n");
            List<string[]> rows = new List<string[]>();
            int columnCount = 0;

            foreach (string line in s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] columns = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int currColumnCount = columns.Length;
                if (columnCount != 0 && columnCount != currColumnCount)
                {
                    throw new Exception("矩阵每行的列数不相等。");
                }
                else if (columnCount == 0)
                {
                    columnCount = currColumnCount;
                }
                rows.Add(columns);
            }

            Matrix m = new Matrix(rows.Count, columnCount);
            for (int i = 0; i <= rows.Count - 1; i++)
            {
                for (int j = 0; j <= rows[i].Length - 1; j++)
                {
                    m[i, j] = Convert.ToDouble(rows[i][j]);
                }
            }

            return m;
        }

        public static Matrix Parse(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var sheet1 = result.Tables[0];
                Matrix m = new Matrix(sheet1.Rows.Count, sheet1.Columns.Count);
                for (var r = 0; r < sheet1.Rows.Count - 1; r++)
                {
                    var row = sheet1.Rows[r];
                    for (var c = 0; c < sheet1.Rows.Count - 1; c++)
                    {
                        m[r, c] =Convert.ToDouble( row[c]);
                    }
                }
                return m;
            }
        }
    }
}
