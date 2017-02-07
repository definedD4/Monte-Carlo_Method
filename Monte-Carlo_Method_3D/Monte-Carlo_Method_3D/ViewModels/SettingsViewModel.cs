using System;
using System.Reactive;
using System.Windows.Media;
using Monte_Carlo_Method_3D.AppSettings;
using Monte_Carlo_Method_3D.Visualization;
using Monte_Carlo_Method_3D.Visualization.GraphMesh.Factory;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private readonly Settings m_OldSettings;

        public SettingsViewModel()
        {
            m_OldSettings = Settings.Current;

            LoadProperties(m_OldSettings);

            Ok = ReactiveCommand.Create(() =>
            {
                var settings = EmitSettings();

                Settings.Current = settings;

                CloseDialog.Execute().Subscribe();
            });

            Apply = ReactiveCommand.Create(() =>
            {
                var settings = EmitSettings();

                Settings.Current = settings;
            });

            Cancel = ReactiveCommand.Create(() =>
            {
                Settings.Current = m_OldSettings;

                CloseDialog.Execute().Subscribe();
            });

            CloseDialog = ReactiveCommand.Create(() => Unit.Default);
        }

        private void LoadProperties(Settings settings)
        {
            VisluaizationTextEm = settings.VisualizationOptions.TextEmSize;
            VisualizationTextColor = settings.VisualizationOptions.ForegroundColor;
            VisualizationBackgroundColor = settings.VisualizationOptions.BackgroundColor;
            DrawGrid = settings.VisualizationOptions.DrawGrid;
            VisualizationGridColor = settings.VisualizationOptions.GridColor;
            VisualizationGridThickness = settings.VisualizationOptions.GridThickness;
        }

        private Settings EmitSettings()
        {
            var settings = m_OldSettings.Copy();

            settings.VisualizationOptions.TextEmSize = VisluaizationTextEm;
            settings.VisualizationOptions.ForegroundColor = VisualizationTextColor;
            settings.VisualizationOptions.BackgroundColor = VisualizationBackgroundColor;
            settings.VisualizationOptions.DrawGrid = DrawGrid;
            settings.VisualizationOptions.GridColor = VisualizationGridColor;
            settings.VisualizationOptions.GridThickness = VisualizationGridThickness;

            return settings;
        }

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Apply { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public ReactiveCommand<Unit, Unit> CloseDialog { get; }

        [Reactive]
        public double VisluaizationTextEm { get; set; }

        [Reactive]
        public Color VisualizationTextColor { get; set; }

        [Reactive]
        public Color VisualizationBackgroundColor { get; set; }

        [Reactive]
        public bool DrawGrid { get; set; }

        [Reactive]
        public Color VisualizationGridColor { get; set; }

        [Reactive]
        public double VisualizationGridThickness { get; set; }
    }
}