using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace OtomadHelper.WPF.Common;

public enum FocusMoveDirection {
	Previous = -1,
	Next = 1,
	PageBackward = -10,
	PageForward = 10,
	First = -100,
	Last = 100,
}

public static class FocusMoveDirectionExtension {
	extension(FocusMoveDirection direction) {
		public int Delta => Math.Sign((int)direction);

		public static FocusMoveDirection? FromKey(Key key) => key switch {
			Key.Up or Key.Left => FocusMoveDirection.Previous,
			Key.Down or Key.Right => FocusMoveDirection.Next,
			Key.PageUp => FocusMoveDirection.PageBackward,
			Key.PageDown => FocusMoveDirection.PageForward,
			Key.Home => FocusMoveDirection.First,
			Key.End => FocusMoveDirection.Last,
			_ => null,
		};
	}

}

[ValueConversion(typeof(MouseWheelEventArgs), typeof(FocusMoveDirection))]
public class MouseWheelEventArgsToFocusMoveDirectionConverter : ValueConverter<MouseWheelEventArgs, FocusMoveDirection> {
	public override FocusMoveDirection Convert(MouseWheelEventArgs e, Type targetType, object parameter, CultureInfo culture) =>
		(FocusMoveDirection)Math.Sign(-e.Delta);
}
