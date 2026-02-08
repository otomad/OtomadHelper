using System.Windows.Controls;


namespace OtomadHelper.WPF.Common;

[AttachedDependencyProperty<ScrollViewer, ScrollViewer>("Horizontal")]
[AttachedDependencyProperty<ScrollViewer, ScrollViewer>("Vertical")]
public static partial class ScrollViewerSyncScroll {
	static partial void OnHorizontalChanged(ScrollViewer me, ScrollViewer? oldAnother, ScrollViewer? newAnother) {
		if (oldAnother is not null) {
			SetHorizontal(oldAnother, null);
			oldAnother.ScrollChanged -= ScrollViewer_HorizontalScrollChanged;
		}
		if (newAnother is not null) {
			SetHorizontal(newAnother, me);
			newAnother.ScrollChanged += ScrollViewer_HorizontalScrollChanged;
		}
	}

	private static void ScrollViewer_HorizontalScrollChanged(object sender, ScrollChangedEventArgs e) {
		ScrollViewer? me = sender as ScrollViewer, another = me is null ? null : GetHorizontal(me);
		if (me is not null && another is not null)
			another.ScrollToHorizontalOffset(e.HorizontalOffset);
	}

	static partial void OnVerticalChanged(ScrollViewer me, ScrollViewer? oldAnother, ScrollViewer? newAnother) {
		if (oldAnother is not null) {
			SetVertical(oldAnother, null);
			oldAnother.ScrollChanged -= ScrollViewer_VerticalScrollChanged;
		}
		if (newAnother is not null) {
			SetVertical(newAnother, me);
			newAnother.ScrollChanged += ScrollViewer_VerticalScrollChanged;
		}
	}

	private static void ScrollViewer_VerticalScrollChanged(object sender, ScrollChangedEventArgs e) {
		ScrollViewer? me = sender as ScrollViewer, another = me is null ? null : GetVertical(me);
		if (me is not null && another is not null)
			another.ScrollToVerticalOffset(e.VerticalOffset);
	}
}
