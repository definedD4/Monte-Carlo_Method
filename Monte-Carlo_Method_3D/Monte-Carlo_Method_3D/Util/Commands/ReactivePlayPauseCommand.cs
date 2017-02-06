using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.Util.Commands
{
    public class ReactivePlayPauseCommand : ReactiveObject, ICommand, IObservable<bool>
    {
        public string PlayText { get; }

        public string PauseText { get; }

        public string Text { [ObservableAsProperty] get; }

        [Reactive] public bool Playing { get; private set; }

        private readonly Subject<bool> m_ExecutionSubject = new Subject<bool>();

        private readonly IObservable<bool> m_CanExecute;

        private bool CanExecuteValue { [ObservableAsProperty] get; }

        public ReactivePlayPauseCommand(string playText, string pauseText,
            IObservable<bool> canExecute, bool playing = false)
        {
            PlayText = playText;
            PauseText = pauseText;
            m_CanExecute = canExecute;

            this
                .WhenAnyValue(x => x.Playing)
                .Select(p => p ? PauseText : PlayText)
                .ToPropertyEx(this, x => x.Text);

            m_CanExecute
                .ToPropertyEx(this, x => x.CanExecuteValue);

            m_CanExecute
                .Subscribe(c =>
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                });

            Playing = playing;
        }

        public ReactivePlayPauseCommand(string playText, string pauseText, bool playing = false)
            : this(playText, pauseText, Observable.Return(true), playing)
        {
            
        }

        public bool CanExecute(object parameter) => CanExecuteValue;

        public void Execute(object parameter)
        {
            Playing = !Playing;
            m_ExecutionSubject.OnNext(Playing);
        }

        public event EventHandler CanExecuteChanged;

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            return m_ExecutionSubject.Subscribe(observer);
        }
    }
}
