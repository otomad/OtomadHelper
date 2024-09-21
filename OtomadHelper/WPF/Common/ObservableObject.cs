using System.Runtime.CompilerServices;

using IView = System.Windows.FrameworkElement;

namespace OtomadHelper.WPF.Common;

public interface IViewAccessibleViewModel {
	public IView? View { get; internal set; }
}

/// <inheritdoc/>
/// <remarks>
/// Add more options.
/// </remarks>
public class ObservableObject : CommunityToolkit.Mvvm.ComponentModel.ObservableObject {
	public event PropertyChangedEventHandler? PropertyNoChanged;

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed.</param>
	protected void OnPropertyChanged<T>(T newValue, T oldValue, [CallerMemberName] string? propertyName = null) =>
		OnPropertyChanged(new PropertyChangedEventArgs<T>(propertyName, newValue, oldValue));

	protected void OnPropertyNoChanged<T>(T newValue, T oldValue, [CallerMemberName] string? propertyName = null) =>
		PropertyNoChanged?.Invoke(this, new PropertyChangedEventArgs<T>(propertyName, newValue, oldValue));

	/// <summary>
	/// Raises the <see cref="PropertyChanging"/> event.
	/// </summary>
	/// <param name="propertyName">(optional) The name of the property that changed.</param>
	protected void OnPropertyChanging<T>(T newValue, T oldValue, [CallerMemberName] string? propertyName = null) =>
		OnPropertyChanging(new PropertyChangingEventArgs<T>(propertyName, newValue, oldValue));

	/// <inheritdoc cref="CommunityToolkit.Mvvm.ComponentModel.ObservableObject.SetProperty{T}(ref T, T, string?)"/>
	protected bool SetProperty<T>(ref T field, T value, bool condition = true, [CallerMemberName] string? propertyName = null) {
		if (!condition) return false;
		T oldValue = field;
		if (EqualityComparer<T>.Default.Equals(field, value)) {
			OnPropertyNoChanged(value, oldValue, propertyName);
			return false;
		}
		OnPropertyChanging(value, oldValue, propertyName);
		field = value;
		OnPropertyChanged(value, oldValue, propertyName);
		return true;
	}

	protected bool SetProperty<T>(T getField, Action<T> SetField, T value, bool condition = true, [CallerMemberName] string? propertyName = null) {
		bool result = SetProperty(ref getField, value, condition, propertyName);
		if (result) SetField(getField);
		return result;
	}

	protected bool SetProperty<T>(ref T field, T value, Func<bool> GetCondition, [CallerMemberName] string? propertyName = null) =>
		SetProperty(ref field, value, GetCondition(), propertyName);

	protected bool SetProperty<T>(T getField, Action<T> SetField, T value, Func<bool> GetCondition, [CallerMemberName] string? propertyName = null) =>
		SetProperty(getField, SetField, value, GetCondition(), propertyName);

	public delegate void NotifyPropertyChangeHandler<T>(T newValue, T? oldValue, string propertyName);

	public void NotifyPropertyChanged<T>(string propertyName, NotifyPropertyChangeHandler<T> Handler, bool triggerEvenIfNoChange = false) {
		PropertyChangedEventHandler OnPropertyChanged = (sender, _e) => {
			if (_e.PropertyName == propertyName) {
				dynamic e = _e;
				Handler(e.NewValue, e.OldValue, e.PropertyName);
			}
		};
		PropertyChanged += OnPropertyChanged;
		if (triggerEvenIfNoChange)
			PropertyNoChanged += OnPropertyChanged;
	}

	public void NotifyPropertyChanging<T>(string propertyName, NotifyPropertyChangeHandler<T> Handler) =>
		PropertyChanging += (sender, _e) => {
			if (_e.PropertyName == propertyName) {
				dynamic e = _e;
				Handler(e.NewValue, e.OldValue, e.PropertyName);
			}
		};
}

/// <inheritdoc/>
/// <typeparam name="TView">The type of View.</typeparam>
public class ObservableObject<TView> : ObservableObject, IViewAccessibleViewModel
	where TView : IView {

	IView? IViewAccessibleViewModel.View {
		get => View;
		set => View = value as TView;
	}

	/// <summary>
	/// Access View from ViewModel.
	/// </summary>
	public TView? View { get; set; }
}

public class PropertyChangedEventArgs<T>(string? propertyName, T? newValue, T? oldValue) : PropertyChangedEventArgs(propertyName) {
	public virtual T NewValue { get; private set; } = newValue!;
	public virtual T? OldValue { get; private set; } = oldValue;
}

public class PropertyChangingEventArgs<T>(string? propertyName, T? newValue, T? oldValue) : PropertyChangingEventArgs(propertyName) {
	public virtual T NewValue { get; private set; } = newValue!;
	public virtual T? OldValue { get; private set; } = oldValue;
}
