using System.Windows.Input;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Command base class.
/// </summary>
public class Command : ICommand {
	private readonly Func<object, bool>? canExecute;
	private readonly Action<object> execute;

	public event EventHandler CanExecuteChanged {
		add {
			if (canExecute != null)
				CommandManager.RequerySuggested += value;
		}
		remove {
			if (canExecute != null)
				CommandManager.RequerySuggested -= value;
		}
	}

	public bool CanExecute(object parameter) => canExecute == null || canExecute(parameter);

	public void Execute(object parameter) {
		if (execute != null && CanExecute(parameter))
			execute(parameter);
	}

	public Command(Action<object> execute, Func<object, bool>? canExecute) {
		this.execute = execute;
		this.canExecute = canExecute;
	}

	public Command(Action<object> execute) : this(execute, null) { }
}
