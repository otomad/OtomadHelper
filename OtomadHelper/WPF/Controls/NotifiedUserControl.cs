using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;
public abstract class NotifiedUserControl : UserControl, INotifyPropertyChanged {
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
