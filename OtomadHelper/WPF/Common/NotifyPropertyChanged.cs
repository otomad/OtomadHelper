using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OtomadHelper.WPF.Common;

public abstract class NotifyPropertyChanged : INotifyPropertyChanged {
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Notify the event changed.
	/// </summary>
	/// <param name="propertyName">Property name.</param>
	protected virtual void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// Set the notify property field.
	/// </summary>
	/// <typeparam name="T">Type of the property field.</typeparam>
	/// <param name="field">The property field.</param>
	/// <param name="value">The new value to be set.</param>
	/// <param name="propertyName">The property name, no need to provide it, it will automatically get by C#.</param>
	/// <returns></returns>
	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "") {
		if (EqualityComparer<T>.Default.Equals(field, value))
			return false;
		field = value;
		OnPropertyChanged(propertyName);
		return true;
	}
}
