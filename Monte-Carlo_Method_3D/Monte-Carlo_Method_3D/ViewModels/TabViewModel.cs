using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monte_Carlo_Method_3D.ViewModels
{
    public abstract class TabViewModel : ViewModelBase
    {
        public string Header { get; protected set; }

        public TabViewModel(string header) : base()
        {
            Header = header;
        }
    }
}
