using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Makes your Grid RowDefinitions / ColumnDefinitions easilier.
/// </summary>
/// <remarks>
/// Original name: <b><c>GridDefinitions</c></b><br/>
/// <see href="https://codemag.com/Article/1405061/XAML-Magic--Attached-Properties"/>
/// <example>
/// <para>Before:</para>
/// <code>
/// <![CDATA[
/// <Grid>
///     <Grid.RowDefinitions>
///          <RowDefinition Height="*"/>
///          <RowDefinition Height="Auto"/>
///          <RowDefinition Height="25"/>
///     </Grid.RowDefinitions>
/// <Grid>
/// ]]>
/// </code>
/// <para>After:</para>
/// <code>
/// <![CDATA[
/// <Grid m:GridDef.Row="*,Auto,25" />
/// ]]>
/// </code>
/// </example>
/// </remarks>
[AttachedDependencyProperty<string, Grid>("Row", DefaultValue = "")]
[AttachedDependencyProperty<string, Grid>("Column", DefaultValue = "")]
public static partial class GridDef {
	static partial void OnRowChanged(Grid grid, string? definitions) {
		grid.RowDefinitions.Clear();
		if (string.IsNullOrWhiteSpace(definitions)) return;
		foreach (GridLength size in ParseDefinitions(definitions!))
			grid.RowDefinitions.Add(new RowDefinition { Height = size });
	}

	static partial void OnColumnChanged(Grid grid, string? definitions) {
		grid.ColumnDefinitions.Clear();
		if (string.IsNullOrEmpty(definitions)) return;
		foreach (GridLength size in ParseDefinitions(definitions!))
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = size });
	}

	private static IEnumerable<GridLength> ParseDefinitions(string definitions) =>
		definitions.Split(',').Select(size => {
			size = size.Trim().ToLowerInvariant();
			if (size == "auto")
				return GridLength.Auto;
			else if (size.EndsWith("*")) {
				string factor = size.Replace("*", "");
				if (string.IsNullOrEmpty(factor)) factor = "1";
				int factorInt = int.Parse(factor);
				return new GridLength(factorInt, GridUnitType.Star);
			} else {
				int pixel = int.Parse(size);
				return new GridLength(pixel, GridUnitType.Pixel);
			}
		});
}
