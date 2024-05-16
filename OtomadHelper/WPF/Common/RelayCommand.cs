/// <summary>
/// <see href="https://github.com/CommunityToolkit/dotnet">CommunityToolkit.Mvvm</see>
/// <para>Cannot use CommunityToolkit.Mvvm, or VEGAS Pro won't recognize the extension.</para>
/// </summary>

using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// An interface expanding <see cref="ICommand"/> with the ability to raise
/// the <see cref="ICommand.CanExecuteChanged"/> event externally.
/// </summary>
public interface IRelayCommand : ICommand {
	/// <summary>
	/// Notifies that the <see cref="ICommand.CanExecute"/> property has changed.
	/// </summary>
	public void NotifyCanExecuteChanged();
}

/// <summary>
/// A command whose sole purpose is to relay its functionality to other
/// objects by invoking delegates. The default return value for the <see cref="CanExecute"/>
/// method is <see langword="true"/>. This type does not allow you to accept command parameters
/// in the <see cref="Execute"/> and <see cref="CanExecute"/> callback methods.
/// </summary>
public sealed class RelayCommand : IRelayCommand {
	/// <summary>
	/// The <see cref="Action"/> to invoke when <see cref="Execute"/> is used.
	/// </summary>
	private readonly Action execute;

	/// <summary>
	/// The optional action to invoke when <see cref="CanExecute"/> is used.
	/// </summary>
	private readonly Func<bool>? canExecute;

	public event EventHandler? CanExecuteChanged;

	/// <summary>
	/// Initializes a new instance of the <see cref="RelayCommand"/> class that can always execute.
	/// </summary>
	/// <param name="execute">The execution logic.</param>
	/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> is <see langword="null"/>.</exception>
	public RelayCommand(Action execute) {
		this.execute = ThrowIfNull(execute);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="RelayCommand"/> class.
	/// </summary>
	/// <param name="execute">The execution logic.</param>
	/// <param name="canExecute">The execution status logic.</param>
	/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> or <paramref name="canExecute"/> are <see langword="null"/>.</exception>
	public RelayCommand(Action execute, Func<bool> canExecute) {
		this.execute = ThrowIfNull(execute);
		this.canExecute = ThrowIfNull(canExecute);
	}

	public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool CanExecute(object? parameter) => canExecute?.Invoke() != false;

	public void Execute(object? parameter) => execute();

	/// <summary>
	/// Polyfill for <see cref="ArgumentNullException"/>.ThrowIfNull method which only available in .NET 6 and above.
	/// </summary>
	/// <returns>If <paramref name="argument"/> is not <see langword="null"/>, return the original <paramref name="argument"/> value.</returns>
	/// <exception cref="ArgumentNullException">Raise when <paramref name="argument"/> is <see langword="null"/>.</exception>
	internal static T ThrowIfNull<T>(T argument, [CallerMemberName] string argumentName = "") =>
		argument is null ? throw new ArgumentNullException(argumentName) : argument;
}

/// <summary>
/// A generic interface representing a more specific version of <see cref="IRelayCommand"/>.
/// </summary>
/// <typeparam name="T">The type used as argument for the interface methods.</typeparam>
public interface IRelayCommand<in T> : IRelayCommand {
	/// <summary>
	/// Provides a strongly-typed variant of <see cref="ICommand.CanExecute(object)"/>.
	/// </summary>
	/// <param name="parameter">The input parameter.</param>
	/// <returns>Whether or not the current command can be executed.</returns>
	/// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
	bool CanExecute(T parameter);

	/// <summary>
	/// Provides a strongly-typed variant of <see cref="ICommand.Execute(object)"/>.
	/// </summary>
	/// <param name="parameter">The input parameter.</param>
	/// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
	void Execute(T parameter);
}

/// <summary>
/// A generic command whose sole purpose is to relay its functionality to other
/// objects by invoking delegates. The default return value for the CanExecute
/// method is <see langword="true"/>. This class allows you to accept command parameters
/// in the <see cref="Execute(T)"/> and <see cref="CanExecute(T)"/> callback methods.
/// </summary>
/// <typeparam name="T">The type of parameter being passed as input to the callbacks.</typeparam>
public sealed class RelayCommand<T> : IRelayCommand<T> {
	/// <summary>
	/// The <see cref="Action"/> to invoke when <see cref="Execute(T)"/> is used.
	/// </summary>
	private readonly Action<T> execute;

	/// <summary>
	/// The optional action to invoke when <see cref="CanExecute(T)"/> is used.
	/// </summary>
	private readonly Predicate<T>? canExecute;

	public event EventHandler? CanExecuteChanged;

	/// <summary>
	/// Initializes a new instance of the <see cref="RelayCommand{T}"/> class that can always execute.
	/// </summary>
	/// <param name="execute">The execution logic.</param>
	/// <remarks>
	/// Due to the fact that the <see cref="System.Windows.Input.ICommand"/> interface exposes methods that accept a
	/// nullable <see cref="object"/> parameter, it is recommended that if <typeparamref name="T"/> is a reference type,
	/// you should always declare it as nullable, and to always perform checks within <paramref name="execute"/>.
	/// </remarks>
	/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> is <see langword="null"/>.</exception>
	public RelayCommand(Action<T> execute) {
		this.execute = RelayCommand.ThrowIfNull(execute);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
	/// </summary>
	/// <param name="execute">The execution logic.</param>
	/// <param name="canExecute">The execution status logic.</param>
	/// <remarks>See notes in <see cref="RelayCommand{T}(Action{T})"/>.</remarks>
	/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="execute"/> or <paramref name="canExecute"/> are <see langword="null"/>.</exception>
	public RelayCommand(Action<T> execute, Predicate<T> canExecute) {
		this.execute = RelayCommand.ThrowIfNull(execute);
		this.canExecute = RelayCommand.ThrowIfNull(canExecute);
	}

	public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool CanExecute(T parameter) => canExecute?.Invoke(parameter) != false;

	public bool CanExecute(object? parameter) {
		// Special case a null value for a value type argument type.
		// This ensures that no exceptions are thrown during initialization.
		if (parameter is null && default(T) is not null)
			return false;
		if (!TryGetCommandArgument(parameter, out T result))
			ThrowArgumentExceptionForInvalidCommandArgument(parameter);
		return CanExecute(result);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Execute(T parameter) => execute(parameter);

	public void Execute(object? parameter) {
		if (!TryGetCommandArgument(parameter, out T result))
			ThrowArgumentExceptionForInvalidCommandArgument(parameter);
		Execute(result);
	}

	/// <summary>
	/// Tries to get a command argument of compatible type <typeparamref name="T"/> from an input <see cref="object"/>.
	/// </summary>
	/// <param name="parameter">The input parameter.</param>
	/// <param name="result">The resulting <typeparamref name="T"/> value, if any.</param>
	/// <returns>Whether or not a compatible command argument could be retrieved.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool TryGetCommandArgument(object? parameter, out T result) {
		// If the argument is null and the default value of T is also null, then the
		// argument is valid. T might be a reference type or a nullable value type.
		if (parameter is null && default(T) is null) {
			result = default!;
			return true;
		}
		// Check if the argument is a T value, so either an instance of a type or a derived
		// type of T is a reference type, an interface implementation if T is an interface,
		// or a boxed value type in case T was a value type.
		if (parameter is T argument) {
			result = argument;
			return true;
		}
		result = default!;
		return false;
	}

	/// <summary>
	/// Throws an <see cref="ArgumentException"/> if an invalid command argument is used.
	/// </summary>
	/// <param name="parameter">The input parameter.</param>
	/// <exception cref="ArgumentException">Thrown with an error message to give info on the invalid parameter.</exception>
	internal static void ThrowArgumentExceptionForInvalidCommandArgument(object? parameter) {
		[MethodImpl(MethodImplOptions.NoInlining)]
		static Exception GetException(object? parameter) => parameter is null ?
			new ArgumentException($"Parameter \"{nameof(parameter)}\" (object) must not be null, as the command type requires an argument of type {typeof(T)}.", nameof(parameter)) :
			(Exception)new ArgumentException($"Parameter \"{nameof(parameter)}\" (object) cannot be of type {parameter.GetType()}, as the command type requires an argument of type {typeof(T)}.", nameof(parameter));
		throw GetException(parameter);
	}
}
