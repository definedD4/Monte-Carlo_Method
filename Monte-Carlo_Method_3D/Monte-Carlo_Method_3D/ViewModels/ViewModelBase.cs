﻿using System;
using System.ComponentModel;
using ReactiveUI;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
        protected void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name.");
            }

            this.RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(e.PropertyName);
        }
    }
}
