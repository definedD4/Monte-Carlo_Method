﻿using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OutputComparasionGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            PrSimulator pr = new PrSimulator(11, 11, new IntPoint(5, 5));
            StSimulator st = new StSimulator(11, 11, new IntPoint(5, 5));

            Func<double, double, double> u = (x, y) => Math.Log(Math.Sqrt((x / 10 + 3)* (x / 10 + 3) + (y / 10 + 3)* (y / 10 + 3)));


            File.WriteAllLines("comp_pr.csv", CalcPr(pr, u).Select(x => $"{x.Item1.ToString("E20")};{x.Item2.ToString("E20")}"), Encoding.UTF8);
            Console.WriteLine("Finished pr");
            File.WriteAllLines("comp_st.csv", CalcSt(st, u).Select(x => $"{x.Item1.ToString("E20")};{x.Item2.ToString("E20")}"), Encoding.UTF8);
        }

        static List<Tuple<double, double>> CalcPr(PrSimulator sim, Func<double, double, double> u)
        {
            List<Tuple<double, double>> ress = new List<Tuple<double, double>>();
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"PR i = {i}");
                double maxSimTime = i * 20d;

                sim.Reset();
                while(sim.TotalSimTime <= maxSimTime)
                {
                    sim.SimulateSteps();
                }

                double res = 0;

                for(int x = 0; x < sim.Width; x++)
                {
                    res += u(x, 0) * sim[x, 0];
                    res += u(x, sim.Height - 1) * sim[x, sim.Height - 1];
                }

                for(int y = 1; y < sim.Height - 1; y++)
                {
                    res += u(0, y) * sim[0, y];
                    res += u(sim.Width - 1, y) * sim[sim.Width - 1, y];
                }

                ress.Add(Tuple.Create(maxSimTime, res));
            }
            return ress;
        }

        static List<Tuple<double, double>> CalcSt(StSimulator sim, Func<double, double, double> u)
        {
            List<Tuple<double, double>> ress = new List<Tuple<double, double>>();
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"PR i = {i}");
                double maxSimTime = i * 20d;

                sim.Reset();
                while (sim.TotalSimTime <= maxSimTime)
                {
                    sim.SimulateSteps();
                }

                double res = 0;

                for (int x = 0; x < sim.Width; x++)
                {
                    res += u(x, 0) * sim[x, 0];
                    res += u(x, sim.Height - 1) * sim[x, sim.Height - 1];
                }

                for (int y = 1; y < sim.Height - 1; y++)
                {
                    res += u(0, y) * sim[0, y];
                    res += u(sim.Width - 1, y) * sim[sim.Width - 1, y];
                }

                ress.Add(Tuple.Create(maxSimTime, res));
            }
            return ress;

        }
    }
}