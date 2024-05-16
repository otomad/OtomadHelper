using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// PitchPicker.xaml 的交互逻辑
/// </summary>
public partial class PitchPickerFlyout : BaseFlyout {
	public PitchPickerFlyout() {
		InitializeComponent();

		BindingOperations.SetBinding(this, NoteNameProperty, new Binding("NoteName"));
		BindingOperations.SetBinding(this, OctaveProperty, new Binding("Octave"));
	}

	public new PitchPickerViewModel DataContext => (PitchPickerViewModel)base.DataContext;

	public static PitchPickerFlyout Initial(Rect targetRect) {
		PitchPickerFlyout picker = new();
		//picker.Width = targetRect.Width + picker.ItemPadding * 2;
		picker.Loaded += (sender, e) => picker.Center(targetRect, SetWidthType.Width);
		return picker;
	}

	private double ItemHeight => (double)Resources["ItemHeight"];
	private double ItemPadding => ((Thickness)Resources["ItemPadding"]).Left;
	private const int DisplayItemCount = 7;
	private static int ReservedForCenteringItemCount => DisplayItemCount / 2;
	public Rect SelectionMaskRect => new(0, ReservedForCenteringItemCount / (double)DisplayItemCount, 1, 1 / (double)DisplayItemCount);

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		Height = ItemHeight * DisplayItemCount + ItemPadding * 2;
		//this.GetChildrenOfType<ScrollViewer>().ForEach(scrollViewer =>
		//	scrollViewer.PreviewMouseWheel += (sender, e) => e.Handled = true);
	}

	public static readonly DependencyProperty NoteNameProperty = DependencyProperty.Register("NoteName", typeof(string), typeof(PitchPickerFlyout), new(null, NoteNamePropertyChanged));
	public static readonly DependencyProperty OctaveProperty = DependencyProperty.Register("Octave", typeof(int?), typeof(PitchPickerFlyout), new(null, OctavePropertyChanged));

	public static readonly DependencyProperty DisplayNoteNamesProperty = DependencyProperty.Register(nameof(DisplayNoteNames), typeof(string[]), typeof(PitchPickerFlyout), new(null));
	private string[] DisplayNoteNames { get => (string[])GetValue(DisplayNoteNamesProperty); set => SetValue(DisplayNoteNamesProperty, value); }

	public static void NoteNamePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		if (sender is not PitchPickerFlyout picker) return;
		List<string> noteNames = PitchPickerViewModel.NoteNames.ToList();
		int noteNamesCount = noteNames.Count;
		string noteName = (string)e.NewValue; string? prevNoteName = (string?)e.OldValue;
		int noteNameIndex = noteNames.IndexOf(noteName), prevNoteNameIndex = noteNames.IndexOf(prevNoteName!);
		bool hasPrevNoteName = prevNoteNameIndex != -1;
		if (!hasPrevNoteName) prevNoteNameIndex = noteNameIndex;
		if (prevNoteNameIndex < noteNameIndex &&
			Math.Abs(prevNoteNameIndex + noteNamesCount - noteNameIndex) < Math.Abs(prevNoteNameIndex - noteNameIndex))
			prevNoteNameIndex += noteNamesCount;
		else if (prevNoteNameIndex > noteNameIndex &&
			Math.Abs(prevNoteNameIndex - noteNamesCount - noteNameIndex) < Math.Abs(prevNoteNameIndex - noteNameIndex))
			prevNoteNameIndex -= noteNamesCount;
		int minIndex = Math.Min(noteNameIndex, prevNoteNameIndex) - ReservedForCenteringItemCount,
			maxIndex = Math.Max(noteNameIndex, prevNoteNameIndex) + ReservedForCenteringItemCount;
		string[] displayNoteNames = new string[maxIndex - minIndex + 1];
		for (int i = minIndex, j = 0; i <= maxIndex; i++, j++)
			displayNoteNames[j] = noteNames[MathEx.PNMod(i, noteNamesCount)];
		picker.DisplayNoteNames = displayNoteNames;
		noteNameIndex -= minIndex; prevNoteNameIndex -= minIndex;
		double toTop = picker.GetItemTopFromIndex(noteNameIndex);
		double? fromTop = !hasPrevNoteName ? null : picker.GetItemTopFromIndex(prevNoteNameIndex);
		picker.SetListViewTopAnimatedly(ColumnType.NoteName, toTop, fromTop);
	}

	public static void OctavePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		if (sender is not PitchPickerFlyout picker) return;
		List<int> octaves = PitchPickerViewModel.Octaves.ToList();
		int octave = (int)e.NewValue; int? prevOctave = (int?)e.OldValue;
		int octaveIndex = octaves.IndexOf(octave), prevOctaveIndex = octaves.IndexOf(prevOctave);
		if (octaveIndex == -1) return;
		double toTop = picker.GetItemTopFromIndex(octaveIndex);
		double? fromTop = prevOctaveIndex == -1 ? null : picker.GetItemTopFromIndex(prevOctaveIndex);
		picker.SetListViewTopAnimatedly(ColumnType.Octave, toTop, fromTop);
	}

	private double GetItemTopFromIndex(int index, bool center = true) =>
		ItemHeight * (-index + (center ? ReservedForCenteringItemCount : 0));

	private void SetListViewTopAnimatedly(ColumnType column, double toTop, double? fromTop = null) {
		PitchPickerFlyoutListView listView = column == ColumnType.NoteName ? NoteNameListView : OctaveListView;
		fromTop ??= Canvas.GetTop(listView);
		if (fromTop is null or double.NaN) {
			Canvas.SetTop(listView, toTop);
			return;
		}

		DoubleAnimation animation = new() {
			From = fromTop,
			To = toTop,
			Duration = new(TimeSpan.FromMilliseconds(250)),
			FillBehavior = FillBehavior.HoldEnd,
			EasingFunction = (IEasingFunction)Resources["EaseOutExpo"],
		};
		Storyboard.SetTargetProperty(animation, new("(Canvas.Top)"));
		Storyboard storyboard = new();
		storyboard.Children.Add(animation);
		storyboard.Begin(listView);
	}

	internal enum ColumnType {
		NoteName,
		Octave,
	}

	private ColumnType? GetFocusedColumn() =>
		NoteNameWrapper.IsKeyboardFocused ? ColumnType.NoteName :
		OctaveWrapper.IsKeyboardFocused ? ColumnType.Octave :
		null;

	private ColumnType? GetActiveColumn() =>
		GetFocusedColumn() ?? (
			NoteNameListView.IsMouseOver ? ColumnType.NoteName :
			OctaveListView.IsMouseOver ? ColumnType.Octave :
			null
		);

	private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
		if (e.Key is Key.Up or Key.Down) {
			ColumnType? column = GetActiveColumn();
			int delta = e.Key == Key.Up ? 120 : -120;
			if (column == ColumnType.NoteName) DataContext.NoteNameSpinCommand.Execute(delta);
			else if (column == ColumnType.Octave) DataContext.OctaveSpinCommand.Execute(delta);
			e.Handled = true;
		} else if (e.Key is Key.Left or Key.Right && GetFocusedColumn() is null) {
			Keyboard.Focus(e.Key == Key.Left ? NoteNameWrapper : OctaveWrapper);
			e.Handled = true;
		}
	}

	private void ListViewWrapper_PreviewMouseDown(object sender, MouseButtonEventArgs e) =>
		Keyboard.ClearFocus();
}
