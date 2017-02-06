using System;
using System.Reactive;
using Monte_Carlo_Method_3D.AppSettings;
using Monte_Carlo_Method_3D.Visualization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private Settings m_OldSettings;

        public SettingsViewModel()
        {
            m_OldSettings = Settings.Current;

            LoadProperties(m_OldSettings);

            Ok = ReactiveCommand.Create(() =>
            {
                var settings = EmitSettings();

                Settings.Current = settings;

                CloseDialog.Execute().Subscribe(_ => { });
            });

            Apply = ReactiveCommand.Create(() =>
            {
                var settings = EmitSettings();

                Settings.Current = settings;
            });

            Cancel = ReactiveCommand.Create(() =>
            {
                Settings.Current = m_OldSettings;

                CloseDialog.Execute().Subscribe(_ => { });
            });

            CloseDialog = ReactiveCommand.Create(() => Unit.Default);
        }

        private void LoadProperties(Settings settings)
        {
            VisluaizationTextEm = settings.VisualizationOptions.TextEmSize;
        }

        private Settings EmitSettings()
        {
            var settings = m_OldSettings.Copy();

            settings.VisualizationOptions.TextEmSize = VisluaizationTextEm;

            return settings;
        }

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Apply { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public ReactiveCommand<Unit, Unit> CloseDialog { get; }

        [Reactive]
        public double VisluaizationTextEm { get; set; }
    }
}