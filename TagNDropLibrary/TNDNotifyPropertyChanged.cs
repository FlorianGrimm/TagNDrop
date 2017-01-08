using System.ComponentModel;

namespace TagNDropLibrary {
    public class TNDNotifyPropertyChanged : INotifyPropertyChanged {
        public TNDNotifyPropertyChanged() { }

        public event PropertyChangedEventHandler PropertyChanged;

        [System.Diagnostics.DebuggerStepThrough]
        protected void OnPropertyChanged(string propertyName) { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }
}
