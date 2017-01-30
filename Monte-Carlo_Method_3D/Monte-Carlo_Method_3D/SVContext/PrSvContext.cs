﻿using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.SVContext
{
    /// <summary>
    /// Bundles together simulator and visualizer.
    /// </summary>
    public abstract class PrSvContext
    {
        public SimulationOptions Options { get; }

        public Pallete Pallete { get; }

        public PrSimulator Simulator { get; }

        private PrSvContext(SimulationOptions options, Pallete pallete)
        {
            Options = options;
            Pallete = pallete;
            Simulator = new PrSimulator(options);
        }

        public abstract IVisualization ProvideVisualization();

        public abstract PrSvContext Clone(SimulationOptions options);
        
        // Implementations

        // Table

        public static PrSvContext Table(SimulationOptions options, Pallete pallete) => 
            new TablePrSvContext(options, pallete);

        private class TablePrSvContext : PrSvContext
        {
            private readonly Visualizer2D m_Visualizer;

            public TablePrSvContext(SimulationOptions options, Pallete pallete) : base(options, pallete)
            {
                m_Visualizer = new Visualizer2D() {Pallete = pallete};
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateTableVisualization(Simulator.GetData());
            }

            public override PrSvContext Clone(SimulationOptions options)
            {
                return new TablePrSvContext(options, Pallete);
            }
        }

        // Color

        public static PrSvContext Color(SimulationOptions options, Pallete pallete) =>
            new ColorPrSvContext(options, pallete);

        private class ColorPrSvContext : PrSvContext
        {
            private readonly Visualizer2D m_Visualizer;

            public ColorPrSvContext(SimulationOptions options, Pallete pallete) : base(options, pallete)
            {
                m_Visualizer = new Visualizer2D() {Pallete = pallete};
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateColorVisualization(Simulator.GetData());
            }

            public override PrSvContext Clone(SimulationOptions options)
            {
                return new ColorPrSvContext(options, Pallete);
            }
        }

        // Model 3D

        public static PrSvContext Model3D(SimulationOptions options, Pallete pallete) =>
            new Model3DPrSvContext(options, pallete);

        private class Model3DPrSvContext : PrSvContext
        {
            private readonly Visualizer3D m_Visualizer;

            public Model3DPrSvContext(SimulationOptions options, Pallete pallete) : base(options, pallete)
            {
                m_Visualizer = new Visualizer3D(options.Size) {Pallete = pallete};
            }

            public override IVisualization ProvideVisualization()
            {
                return m_Visualizer.GenerateModel3DVisualization(Simulator.GetData());
            }

            public override PrSvContext Clone(SimulationOptions options)
            {
                return new Model3DPrSvContext(options, Pallete);
            }
        }
    }
}