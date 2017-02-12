using System;
using System.ComponentModel;
using System.Windows;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SimulationOptionsDialog.xaml
    /// </summary>
    public partial class SimulationOptionsDialog : Window, INotifyPropertyChanged
    {
        private int m_WidthSetting;
        private int m_HeightSetting;
        private int m_StartXSetting;
        private int m_StartYSetting;

        public SimulationOptionsDialog(SimulationOptions options)
        {
            InitializeComponent();
            DataContext = this;

            WidthSetting = options.Size.Width;
            HeightSetting = options.Size.Height;
            StartXSetting = options.StartLocation.J + 1;
            StartYSetting = options.StartLocation.I + 1;
        }

        public int WidthSetting
        {
            get { return m_WidthSetting; }
            set
            {
                if (m_WidthSetting != value)
                {
                    m_WidthSetting = value; OnPropertyChanged("WidthSetting");
                }
            }
        }

        public int HeightSetting
        {
            get { return m_HeightSetting; }
            set
            {
                if (m_HeightSetting != value)
                {
                    m_HeightSetting = value; OnPropertyChanged("HeightSetting");
                }
            }
        }

        public int StartXSetting
        {
            get { return m_StartXSetting; }
            set
            {
                if (m_StartXSetting != value)
                {
                    m_StartXSetting = value; OnPropertyChanged("StartXSetting");
                }
            }
        }

        public int StartYSetting
        {
            get { return m_StartYSetting; }
            set
            {
                if (m_StartYSetting != value)
                {
                    m_StartYSetting = value; OnPropertyChanged("StartYSetting");
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

        public SimulationOptions SimulationOptions => new SimulationOptions(new GridSize(WidthSetting, HeightSetting),
            new GridIndex(StartYSetting - 1, StartXSetting - 1));

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name.");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartXSetting = (int)Math.Round((double)WidthSetting / 2, MidpointRounding.AwayFromZero);
            StartYSetting = (int)Math.Round((double)HeightSetting / 2, MidpointRounding.AwayFromZero);
        }
    }
}
