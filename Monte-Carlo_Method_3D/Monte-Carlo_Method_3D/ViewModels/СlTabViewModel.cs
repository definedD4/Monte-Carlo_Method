using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Monte_Carlo_Method_3D.Calculation;
using Monte_Carlo_Method_3D.DataModel;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Simulation;
using Monte_Carlo_Method_3D.Util.Commands;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class СlTabViewModel : TabViewModel
    {
        private enum CalculationState
        {
            NotStarted,
            Working
        }

        [Reactive]
        public CalculationMethod CalculationMethod { get; set; }

        public IEnumerable<CalculationConstraintCreator> ConstraintCreators { [ObservableAsProperty] get; }

        public string ConstraintArgumentName { [ObservableAsProperty] get; }

        [Reactive]
        public CalculationConstraintCreator SelectedConstraintCreator { get; set; }

        [Reactive]
        public string ConstraintArgument { get; set; }

        [Reactive]
        private CalculationState State { get; set; }

        [Reactive]
        public string CalculationMask { get; set; }

        [Reactive]
        public GridData EdgeData { get; set; }

        [Reactive]
        public GridData Result { get; private set; }

        [Reactive]
        public double Progress { get; private set; }

        public ReactiveCommand<Unit, Unit> Start { get; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        private Calculation.Calculation m_Calculation;

        public СlTabViewModel() : base("Розрахунок")
        {
            var logger = Logger.New(typeof(CpTabViewModel));

            using (logger.LogPerf("Init"))
            {
                this
                    .WhenAnyValue(x => x.CalculationMethod)
                    .Where(method => method != null)
                    .Select(method => method.AvailableConstraints)
                    .ToPropertyEx(this, x => x.ConstraintCreators);

                this
                    .WhenAnyValue(x => x.CalculationMethod)
                    .Where(method => method != null)
                    .Select(method => method.AvailableConstraints.First())
                    .Subscribe(creator =>
                    {
                        SelectedConstraintCreator = creator;
                    });

                this
                    .WhenAnyValue(x => x.SelectedConstraintCreator)
                    .Where(creator => creator != null)
                    .Select(creator => creator.ArgumentDisplayName)
                    .ToPropertyEx(this, x => x.ConstraintArgumentName);

                var working = this
                    .WhenAnyValue(x => x.State)
                    .Select(s => s == CalculationState.Working);

                Start = ReactiveCommand.Create(() =>
                {
                    CalculationConstraint constraint;
                    try
                    {
                        constraint = SelectedConstraintCreator.Create(ConstraintArgument);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Некоректно задані обмеження симуляції.\nДеталі:\n{e.Message}");
                        return;
                    }
                    if (EdgeData == null)
                    {
                        MessageBox.Show($"Не заданні значення на межі сітки.");
                        return;
                    }

                    List<GridIndex> calcMask = new List<GridIndex>();

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(CalculationMask))
                        {
                            calcMask.AddRange(CalculationMask.Split(';').Select(
                                i =>
                                {
                                    var coords = i.Trim().Split(',').Select(j => int.Parse(j.Trim())).ToArray();
                                    return new GridIndex(coords[0], coords[1]);
                                }));
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Некоректно задані вузли для розрахунку.\nДетали:\n{e.Message}");
                        return;
                    }

                    if (CalculationMethod == CalculationMethod.Propability)
                    {
                        m_Calculation = new PrCalculation(constraint, EdgeData.AsEdgeData(), calcMask);
                    }
                    else if (CalculationMethod == CalculationMethod.Statistical)
                    {
                        m_Calculation = new StCalculation(constraint, EdgeData.AsEdgeData(), calcMask);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid calculation method selected: {CalculationMethod}.");
                    }

                    m_Calculation.Result
                        .Subscribe(result =>
                        {
                            Result = result;
                            State = CalculationState.NotStarted;
                        });

                    m_Calculation.Progress
                        .Subscribe(progress =>
                        {
                            Progress = progress;
                        });

                    State = CalculationState.Working;
                    m_Calculation.Start();
                }, working.Select(x => !x));

                CalculationMethod = CalculationMethod.Propability;

                State = CalculationState.NotStarted;

                Progress = 0;

                Cancel = ReactiveCommand.Create(() =>
                {
                    m_Calculation.Cancel();
                    State = CalculationState.NotStarted;
                }, working);
            }
        }
    }
}