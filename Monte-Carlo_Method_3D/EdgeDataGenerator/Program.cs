using System;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Util.AssertHelper;
using static System.Math;

namespace EdgeDataGenerator
{
    public class Program
    {
        static double Function(double x, double y) => Log(x*x + y*y);

        static void PrintUsage()
        {
            Console.WriteLine("USAGE:");
            Console.WriteLine("EdgeDataGenerator.exe <Output> <X coord> <Y coord> <H> <X size> <Y size>");
            Console.WriteLine("Output: path to output file");
            Console.WriteLine("X - coord: X coordinate of bottom left corner of the grid");
            Console.WriteLine("Y - coord: Y coordinate of bottom left corner of the grid");
            Console.WriteLine("H: size of the grid cell");
            Console.WriteLine("X size: number of nodes in horizontal dimension");
            Console.WriteLine("Y size: number of nodes in vertical dimension");
        }

        static void Pause()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            if (args.Length < 6)
            {
                PrintUsage();
                Pause();
                return;
            }

            string output;
            double xs, ys, h;
            int xd, yd;
            try
            {
                output = args[0];
                xs = double.Parse(args[1]);
                ys = double.Parse(args[2]);
                h = double.Parse(args[3]);
                xd = int.Parse(args[4]);
                yd = int.Parse(args[5]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while parsing arguments: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Stack trace:");
                Console.WriteLine(e.StackTrace);
                Console.Write("Passed arguments: ");
                foreach (var arg in args)
                {
                    Console.Write($"{arg} ");
                }
                Console.WriteLine();
                PrintUsage();
                Pause();
                return;
            }

            Console.WriteLine($"Generating grid {xd} by {yd} starting from ({xs}; {ys}) with cell size {h} ...");
            GridData res = GridData.AllocateNew(new GridSize(xd, yd));

            for (int xi = 0; xi < xd; xi++)
            {
                for (int yi = 0; yi < yd; yi++)
                {
                    double x = xs + xi*h;
                    double y = ys + yi*h;

                    res[xi, yi] = Function(x, y);
                }
            }

            Console.WriteLine($"Writing result to {output} ...");

            CsvUtil.ExportToFile(res, output);

            Console.WriteLine("Done.");
            Pause();
        }
    }
}
