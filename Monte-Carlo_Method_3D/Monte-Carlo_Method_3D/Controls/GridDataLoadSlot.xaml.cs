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
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Controls
{
    /// <summary>
    /// Interaction logic for GridDataLoadSlot.xaml
    /// </summary>
    public partial class GridDataLoadSlot : UserControl
    {
        public GridData Data
        {
            get { return (GridData)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(GridData), typeof(GridDataLoadSlot), new PropertyMetadata(null, DataChanged));

        private static void DataChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            GridDataLoadSlot @this = dependencyObject as GridDataLoadSlot;
            if (@this.Data != null)
            {
                @this.TbName.Text = $"Таблица: {@this.Data.Width} на {@this.Data.Height}";
                @this.TbName.Visibility = Visibility.Visible;
                @this.BtnLoad.Visibility = Visibility.Collapsed;
                @this.BtnViewData.IsEnabled = true;
            }
            else
            {
                @this.TbName.Visibility = Visibility.Collapsed;
                @this.BtnLoad.Visibility = Visibility.Visible;
                @this.BtnViewData.IsEnabled = false;
            }
        }

        public GridDataLoadSlot()
        {
            InitializeComponent();
        }

        private void BtnLoad_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnViewData_OnClick(object sender, RoutedEventArgs e)
        {
            ShowTableDialog.ShowNew(Data);
        }
    }
}
