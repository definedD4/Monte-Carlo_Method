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
using Microsoft.Win32;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Exceptions;
using Monte_Carlo_Method_3D.Util;

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
                @this.TbName.Text = $"Таблица: {@this.Data.Size.Height} на {@this.Data.Size.Width}";
                @this.BtnViewData.IsEnabled = true;
            }
            else
            {
                @this.TbName.Text = "";
                @this.BtnViewData.IsEnabled = false;
            }
        }

        public GridDataLoadSlot()
        {
            InitializeComponent();
        }

        private void BtnLoad_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
            if (ofd.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
            {
                try
                {
                    Data = CsvUtil.ImportFromFile(ofd.FileName);
                }
                catch (TableLoadException exception)
                {
                    MessageBox.Show(Application.Current.MainWindow,
                        "При завантаженні таблиці виникла помилка. Перевірте цілісність даних.\n" +
                        "Деталі:\n" +
                        $"{exception.InnerException?.Message}");
                }
            }
        }

        private void BtnViewData_OnClick(object sender, RoutedEventArgs e)
        {
            ShowTableDialog.ShowNew(Data);
        }
    }
}
