using Monte_Carlo_Method_3D.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для SimulationOptionsDialog.xaml
    /// </summary>
    public partial class SimulationOptionsDialog : Window, INotifyPropertyChanged
    {
        public SimulationOptionsDialog(int width, int height, IntPoint startLoc)
        {
            InitializeComponent();
            DataContext = this;

            WidthSetting = width;
            HeightSetting = height;
            StartXSetting = startLoc.X;
            StartYSetting = startLoc.Y;
        }


        private int p_WidthSetting;
        public int WidthSetting
        {
            get { return p_WidthSetting; }
            set
            {
                if (p_WidthSetting != value)
                {
                    p_WidthSetting = value; OnPropertyChanged("WidthSetting");
                }
            }
        }


        private int p_HeightSetting;
        public int HeightSetting
        {
            get { return p_HeightSetting; }
            set
            {
                if (p_HeightSetting != value)
                {
                    p_HeightSetting = value; OnPropertyChanged("HeightSetting");
                }
            }
        }

        private int p_StartXSetting;
        public int StartXSetting
        {
            get { return p_StartXSetting; }
            set
            {
                if (p_StartXSetting != value)
                {
                    p_StartXSetting = value; OnPropertyChanged("StartXSetting");
                }
            }
        }

        private int p_StartYSetting;
        public int StartYSetting
        {
            get { return p_StartYSetting; }
            set
            {
                if (p_StartYSetting != value)
                {
                    p_StartYSetting = value; OnPropertyChanged("StartYSetting");
                }
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (VerifyData())
            {
                DialogResult = true;
            }
        }

        private bool VerifyData()
        {
            if(StartXSetting < 0 || StartXSetting >= WidthSetting)
            {
                MessageBox.Show("X координата стартовой позиции вне границ. Она должна быть больше нуля и меньше ширины поля.");
                return false;
            }
            else if (StartYSetting < 0 || StartYSetting >= HeightSetting)
            {
                MessageBox.Show("Y координата стартовой позиции вне границ. Она должна быть больше нуля и меньше высоты поля.");
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name.");
            }

            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
