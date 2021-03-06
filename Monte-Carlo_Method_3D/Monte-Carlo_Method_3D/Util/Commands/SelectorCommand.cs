﻿using System;
using System.Windows.Input;

namespace Monte_Carlo_Method_3D.Util.Commands
{
    public class SelectorCommand : ICommand
    {
        private string m_SelectedValue;

        public SelectorCommand(string defaultValue)
        {
            SelectedValue = defaultValue;
        }

        public void Execute(object argument)
        {
            string value = (string)argument;

            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Invalid argument.");

            SelectedValue = value;
        }

        public bool CanExecute(object argument)
        {
            string value = (string)argument;

            if (String.IsNullOrWhiteSpace(value))
                return true;

            return SelectedValue != value;
        }

        public string SelectedValue
        {
            get
            {
                return m_SelectedValue;
            }
            set
            {
                if(m_SelectedValue != value)
                {
                    m_SelectedValue = value;

                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler SelectionChanged;

        public void UpdateSelectors()
        {
                CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public void RaiseSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
