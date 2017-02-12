using System;
using System.Reactive.Linq;
using System.Windows.Threading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.Util
{
    public class Player : ReactiveObject
    {
        // Settings
        public TimeSpan TimerInterval { get; }

        public DispatcherPriority DispatcherPriority { get; }

        // Actions
        private readonly Action m_StepOnce;

        private readonly Action m_StepAuto;

        private readonly Action m_AfterStep;

        // Readonly objects
        private readonly DispatcherTimer m_Timer;

        // State objects

        /// <summary>
        /// Indicates whether automatic playing is on
        /// </summary>
        [Reactive] public bool Playing { get; private set; } = false;

        /// <summary>
        /// Indicates whether any step operations is running
        /// </summary>
        [Reactive] public bool Running { get; private set; } = false;

        /// <summary>
        /// Indicates whether running or playing is in progress
        /// </summary>
        public bool RunningOrPlaying { [ObservableAsProperty] get; }

        /// <summary>
        /// Indicates whether single step operation is running
        /// </summary>
        public bool SingleStepRunning { [ObservableAsProperty]get; }

        public Player(Action stepOnce, Action stepAuto, Action afterStep, TimeSpan timerInterval, DispatcherPriority dispatcherPriority)
        {
            m_StepOnce = stepOnce;
            m_StepAuto = stepAuto;
            m_AfterStep = afterStep;
            TimerInterval = timerInterval;
            DispatcherPriority = dispatcherPriority;

            m_Timer = new DispatcherTimer(DispatcherPriority) { Interval = TimerInterval };
            m_Timer.Tick += (sender, args) =>
            {
                if (Running)
                {
                    // Can't keep up with intervals
                    Console.WriteLine($"[Player] Cant't keep up with intervals. Current interval is {TimerInterval}.");
                    return;
                }

                Running = true;

                m_StepAuto();

                Running = false;

                m_AfterStep();
            };

            this.WhenAnyValue(x => x.Running, x => x.Playing)
                .Select(t => t.Item1 || t.Item2)
                .ToPropertyEx(this, x => x.RunningOrPlaying);

            this.WhenAnyValue(x => x.Running, x => x.Playing)
                .Select(t => t.Item1 && !t.Item2)
                .ToPropertyEx(this, x => x.SingleStepRunning);
        }

        public void StepOnce()
        {
            if (Playing)
            { 
                return;
            }

            if (Running)
            {
                return;
            }

            Running = true;

            m_StepOnce();

            Running = false;

            m_AfterStep();
        }

        public void Start()
        {
            if (Playing)
            {
                return;
            }

            m_Timer.Start();

            Playing = true;
        }

        public void Stop()
        {
            if (!Playing)
            {
                return;
            }

            m_Timer.Stop();

            Playing = false;
        }
    }
}
