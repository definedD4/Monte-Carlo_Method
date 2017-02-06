using System;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.Util.Commands
{
    public class ReactiveSelectorCommand<T> : ReactiveObject, ICommand
    {
        [Reactive] private T SelectedOption { get; set; }

        public IObservable<T> Selected { get; }

        private Func<object, T> m_Converter;

        public ReactiveSelectorCommand(Func<object, T> converter, T defaultOption)
        {
            m_Converter = converter;

            Selected = this
                .WhenAnyValue(x => x.SelectedOption);

            Selected
                .Subscribe(_ =>
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                });

            SelectedOption = defaultOption;
        }

        public bool CanExecute(object parameter)
        {
            var val = m_Converter(parameter);

            return !object.Equals(SelectedOption, val);
        }

        public void Execute(object parameter)
        {
            var val = m_Converter(parameter);

            SelectedOption = val;
        }

        public event EventHandler CanExecuteChanged;
    }
}