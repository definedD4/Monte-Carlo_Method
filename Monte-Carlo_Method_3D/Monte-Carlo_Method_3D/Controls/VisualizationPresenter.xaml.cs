using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Monte_Carlo_Method_3D.VisualizationModel;

namespace Monte_Carlo_Method_3D.Controls
{
    /// <summary>
    /// Логика взаимодействия для VisualizationPresenter.xaml
    /// </summary>
    public partial class VisualizationPresenter : UserControl
    {
        public VisualizationPresenter()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty VisualizationProperty = DependencyProperty.Register(
            "Visualization", typeof(IVisualization), typeof(VisualizationPresenter), new PropertyMetadata(default(IVisualization)));

        public IVisualization Visualization
        {
            get { return (IVisualization) GetValue(VisualizationProperty); }
            set { SetValue(VisualizationProperty, value); }
        }
    }
}
