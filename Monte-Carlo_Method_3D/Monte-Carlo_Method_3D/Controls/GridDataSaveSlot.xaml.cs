﻿using Microsoft.Win32;
using Monte_Carlo_Method_3D.Dialogs;
using Monte_Carlo_Method_3D.Util;
using System.Windows;
using System.Windows.Controls;
using Monte_Carlo_Method_3D.DataModel;

namespace Monte_Carlo_Method_3D.Controls
{
    /// <summary>
    /// Interaction logic for GridDataSaveSlot.xaml
    /// </summary>
    public partial class GridDataSaveSlot : UserControl
    {
        public GridData Data
        {
            get { return (GridData)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(GridData), typeof(GridDataSaveSlot), new PropertyMetadata(null, DataChanged));

        public GridDataSaveSlot()
        {
            InitializeComponent();

            DataChanged(this, new DependencyPropertyChangedEventArgs());
        }

        private static void DataChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var @this = dependencyObject as GridDataSaveSlot;

            if(@this.Data == null)
            {
                @this.BtnView.IsEnabled = false;
                @this.BtnSave.IsEnabled = false;
                @this.TbName.Text = "";
            }
            else
            {
                @this.BtnView.IsEnabled = true;
                @this.BtnSave.IsEnabled = true;
                @this.TbName.Text = $"Таблица: {@this.Data.Size.Height} на {@this.Data.Size.Width}";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(Data != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Файл CSV (*.csv)|*.csv" };
                if (saveFileDialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
                {
                    CsvUtil.ExportToFile(Data, saveFileDialog.FileName);
                }
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            if(Data != null)
            {
                ShowTableDialog.ShowNew(Data);
            }
        }
    }
}
