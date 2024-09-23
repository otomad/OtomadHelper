/// <summary>
/// <see href="https://github.com/JonghoL/EventBindingMarkup"/>
/// </summary>

using System.Reflection.Emit;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows;

namespace OtomadHelper.WPF.Common;

public class EventBindingExtension : MarkupExtension {
	public string? Command { get; set; }
	public string? CommandParameter { get; set; } // TODO: DependencyProperty

	public override object? ProvideValue(IServiceProvider serviceProvider) {
		if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget targetProvider)
			throw new InvalidOperationException();

		if (targetProvider.TargetObject is not FrameworkElement targetObject)
			throw new InvalidOperationException();

		if (targetProvider.TargetProperty is not MemberInfo memberInfo)
			throw new InvalidOperationException();

		if (string.IsNullOrWhiteSpace(Command)) {
			Command = memberInfo.Name.Replace("Add", "");
			Command = Command.Contains("Handler") ? Command.Replace("Handler", "Command") : Command + "Command";
		}

		return CreateHandler(memberInfo, Command!, targetObject.GetType());
	}

	private Type? GetEventHandlerType(MemberInfo memberInfo) {
		Type? eventHandlerType = null;
		if (memberInfo is EventInfo eventInfo)
			eventHandlerType = eventInfo.EventHandlerType;
		else if (memberInfo is MethodInfo methodInfo) {
			ParameterInfo[] pars = methodInfo.GetParameters();
			eventHandlerType = pars[1].ParameterType;
		}
		return eventHandlerType;
	}

	private object? CreateHandler(MemberInfo memberInfo, string cmdName, Type targetType) {
		Type? eventHandlerType = GetEventHandlerType(memberInfo);
		if (eventHandlerType is null) return null;

		MethodInfo? handlerInfo = eventHandlerType.GetMethod("Invoke");
		DynamicMethod? method = new("", handlerInfo.ReturnType, [
			handlerInfo.GetParameters()[0].ParameterType,
			handlerInfo.GetParameters()[1].ParameterType,
		]);

		ILGenerator? gen = method.GetILGenerator();
		gen.Emit(OpCodes.Ldarg, 0);
		gen.Emit(OpCodes.Ldarg, 1);
		gen.Emit(OpCodes.Ldstr, cmdName);
		if (CommandParameter is null)
			gen.Emit(OpCodes.Ldnull);
		else
			gen.Emit(OpCodes.Ldstr, CommandParameter);
		gen.Emit(OpCodes.Call, getMethod);
		gen.Emit(OpCodes.Ret);

		return method.CreateDelegate(eventHandlerType);
	}

	internal static readonly MethodInfo getMethod = typeof(EventBindingExtension).GetMethod("HandlerIntern",
		[typeof(object), typeof(object), typeof(string), typeof(string)]);

	internal static void Handler(object sender, object args) =>
		HandlerIntern(sender, args, "cmd", null);

	public static void HandlerIntern(object sender, object args, string cmdName, string? commandParameter) {
		if (sender is FrameworkElement fe) {
			ICommand? cmd = GetCommand(fe, cmdName, out ObservableObject? viewModel);
			object? commandParam = null;
			if (!string.IsNullOrWhiteSpace(commandParameter))
				commandParam = GetCommandParameter(fe, args, commandParameter!);
			if (cmd is not null && cmd.CanExecute(commandParam))
				cmd.Execute(commandParam);
		}
	}

	internal static ICommand? GetCommand(FrameworkElement target, string cmdName, out ObservableObject? viewModel) {
		ObservableObject? vm = FindViewModel(target);
		viewModel = vm;
		if (vm is null) return null;

		Type? vmType = vm.GetType();
		PropertyInfo? cmdProp = vmType.GetProperty(cmdName);
		if (cmdProp is not null)
			return cmdProp.GetValue(vm) as ICommand;
#if DEBUG
		throw new Exception($"EventBinding path error: '{cmdName}' property not found on '{vmType}' 'DelegateCommand'");
#else
		return null;
#endif
	}

	internal static object? GetCommandParameter(FrameworkElement target, object args, string commandParameter, ObservableObject? viewModel = null) {
		string[] classify = commandParameter.Split('.');
		return classify[0] switch {
			"$e" => GetProperty(args),
			"$this" => GetProperty(target),
			"$vm" => GetProperty(viewModel),
			"$view" => viewModel is IViewAccessibleViewModel vm ? GetProperty(vm.View) : null,
			_ => commandParameter,
		};

		object? GetProperty(object? target) =>
			target is null ? null :
			classify.Length > 1 ? FollowPropertyPath(target, classify.Skip(1), target.GetType()) : target;
	}

	internal static ObservableObject? FindViewModel(FrameworkElement? target) {
		if (target is null) return null;
		if (target.DataContext is ObservableObject vm) return vm;
		FrameworkElement? parent = target.GetParentObject() as FrameworkElement;
		return FindViewModel(parent);
	}

	internal static object FollowPropertyPath(object target, IEnumerable<string> path, Type? valueType = null) {
		if (target is null) throw new ArgumentNullException(nameof(target));
		if (path is null) throw new ArgumentNullException(nameof(path));
		Type currentType = valueType ?? target.GetType();

		foreach (string propertyName in path) {
			if (propertyName == "Parent" && currentType.Extends(typeof(DependencyObject))) {
				target = ((DependencyObject)target).GetParentObject()!;
				currentType = target.GetType();
				continue;
			}
			PropertyInfo property = currentType.GetProperty(propertyName);
			if (property is null)
				throw new NullReferenceException(nameof(property));

			target = property.GetValue(target);
			currentType = property.PropertyType;
		}
		return target;
	}
}
