using System.Windows;

namespace OtomadHelper.WPF.Common;

public interface IViewAccessibleViewModel { }

internal static class ExtendViewAccessibleViewModel {
	public static void SetView(this IViewAccessibleViewModel? viewModel, FrameworkElement view) {
		if (viewModel is null) return;
		PropertyInfo? viewProperty = viewModel.GetType().GetProperty("View");
		if (viewProperty is not null &&
			typeof(FrameworkElement).IsAssignableFrom(viewProperty.PropertyType) &&
			viewProperty.SetMethod is not null)
			viewProperty.SetMethod.Invoke(viewModel, new object[] { view });
	}
}

public interface IViewAccessibleViewModel<TView> : IViewAccessibleViewModel
	where TView : FrameworkElement {
	/// <summary>
	/// Access View from ViewModel.
	/// </summary>
	public TView? View { get; set; }
}
