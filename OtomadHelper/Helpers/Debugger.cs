using System.Windows;

using OtomadHelper.Module;

namespace OtomadHelper.Helpers;

internal static class Debugger {
	/// <summary>
	/// Show me the fucking content!
	/// </summary>
	/// <remarks>
	/// 给老娘展示内容！
	/// </remarks>
	[Obsolete("This method can only be used for debugging and should not be used in release!")]
#pragma warning disable IDE1006 // 命名样式
	public static object? s {
#pragma warning restore IDE1006 // 命名样式
		set {
			object nullableValue = value is null ? "null" : value;
#if !VEGAS_ENV
			Debug.WriteLine(nullableValue);
#else
			MessageBox.Show(nullableValue.ToString());
#endif
		}
	}

	[Obsolete("This method can only be used for debugging and should not be used in release!")]
	public static void DebugPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		s = e.NewValue;
	}

	/// <summary>
	/// <para>Suppress unused variables or parameters warning.</para>
	/// <para>Hack goto label at the end of the block.</para>
	/// </summary>
	/// <remarks>
	/// <example>
	/// <code>
	/// public void UnusedParams(string foo, int bar) {
	///     bool baz = true;
	///     Unused(foo, bar, baz);
	/// }
	///
	/// public void HackLabel() {
	///     if (...) {
	///         while (...) {
	///             if (...) {
	///                 DoSomething();
	///             } else {
	///                 goto EndOfIf;
	///             }
	///         }
	///         DoSomething();
	///     EndOfIf:
	///         Unused();
	///     }
	///     DoSomething();
	/// }
	/// </code>
	/// </example>
	/// </remarks>
	public static void Unused(params object[] unused) => _ = unused;
}
