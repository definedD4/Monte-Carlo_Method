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
using System.Windows.Shapes;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.VisualizationModel;
using Monte_Carlo_Method_3D.VisualizationProvider;

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

        // TODO: Fix bugs with rendering
        public ShowTableDialog(GridData data)
        {
            m_Data = data;

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
                            GridDataVisualizationProvider.Color().ProvideVisualization(m_Data);
                        break;
                    case "3D":
                        VisualizationPresenter.Visualization =
                            GridDataVisualizationProvider.Model3D(m_Data.Size).ProvideVisualization(m_Data);
                        break;
                }
            };

            VisualTypeSelector.RaiseSelectionChanged();                      

            TitleTb.Text = $"Таблица: {m_Data.Size.Height} на {m_Data.Size.Width}";

            /*Img.Source = DrawingUtil.DrawTable(data, BackgroundColor, ForegroundColor, GridPen);

            Img.MouseMove += (s, e) =>
            {
                var pos = e.GetPosition(Img);

                int x = (int)Math.Truncate(pos.X * data.Size.Width / Img.ActualWidth);
                int y = (int)Math.Truncate(pos.Y * data.Size.Height / Img.ActualHeight);

                if(x < 0 || x >= m_Data.Size.Width || y < 0 || y >= m_Data.Size.Height)
                    return;

                if (!PointedValuePopup.IsOpen) { PointedValuePopup.IsOpen = true; }

                PointedValuePopup.HorizontalOffset = pos.X + 20;
                PointedValuePopup.VerticalOffset = pos.Y;

                PointedValueText.Text = m_Data[x,y].ToString("E4");
            };

            Img.MouseLeave += (s, e) =>
            {
                PointedValuePopup.IsOpen = false;
            };*/
        }

        public SelectorCommand VisualTypeSelector { get; }
    }
}
