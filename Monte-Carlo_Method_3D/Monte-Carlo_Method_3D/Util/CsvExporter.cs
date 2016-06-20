using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.Util
{
    public class CsvExporter
    {
        public CsvExporter(char delim)
        {
            Delim = delim;
        }

        public char Delim { get; }

        public IEnumerable<string> ExportToLines(double[,] data)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                StringBuilder str = new StringBuilder();
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    str.Append(data[x, y]);
                    if (x != data.GetLength(0) - 1)
                        str.Append(Delim);
                }
                yield return str.ToString();
            }
        } 

        public void ExportToFile(double[,] data, string path)
        {
            File.WriteAllLines(path, ExportToLines(data), Encoding.UTF8);
        }
    }
}
