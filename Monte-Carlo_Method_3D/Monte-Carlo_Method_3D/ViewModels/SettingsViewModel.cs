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
            m_OldSettings = Settings.Current.Copy();

            LoadProperties(m_OldSettings);

            CloseDialog = ReactiveCommand.Create(() => Unit.Default);

            Ok = ReactiveCommand.Create(() =>
            {
                var settings = EmitSettings();

                Settings.Current = settings;
            });

            Ok.InvokeCommand(CloseDialog);

            Apply = ReactiveCommand.Create(() =>
            {
                var settings = EmitSettings();

                Settings.Current = settings;
            });

            Cancel = ReactiveCommand.Create(() =>
            {
                Settings.Current = m_OldSettings;
            });

            Cancel.InvokeCommand(CloseDialog);
        }

        private void LoadProperties(Settings settings)
        {
            VisluaizationTextEm = settings.VisualizationOptions.TextEmSize;
            VisualizationTextColor = settings.VisualizationOptions.ForegroundColor;
            VisualizationBackgroundColor = settings.VisualizationOptions.BackgroundColor;
            DrawGrid = settings.VisualizationOptions.DrawGrid;
            VisualizationGridColor = settings.VisualizationOptions.GridColor;
            VisualizationGridThickness = settings.VisualizationOptions.GridThickness;
            DrawStartPoint = settings.VisualizationOptions.DrawStartPoint;
            StartPointColor = settings.VisualizationOptions.StartPointColor;
            DrawEndPoint = settings.VisualizationOptions.DrawEndPoint;
            EndPointColor = settings.VisualizationOptions.EndPointColor;
            PathColor = settings.VisualizationOptions.PathColor;
            PathThickness = settings.VisualizationOptions.PathThickness;
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
            settings.VisualizationOptions.DrawStartPoint = DrawStartPoint;
            settings.VisualizationOptions.StartPointColor = StartPointColor;
            settings.VisualizationOptions.DrawEndPoint = DrawEndPoint;
            settings.VisualizationOptions.EndPointColor = EndPointColor;
            settings.VisualizationOptions.PathColor = PathColor;
            settings.VisualizationOptions.PathThickness = PathThickness;

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

        [Reactive]
        public bool DrawStartPoint { get; set; }

        [Reactive]
        public Color StartPointColor { get; set; }

        [Reactive]
        public bool DrawEndPoint { get; set; }

        [Reactive]
        public Color EndPointColor { get; set; }

        [Reactive]
        public Color PathColor { get; set; }

        [Reactive]
        public double PathThickness { get; set; }
    }
}