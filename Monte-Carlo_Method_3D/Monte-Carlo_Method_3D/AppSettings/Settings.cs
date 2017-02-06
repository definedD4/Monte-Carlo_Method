using System;
using System.Reactive;
using System.Reactive.Subjects;
using Monte_Carlo_Method_3D.Visualization;

namespace Monte_Carlo_Method_3D.AppSettings
{
    public class Settings
    {
        private static Settings s_Settings;

        public static Settings Current
        {
            get
            {
                if (s_Settings == null)
                {
                    // TODO: Load setting from file
                    s_Settings = new Settings(new VisualizationOptions());
                }

                return s_Settings;
            }
            set
            {
                s_Settings = value;

                s_SettingsChangeSubject.OnNext(Unit.Default);

                // TODO: Save settings
            }
        }

        private static Subject<Unit> s_SettingsChangeSubject = new Subject<Unit>();

        public static IObservable<Unit> SettingsChange => s_SettingsChangeSubject;

        public VisualizationOptions VisualizationOptions { get; }

        public Settings(VisualizationOptions visualizationOptions)
        {
            VisualizationOptions = visualizationOptions;
        }

        public Settings Copy()
        {
            return (Settings)MemberwiseClone();
        }
    }
}