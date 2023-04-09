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
	public partial class MainDock : UserControl {
		private Vegas vegas { get { return dock.vegas; } }
		internal static MainDock Instance { get; private set; }
		private readonly OtomadHelperDock dock;

		public MainDock() {
			InitializeComponent();
			Instance = this;
		}

		internal string Received {
			get { return ReceivedLbl.Text; }
			set { ReceivedLbl.Text = value; }
		}

		public MainDock(OtomadHelperDock dock) : this() {
			this.dock = dock;
			Background = new SolidColorBrush(ToMediaColor(Skins.Colors.ButtonFace));
			Foreground = new SolidColorBrush(ToMediaColor(Skins.Colors.ButtonText));
			ReceivedLbl.Foreground = new SolidColorBrush(ToMediaColor(Skins.Colors.ButtonText));
		}

		public void SendBtn_Click(object sender, RoutedEventArgs e) {
			//(App.Current.MainWindow as TestWindow).Send(SendTxt.Text);
			dock.windowHelper.SendMessage(SendTxt.Text);
		}

		/*public TrackEvent[] GetSelectedEvents() {
			List<TrackEvent> selectedList = new List<TrackEvent>();
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent.Selected)
						selectedList.Add(trackEvent);
			return selectedList.ToArray();
		}*/

		public static Color ToMediaColor(System.Drawing.Color color) {
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		private void OpenClientBtn_Click(object sender, RoutedEventArgs e) {
			//(App.Current.MainWindow as TestWindow).windowHelper.OpenClient();
			dock.windowHelper.OpenClient();
		}
	}
}
