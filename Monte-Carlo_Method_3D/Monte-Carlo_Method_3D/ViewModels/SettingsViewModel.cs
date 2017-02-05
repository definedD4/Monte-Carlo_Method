using System;
using System.Reactive;
using Monte_Carlo_Method_3D.Visualization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private VisualizationOptions m_OldOptions;

        public SettingsViewModel()
        {
            m_OldOptions = VisualizationOptions.Current;

            LoadProperties(m_OldOptions);

            Ok = ReactiveCommand.Create(() =>
            {
                var visualizationOptions = EmitOptions();

                VisualizationOptions.Current = visualizationOptions;

                CloseDialog.Execute().Subscribe(_ => { });
            });

            Apply = ReactiveCommand.Create(() =>
            {
                var visualizationOptions = EmitOptions();

                VisualizationOptions.Current = visualizationOptions;
            });

            Cancel = ReactiveCommand.Create(() =>
            {
                VisualizationOptions.Current = m_OldOptions;

                CloseDialog.Execute().Subscribe(_ => { });
            });

            CloseDialog = ReactiveCommand.Create(() => Unit.Default);
        }

        private void LoadProperties(VisualizationOptions options)
        {
            VisluaizationTextEm = options.TextEmSize;
        }

        private VisualizationOptions EmitOptions()
        {
            var opt = m_OldOptions.Copy();

            opt.TextEmSize = VisluaizationTextEm;

            return opt;
        }

        public ReactiveCommand<Unit, Unit> Ok { get; }

        public ReactiveCommand<Unit, Unit> Apply { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public ReactiveCommand<Unit, Unit> CloseDialog { get; }

        [Reactive]
        public double VisluaizationTextEm { get; set; }
    }
}