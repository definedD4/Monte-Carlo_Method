using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Dictionary<string, List<string>> propertyDependencies = new Dictionary<string, List<string>>();

        protected void AddListenedObject(INotifyPropertyChanged obj)
        {
            obj.PropertyChanged += (s, e) =>
            {
                if (propertyDependencies.ContainsKey(e.PropertyName))
                {
                    var list = propertyDependencies[e.PropertyName];
                    foreach (var dep in list)
                    {
                        OnPropertyChanged(dep);
                    }
                }
            };
        }

        protected void RegisterPropertyDependency(string source, string target)
        {
            if(propertyDependencies.ContainsKey(source))
            {
                propertyDependencies[source].Add(target);
            }
            else
            {
                var list = new List<string>();
                list.Add(target);
                propertyDependencies.Add(source, list);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name.");
            }

            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, e);
            }
        }
    }
}
