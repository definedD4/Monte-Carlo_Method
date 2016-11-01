﻿using System;
using System.Windows;
using System.Windows.Input;
using Monte_Carlo_Method_3D.Calculation;
using Monte_Carlo_Method_3D.Util;
using Monte_Carlo_Method_3D.Simulation;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public class СlTabViewModel : TabViewModel
    {
        private enum StateT
        {
            NotStarted,
            Working
        }

        private CalculationMethod m_CalculationMethod;
        private ConstraintChooserViewModel m_ConstraintChooser;
        private StateT m_State = StateT.NotStarted;
        private int m_GridWidth;
        private int m_GridHeight;
        private GridData m_EdgeData;

        private readonly DelegateCommand m_StartCommand;
        private readonly DelegateCommand m_StopCommand;

        private Calculation.Calculation m_Calculation;

        public СlTabViewModel() : base("Розрахунок")
        {
            CalculationMethod = CalculationMethod.Propability;

            m_StartCommand = new DelegateCommand(x =>
            {
                ICalculationConstraint constraint;
                try
                {
                    constraint = ConstraintChooser.GetConstraint();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Некоректно введенное ограничение.\nДетали:\n{e.Message}");
                    return;
                }
                if(GridWidth != EdgeData.Width || GridHeight != EdgeData.Height)
                {
                    MessageBox.Show($"Размеры сетки и таблицы граничных значений не совпадают.");
                    return;
                }
                m_Calculation = new PrCalculation(constraint, GridWidth, GridHeight, EdgeData);
                State = StateT.Working;
                m_Calculation.Start();
                m_Calculation.DoneCalculation += (s, e) =>
                {
                    //TODO: set result
                    State = StateT.NotStarted;
                };
                m_Calculation.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(Calculation.Calculation.Progress))
                    {
                        OnPropertyChanged(nameof(Progress));
                    }
                };
            }, x => State == StateT.NotStarted);

            m_StopCommand = new DelegateCommand(x =>
            {
                m_Calculation.Cancel();
            }, x => State == StateT.Working);
        }

        public CalculationMethod CalculationMethod
        {
            get { return m_CalculationMethod; }
            set
            {
                m_CalculationMethod = value;
                ConstraintChooser = new ConstraintChooserViewModel(m_CalculationMethod);
                OnPropertyChanged(nameof(CalculationMethod));
            }
        }

        public ConstraintChooserViewModel ConstraintChooser
        {
            get { return m_ConstraintChooser; }
            set { m_ConstraintChooser = value; OnPropertyChanged(nameof(ConstraintChooser)); }
        }

        private StateT State
        {
            get { return m_State; }
            set
            {
                m_State = value;

                m_StartCommand.RaiseCanExecuteChanged();
                m_StopCommand.RaiseCanExecuteChanged();
            }
        }

        public int Progress => m_Calculation?.Progress ?? 0;

        public int GridWidth
        {
            get { return m_GridWidth; }
            set { m_GridWidth = value; OnPropertyChanged(nameof(GridWidth)); }
        }

        public int GridHeight
        {
            get { return m_GridHeight; }
            set { m_GridHeight = value; OnPropertyChanged(nameof(GridHeight)); }
        }

        public GridData EdgeData
        {
            get { return m_EdgeData; }
            set { m_EdgeData = value; OnPropertyChanged(nameof(EdgeData)); }
        }

        public ICommand StartCommand => m_StartCommand;
    }
}