using ScriptPortal.MediaSoftware.Skins;
using ScriptPortal.Vegas;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtomadHelper.Core {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : UserControl {
		private readonly Vegas vegas;

		public MainWindow(Vegas myVegas) {
			vegas = myVegas;
			InitializeComponent();
			Background = new SolidColorBrush(ToMediaColor(Skins.Colors.ButtonFace));
			Foreground = new SolidColorBrush(ToMediaColor(Skins.Colors.ButtonText));
			SelectLbl.Foreground = new SolidColorBrush(ToMediaColor(Skins.Colors.ButtonText));
		}

		public void SelectBtn_Click(object sender, RoutedEventArgs e) {
			int count = GetSelectedEvents().Length;
			SelectLbl.Content = count;
		}

		public TrackEvent[] GetSelectedEvents() {
			List<TrackEvent> selectedList = new List<TrackEvent>();
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent.Selected)
						selectedList.Add(trackEvent);
			return selectedList.ToArray();
		}

		public static Color ToMediaColor(System.Drawing.Color color) {
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}
	}
}
