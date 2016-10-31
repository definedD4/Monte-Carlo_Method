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
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util;

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

        private readonly Color BackgroundColor = Colors.White;
        private readonly Color ForegroundColor = Colors.Black;

        private readonly GridData m_Data;

        public ShowTableDialog(GridData data)
        {
            m_Data = data;

            InitializeComponent();

            Img.Source = DrawingUtil.DrawTable(data, BackgroundColor, ForegroundColor);

            Img.MouseMove += (s, e) =>
            {
                var pos = e.GetPosition(Img);

                int x = (int)Math.Truncate(pos.X * data.Width / Img.ActualWidth);
                int y = (int)Math.Truncate(pos.Y * data.Height / Img.ActualHeight);

                if(x < 0 || x >= m_Data.Width || y < 0 || y >= m_Data.Height)
                    return;

                if (!PointedValuePopup.IsOpen) { PointedValuePopup.IsOpen = true; }

                PointedValuePopup.HorizontalOffset = pos.X + 20;
                PointedValuePopup.VerticalOffset = pos.Y;

                PointedValueText.Text = m_Data[x,y].ToString("E4");
            };

            Img.MouseLeave += (s, e) =>
            {
                PointedValuePopup.IsOpen = false;
            };
        }
    }
}
