using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Exceptions;

namespace Monte_Carlo_Method_3D.Util
{
    public static class CsvUtil
    {
        public static IEnumerable<string> ExportToLines(GridData data, char delim = ';')
        {
            for (int j = 0; j < data.Size.Width; j++)
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < data.Size.Height; i++)
                {
                    str.Append(data[new GridIndex(i, j)]);
                    if (j != data.Size.Width - 1)
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
            double[,] data;
            try
            {
                var res = File.ReadAllLines(path).Select(
                    l => l.Split(delim).Select(
                        w => double.Parse(w.Trim())
                    ).ToArray()
                ).ToArray();
                int rows = res.First().Length;
                int columns = res.Length;

                data = new double[rows, columns];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        data[j, i] = res[i][j];
                    }
                }
            }
            catch (Exception e)
            {
                throw new TableLoadException("An error occured while loading table.", e);
            }
            return GridData.FromArray(data);
        }
    }
}
