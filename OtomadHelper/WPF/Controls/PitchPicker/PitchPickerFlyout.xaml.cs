using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// PitchPicker.xaml 的交互逻辑
/// </summary>
[DependencyProperty<string>("NoteName")]
[DependencyProperty<int?>("Octave")]
[DependencyProperty<string[]>("DisplayNoteNames")]
public partial class PitchPickerFlyout : BaseFlyout {
	public PitchPickerFlyout() {
		InitializeComponent();

		BindingOperations.SetBinding(this, NoteNameProperty, new Binding("NoteName"));
		BindingOperations.SetBinding(this, OctaveProperty, new Binding("Octave"));
	}

	public new PitchPickerViewModel DataContext => (PitchPickerViewModel)base.DataContext;

	public static PitchPickerFlyout Initial(Rect targetRect, string pitch, out Task<string> dialogResult) {
		PitchPickerFlyout picker = new();
		picker.SetPitchInitially(pitch);
		//picker.Width = targetRect.Width + picker.ItemPadding * 2;
		picker.Loaded += (sender, e) => picker.Center(targetRect, SetWidthType.Width);
		dialogResult = picker.GetDialogResultTask(() => picker.DataContext.Pitch);
		return picker;
	}

	private double ItemHeight => (double)Resources["ItemHeight"];
	private double ItemPadding => ((Thickness)Resources["ItemPadding"]).Left;
	private const int DisplayItemCount = 7;
	private static int ReservedForCenteringItemCount => DisplayItemCount / 2;
	private double ExpectedHeight => ItemHeight * DisplayItemCount + ItemPadding * 2;
	public Rect SelectionMaskRect => new(0, (ReservedForCenteringItemCount * ItemHeight + ItemPadding) / ExpectedHeight, 1, ItemHeight / ExpectedHeight); // Relative
	public Rect HoverCutEdgeRect => new(0, ItemHeight + ItemPadding, Width, ItemHeight * (DisplayItemCount - 2)); // Absolute

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		Height = ExpectedHeight;
	}

	partial void OnNoteNameChanged(string? prevNoteName, string? noteName) {
		List<string> noteNames = [.. PitchPickerViewModel.NoteNames];
		int noteNamesCount = noteNames.Count;
		int noteNameIndex = noteNames.IndexOf(noteName!), prevNoteNameIndex = noteNames.IndexOf(prevNoteName!);
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
		DisplayNoteNames = displayNoteNames;
		noteNameIndex -= minIndex; prevNoteNameIndex -= minIndex;
		double toTop = GetItemTopFromIndex(noteNameIndex);
		double? fromTop = !hasPrevNoteName ? null : GetItemTopFromIndex(prevNoteNameIndex);
		SetListViewTopAnimatedly(ColumnType.NoteName, toTop, fromTop);
	}

	partial void OnOctaveChanged(int? prevOctave, int? octave) {
		List<int> octaves = [.. PitchPickerViewModel.Octaves];
		int octaveIndex = octaves.IndexOf(octave), prevOctaveIndex = octaves.IndexOf(prevOctave);
		if (octaveIndex == -1) return;
		double toTop = GetItemTopFromIndex(octaveIndex);
		double? fromTop = prevOctaveIndex == -1 ? null : GetItemTopFromIndex(prevOctaveIndex);
		SetListViewTopAnimatedly(ColumnType.Octave, toTop, fromTop);
	}

	private double GetItemTopFromIndex(int index, bool center = true) =>
		ItemHeight * (-index + (center ? ReservedForCenteringItemCount : 0));

	protected bool disableSetListViewTopAnimatedly = false;

	public void SetPitchInitially(string pitch) { // Remove unnecessary animations when initializing.
		disableSetListViewTopAnimatedly = true;
		DataContext.Pitch = pitch;
		disableSetListViewTopAnimatedly = false;
	}

	private void SetListViewTopAnimatedly(ColumnType column, double toTop, double? fromTop = null) {
		PitchPickerFlyoutListView listView = column == ColumnType.NoteName ? NoteNameListView : OctaveListView;
		fromTop ??= Canvas.GetTop(listView);
		if (fromTop is null or double.NaN || fromTop == toTop || disableSetListViewTopAnimatedly) {
			Canvas.SetTop(listView, toTop);
			return;
		}

		DoubleAnimation animation = new() {
			From = fromTop,
			To = toTop,
			Duration = (Duration)Resources["BaseAnimationDuration"],
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
			NoteNameWrapper.IsMouseOver ? ColumnType.NoteName :
			OctaveWrapper.IsMouseOver ? ColumnType.Octave :
			null
		);

	private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
		if (e.Key is Key.Up or Key.Down) {
			ColumnType? column = GetActiveColumn();
			int delta = (int)(e.Key == Key.Up ? Resources["SpinUpDelta"] : Resources["SpinDownDelta"]);
			if (column == ColumnType.NoteName) DataContext.NoteNameSpinCommand.Execute(delta);
			else if (column == ColumnType.Octave) DataContext.OctaveSpinCommand.Execute(delta);
			e.Handled = true;
		} else if (e.Key is Key.Left or Key.Right && GetFocusedColumn() is null) {
			Keyboard.Focus(e.Key == Key.Left ? NoteNameWrapper : OctaveWrapper);
			e.Handled = true;
		}
	}

	private void ListViewWrapper_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
		// Click to reset the focus ring.
		if (e.Source is IInputElement element)
			Keyboard.Focus(element);
	}
}
