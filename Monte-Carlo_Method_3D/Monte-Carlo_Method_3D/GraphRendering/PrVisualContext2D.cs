﻿using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Visualization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Monte_Carlo_Method_3D.GraphRendering
{
    public class PrVisualContext2D : PrGridVisualContext
    {
        private ImageSource m_Texture;

        public PrVisualContext2D(PrSimulator simulator, PrVisualizer visualizer) : base(simulator, visualizer)
        {

        }

        public ImageSource Texture
        {
            get { return m_Texture; }
            private set { m_Texture = value; OnPropertyChanged(nameof(Texture));}
        }

        public override void Update()
        {
            Texture = Visualizer.GenerateColorTexture();
            OnPropertyChanged("Texture");
        }
    }
}
