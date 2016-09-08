using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutputComparasionGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            double simTime = 1000d;

            PrSimulator pr = new PrSimulator(5, 5, new IntPoint(2, 2));
            StSimulator st = new StSimulator(5, 5, new IntPoint(2, 2));

            List<Tuple<double, double>> prp = new List<Tuple<double, double>>();
            List<Tuple<double, double>> stp = new List<Tuple<double, double>>();

            double prt = -1d;
            double stt = -1d;

            while (pr.TotalSimTime < simTime)
            {
                pr.SimulateSteps();
                if (prt == -1d) prt = pr.TotalSimTime;
                prp.Add(Tuple.Create(pr.TotalSimTime - prt, pr[2, 0]));
            }

            while (st.TotalSimTime < simTime)
            {
                st.SimulateSteps();
                if (stt == -1d) stt = st.TotalSimTime;
                stp.Add(Tuple.Create(st.TotalSimTime - stt, st[2, 0]));
            }

            File.WriteAllLines("comp_pr.csv", prp.Select(x => $"{x.Item1};{x.Item2}"), Encoding.UTF8);
            File.WriteAllLines("comp_st.csv", stp.Select(x => $"{x.Item1};{x.Item2}"), Encoding.UTF8);

        }
    }
}
