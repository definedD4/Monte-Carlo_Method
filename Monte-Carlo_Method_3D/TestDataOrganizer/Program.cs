using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monte_Carlo_Method_3D.Util;

namespace TestDataOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string absF = args[0];
            string prF = args[1];
            string stF = args[2];

            var abs = CsvUtil.ImportFromFile(absF);
            var pr = CsvUtil.ImportFromFile(prF);
            var st = CsvUtil.ImportFromFile(stF);

            var mask = args[3].Trim().Split(';').Select(
                        i =>
                        {
                            var coords = i.Trim().Split(',').Select(j => int.Parse(j.Trim())).ToArray();
                            return new IntPoint(coords[0], coords[1]);
                        }).ToList();

            double[,] res = new double[mask.Count, 5];

            for (int i = 0; i < mask.Count; i++)
            {
                res[i, 0] = abs[mask[i]];
                res[i, 1] = pr[mask[i]];
                res[i, 2] = pr[mask[i]] - abs[mask[i]];
                res[i, 3] = st[mask[i]];
                res[i, 4] = st[mask[i]] - abs[mask[i]];
            }

            CsvUtil.ExportToFile(res, args[4]);
        }
    }
}
