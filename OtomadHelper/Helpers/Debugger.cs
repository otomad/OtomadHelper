using System.Windows;

using OtomadHelper.Models;

namespace OtomadHelper.Helpers;

[SuppressMessage("Style", "IDE1006")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Debugger {
	/// <summary>
	/// Show me the fucking content!
	/// </summary>
	/// <remarks>
	/// 给老娘展示内容！
	/// </remarks>
	[Obsolete("This method can only be used for debugging and should not be used in release!")]
	public static object? s {
		set {
#if !VEGAS_ENV
			Debug.WriteLine(value ?? "null");
#else
			string text = ToString(value);
			//alert = text;
			PostWebMessage(new ConsoleLog(text));
#endif
		}
	}

	/// <summary>
	/// Show me the fucking content!
	/// </summary>
	/// <remarks>
	/// 给老娘展示内容！
	/// </remarks>
	[Obsolete("This method can only be used for debugging and should not be used in release!")]
	public static object? alert {
		set {
			string text = ToString(value);
			Task.Run(() => MessageBox.Show(text));
		}
	}

	[Obsolete("This method can only be used for debugging and should not be used in release!")]
	public static void DebugPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		s = e.NewValue;
	}

	/// <summary>
	/// Special: Some test environments may not be able to use any debugging, so performed by writing text to the a txt file on the desktop instead.
	/// </summary>
	[Obsolete("This method can only be used for debugging and should not be used in release!")]
	public static object? writeTxt {
		set {
			const string fileName = ".test.txt";
			string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName;
			string text = ToString(value);
			try {
				using StreamWriter writer = File.AppendText(filePath);
				writer.WriteLine(text);
			} catch (Exception e) {
				Debug.WriteLine(e);
			}
		}
	}

	private static string ToString(object? nullableValue) => (nullableValue ?? "null").ToString();

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
