using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(bool), typeof(string))]
public class ExpandButtonIsCheckedToIsExpandedTextConverter : ValueConverter<bool, string> {
	public override string Convert(bool isExpanded, Type targetType, object parameter, CultureInfo culture) =>
		isExpanded ? t.ContentDialog.Expander.CollapseDetails : t.ContentDialog.Expander.ExpandDetails;
}
