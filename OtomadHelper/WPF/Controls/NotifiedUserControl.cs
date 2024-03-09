using System.ComponentModel;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;
public abstract class NotifiedUserControl : UserControl, INotifyPropertyChanged {
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
