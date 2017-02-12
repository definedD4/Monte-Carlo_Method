using System.Windows;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Util.Commands;
using Monte_Carlo_Method_3D.VisualizationProvider;
using static System.Math;

namespace Monte_Carlo_Method_3D.Dialogs
{
    /// <summary>
    /// Interaction logic for ShowTableDialog.xaml
    /// </summary>
    public partial class ShowTableDialog : Window
    {
        public static void ShowNew(GridData data)
        {
            new ShowTableDialog(data).Show();
        }

        private readonly GridData m_Data;
        private readonly GridData m_NormalizedData;

        public SelectorCommand VisualTypeSelector { get; }

        public ShowTableDialog(GridData data)
        {
            m_Data = data;
            m_NormalizedData = NormalizeData(data);

            InitializeComponent();

            this.DataContext = this;

            VisualTypeSelector = new SelectorCommand("Table");

            VisualTypeSelector.SelectionChanged += (sender, args) =>
            {
                switch (VisualTypeSelector.SelectedValue)
                {
                    case "Table":
                        VisualizationPresenter.Visualization =
                            GridDataVisualizationProvider.Table().ProvideVisualization(m_Data);
                        break;
                    case "2D":
                        VisualizationPresenter.Visualization =
                            GridDataVisualizationProvider.Color().ProvideVisualization(m_NormalizedData);
                        break;
                    case "3D":
                        VisualizationPresenter.Visualization =
                            GridDataVisualizationProvider.Model3D(m_Data.Size).ProvideVisualization(m_NormalizedData);
                        break;
                }
            };

            TitleTb.Text = $"Таблица: {m_Data.Size.Height} на {m_Data.Size.Width}";

            VisualTypeSelector.RaiseSelectionChanged();
        }

        private static GridData NormalizeData(GridData data)
        {
            double min = double.MaxValue, max = double.MinValue;

            foreach (var idx in data.Bounds.EnumerateRegion())
            {
                min = Min(min, data[idx]);
                max = Max(max, data[idx]);
            }

            double magnitude = max - min;

            var newData = GridData.AllocateLike(data);

            foreach (var idx in data.Bounds.EnumerateRegion())
            {
                newData[idx] = (data[idx] - min) / magnitude;
            }

            return newData;
        }
    }
}
