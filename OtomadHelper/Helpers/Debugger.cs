using System.Windows;

namespace OtomadHelper.Helpers;

internal static class Debugger {
#pragma warning disable IDE1006 // 命名样式
	public static object s {
#pragma warning restore IDE1006 // 命名样式
		set => Debug.WriteLine(value is null ? "null" : value);
	}

	public static void DebugPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		s = e.NewValue;
	}
}
