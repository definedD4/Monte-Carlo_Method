using Monte_Carlo_Method_3D.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Util
{
    public static class CsvUtil
    {
        public static IEnumerable<string> ExportToLines(double[,] data, char delim = ';')
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                StringBuilder str = new StringBuilder();
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    str.Append(data[x, y]);
                    if (x != data.GetLength(0) - 1)
                        str.Append(delim);
                }
                yield return str.ToString();
            }
        } 

        public static void ExportToFile(double[,] data, string path, char delim = ';')
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
            int width = res.First().Length;
            int height = res.Length;

            double[,] data = new double[width, height];

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    data[j, i] = res[i][j];
                }
            }

            return new GridData(data);
        }
    }
}
