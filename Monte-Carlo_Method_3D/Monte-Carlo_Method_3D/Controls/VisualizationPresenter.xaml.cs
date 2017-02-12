using System.Windows;
using System.Windows.Controls;
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
