/// <summary>
/// <see href="https://github.com/CommunityToolkit/dotnet">CommunityToolkit.Mvvm</see>
/// <para>Cannot use CommunityToolkit.Mvvm, or VEGAS Pro won't recognize the extension.</para>
/// </summary>

using System.Runtime.CompilerServices;

using IView = System.Windows.FrameworkElement;

namespace OtomadHelper.WPF.Common;

public interface IViewAccessibleViewModel {
	public IView? View { get; internal set; }
}

/// <summary>
/// A base class for objects of which the properties must be observable.
/// </summary>
public class ObservableObject : INotifyPropertyChanged {
	/// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <inheritdoc cref="INotifyPropertyChanging.PropertyChanging"/>
	public event PropertyChangingEventHandler? PropertyChanging;

	/// <summary>
	/// Raises the <see cref="PropertyChanged"/> event.
	/// </summary>
	/// <param name="propertyName">The name of the property that changed.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// Raises the <see cref="PropertyChanging"/> event.
	/// </summary>
	/// <param name="propertyName">(optional) The name of the property that changed.</param>
	protected void OnPropertyChanging([CallerMemberName] string? propertyName = null) =>
		PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

	/// <summary>
	/// Compares the current and new values for a given property. If the value has changed,
	/// raises the <see cref="PropertyChanging"/> event, updates the property with the new
	/// value, then raises the <see cref="PropertyChanged"/> event.
	/// </summary>
	/// <typeparam name="T">The type of the property that changed.</typeparam>
	/// <param name="field">The field storing the property's value.</param>
	/// <param name="newValue">The property's value after the change occurred.</param>
	/// <param name="propertyName">(optional) The name of the property that changed.</param>
	/// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
	/// <remarks>
	/// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
	/// if the current and new value for the target property are the same.
	/// </remarks>
	protected bool SetProperty<T>(ref T field, T value, bool condition = true, [CallerMemberName] string? propertyName = null) {
		// We duplicate the code here instead of calling the overload because we can't
		// guarantee that the invoked SetProperty<T> will be inlined, and we need the JIT
		// to be able to see the full EqualityComparer<T>.Default.Equals call, so that
		// it'll use the intrinsics version of it and just replace the whole invocation
		// with a direct comparison when possible (eg. for primitive numeric types).
		// This is the fastest SetProperty<T> overload so we particularly care about
		// the codegen quality here, and the code is small and simple enough so that
		// duplicating it still doesn't make the whole class harder to maintain.
		if (!condition || EqualityComparer<T>.Default.Equals(field, value)) return false;
		OnPropertyChanging(propertyName);
		field = value;
		OnPropertyChanged(propertyName);
		return true;
	}

	protected bool SetProperty<T>(ref T field, T value, Func<bool> GetCondition, [CallerMemberName] string? propertyName = null) =>
		SetProperty(ref field, value, GetCondition(), propertyName);

	private readonly Dictionary<string, object> relayCommands = new();

	private TAction GetRelayCommand<TAction>(string commandName, TAction execute) =>
		(TAction)relayCommands.GetOrInit(commandName, RelayCommand.ThrowIfNull(execute)!);

	protected RelayCommand<T> DefineCommand<T>(Action<T> execute, [CallerMemberName] string? commandName = null) =>
		new(GetRelayCommand(commandName!, execute));
	protected RelayCommand DefineCommand(Action execute, [CallerMemberName] string? commandName = null) =>
		new(GetRelayCommand(commandName!, execute));
}

/// <inheritdoc/>
/// <typeparam name="TView">The type of View.</typeparam>
public class ObservableObject<TView> : ObservableObject, IViewAccessibleViewModel
	where TView : IView {
	IView? IViewAccessibleViewModel.View { get => View; set => View = value as TView; }

	/// <summary>
	/// Access View from ViewModel.
	/// </summary>
	public TView? View { get; set; }
}
