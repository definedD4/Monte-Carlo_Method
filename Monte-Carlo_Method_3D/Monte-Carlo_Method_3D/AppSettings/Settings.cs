using System;
using System.IO;
using System.Reactive;
using System.Reactive.Subjects;
using Monte_Carlo_Method_3D.Visualization;
using Newtonsoft.Json;

namespace Monte_Carlo_Method_3D.AppSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        private const string SettingsFilePath = "settings.json";
        private static Settings s_Settings;

        public static Settings Current
        {
            get
            {
                if (s_Settings == null)
                {
                    if (!File.Exists(SettingsFilePath))
                    {
                        s_Settings = new Settings(new VisualizationOptions());
                        File.WriteAllText(SettingsFilePath, JsonConvert.SerializeObject(s_Settings));
                    }

                    s_Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsFilePath));
                }

                return s_Settings;
            }
            set
            {
                s_Settings = value;

                s_SettingsChangeSubject.OnNext(Unit.Default);

                File.WriteAllText(SettingsFilePath, JsonConvert.SerializeObject(s_Settings));
            }
        }

        private static readonly Subject<Unit> s_SettingsChangeSubject = new Subject<Unit>();

        public static IObservable<Unit> SettingsChange => s_SettingsChangeSubject;

        [JsonProperty]
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