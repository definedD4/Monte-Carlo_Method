using Monte_Carlo_Method_3D.Visualization;
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

namespace Monte_Carlo_Method_3D.Dialogs
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        private VisualizationOptions m_Options;

        private VisualizationOptions m_OldOptions;

        public SettingsDialog()
        {
            InitializeComponent();

            m_Options = VisualizationOptions.Current;
            m_OldOptions = VisualizationOptions.Current;
        }

        public VisualizationOptions Options => m_Options;

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            VisualizationOptions.Current = m_Options;
            DialogResult = true;
            Close();
        }

        private void ApplyButtonClick(object sender, RoutedEventArgs e)
        {
            VisualizationOptions.Current = m_Options;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            VisualizationOptions.Current = m_OldOptions;
            DialogResult = false;
            Close();
        }
    }
}
