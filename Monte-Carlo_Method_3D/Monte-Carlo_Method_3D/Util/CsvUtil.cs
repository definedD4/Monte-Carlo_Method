using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Util
{
    public static class CsvUtil
    {
        public static IEnumerable<string> ExportToLines(GridData data, char delim = ';')
        {
            for (int j = 0; j < data.Size.Columns; j++)
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < data.Size.Rows; i++)
                {
                    str.Append(data[new GridIndex(i, j)]);
                    if (j != data.Size.Columns - 1)
                        str.Append(delim);
                }
                yield return str.ToString();
            }
        } 


        public static void ExportToFile(GridData data, string path, char delim = ';')
        {
            File.WriteAllLines(path, ExportToLines(data, delim), Encoding.UTF8);
        }

        public static GridData ImportFromFile(string path, char delim = ';')
        {
            var res = File.ReadAllLines(path).Select(
                l => l.Split(delim).Select(
                    w => double.Parse(w.Trim())
                    ).ToArray()
                ).ToArray();
            int rows = res.First().Length;
            int columns = res.Length;

            double[,] data = new double[rows, columns];

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    data[j, i] = res[i][j];
                }
            }

            return GridData.FromArray(data);
        }
    }
}
