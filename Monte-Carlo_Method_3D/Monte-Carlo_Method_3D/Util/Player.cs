using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Monte_Carlo_Method_3D.Util
{
    public class Player
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
        public bool Playing { get; private set; } = false;

        /// <summary>
        /// Indicates whether any step operations is running
        /// </summary>
        public bool Running { get; private set; } = false;

        /// <summary>
        /// Indicates whether running or playing is in progress
        /// </summary>
        public bool RunningOrPlaying => Running || Playing;

        /// <summary>
        /// Indicates whether single step operation is running
        /// </summary>
        public bool SingleStepRunning => Running && !Playing;

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
