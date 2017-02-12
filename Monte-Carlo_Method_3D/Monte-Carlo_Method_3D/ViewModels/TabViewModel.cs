namespace Monte_Carlo_Method_3D.ViewModels
{
    public abstract class TabViewModel : ViewModelBase
    {
        public string Header { get; protected set; }

        protected TabViewModel(string header) : base()
        {
            Header = header;
        }
    }
}
