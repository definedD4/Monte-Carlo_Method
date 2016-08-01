using Monte_Carlo_Method_3D.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Monte_Carlo_Method_3D.Util
{
    public class SwitchStateCommand : ViewModelBase, ICommand
    {
        private bool m_State;
        private string m_TrueLabel;
        private string m_FalseLabel;
        private Func<object, bool> m_CanExecute;

        public SwitchStateCommand(string trueLabel, string falseLabel, bool state, Func<object, bool> canExecute)
        {
            TrueLabel = trueLabel;
            FalseLabel = falseLabel;
            m_State = state;
            m_CanExecute = canExecute ?? (_ => true);
        }

        public bool State => m_State;

        public string TrueLabel
        {
            get { return m_TrueLabel; }
            set { m_TrueLabel = value; OnPropertyChanged(nameof(TrueLabel)); OnPropertyChanged(nameof(Label)); }
        }

        public string FalseLabel
        {
            get { return m_FalseLabel; }
            set { m_FalseLabel = value; OnPropertyChanged(nameof(FalseLabel)); OnPropertyChanged(nameof(Label)); }
        }

        public string Label => State ? TrueLabel : FalseLabel;

        public void SwitchState()
        {
            m_State = !m_State;
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(Label));

            StateChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler StateChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public void Execute(object _)
        {
            SwitchState();
        }

        public bool CanExecute(object x)
        {
            return m_CanExecute(x);
        }

        public event EventHandler CanExecuteChanged;
    }
}
