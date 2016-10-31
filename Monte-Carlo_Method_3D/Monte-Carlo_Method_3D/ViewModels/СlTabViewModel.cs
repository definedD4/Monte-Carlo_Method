using System;
using System.Windows;
using System.Windows.Input;
using Monte_Carlo_Method_3D.Calculation;
using Monte_Carlo_Method_3D.Util;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class СlTabViewModel : TabViewModel
    {
        private enum StateT
        {
            NotStarted,
            Working,
            Paused
        }

        private CalculationMethod m_CalculationMethod;
        private ConstraintChooserViewModel m_ConstraintChooser;
        private StateT m_State = StateT.NotStarted;
        private string m_OutputPath;

        private readonly DelegateCommand m_StartCommand;
        private readonly SwitchStateCommand m_PauseResumeCommand;
        private readonly DelegateCommand m_StopCommand;

        public СlTabViewModel() : base("Розрахунок")
        {
            CalculationMethod = CalculationMethod.Propability;

            m_StartCommand = new DelegateCommand(x =>
            {
                ICalculationConstraint constraint;
                try
                {
                    constraint = ConstraintChooser.GetConstraint();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Некоректно введенное ограничение.\nДетали:\n{e.Message}");
                    return;
                }
                MessageBox.Show($"{constraint.ToString()}");
            }, x => State == StateT.NotStarted);
        }

        public CalculationMethod CalculationMethod
        {
            get { return m_CalculationMethod; }
            set
            {
                m_CalculationMethod = value;
                ConstraintChooser = new ConstraintChooserViewModel(m_CalculationMethod);
                OnPropertyChanged(nameof(CalculationMethod));
            }
        }

        public ConstraintChooserViewModel ConstraintChooser
        {
            get { return m_ConstraintChooser; }
            set { m_ConstraintChooser = value; OnPropertyChanged(nameof(ConstraintChooser)); }
        }

        private StateT State
        {
            get { return m_State; }
            set
            {
                m_State = value;

                m_StartCommand.RaiseCanExecuteChanged();
                m_PauseResumeCommand.RaiseCanExecuteChanged();
                m_StopCommand.RaiseCanExecuteChanged();
            }
        }


        public string OutputPath
        {
            get { return m_OutputPath; }
            set { m_OutputPath = value;  OnPropertyChanged(nameof(OutputPath)); }
        }

        public ICommand StartCommand => m_StartCommand;
    }
}