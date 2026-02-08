using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace OtomadHelper.WPF.Common;

public class ScrollViewerScrollBarVisibilityToScrollBarThicknessMarginConverter : MultiValueConverter<Tuple<ScrollViewer, Visibility>, Thickness, bool> {
	public override Thickness Convert(Tuple<ScrollViewer, Visibility> values, Type targetType, bool isHorizontalScrollBar = false, CultureInfo? culture = null) {
		(ScrollViewer scrollViewer, Visibility visibility) = values;

		double GetThickness() {
			if (visibility == Visibility.Collapsed) return 0;
			scrollViewer.ApplyTemplate();
			ScrollBar? verticalScrollBar = scrollViewer.Template.FindName("PART_VerticalScrollBar", scrollViewer) as ScrollBar,
				horizontalScrollBar = scrollViewer.Template.FindName("PART_HorizontalScrollBar", scrollViewer) as ScrollBar;
			return isHorizontalScrollBar ? horizontalScrollBar is not null ? horizontalScrollBar.ActualHeight : SystemParameters.HorizontalScrollBarHeight :
				verticalScrollBar is not null ? verticalScrollBar.ActualWidth : SystemParameters.VerticalScrollBarWidth;
		}

		double thickness = GetThickness();
		return isHorizontalScrollBar ? new(0, 0, 0, thickness) : new(0, 0, thickness, 0);
	}
}

