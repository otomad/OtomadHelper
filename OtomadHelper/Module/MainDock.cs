using Microsoft.Web.WebView2.Core;
using OtomadHelper.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtomadHelper.Module {
	public partial class MainDock : UserControl {
		public MainDock() {
			InitializeComponent();
			Dock = DockStyle.Fill;
			InitLoadingAnimation();
			Browser.EnsureCoreWebView2Async();
			LoadingAnimationTimer.Interval = 1000 / 60;
			LoadingAnimationTimer.Start();
		}

		private void Browser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
			ManagedStream.Handler(Browser);
			Browser.Source = new Uri("http://app/index.html"); // "http://www.sunchateau.com/free/ua.htm"
		}

		private void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e) {
			LoadingAnimationPicture.Visible = false;
			LoadingAnimationTimer.Stop();
			Browser.Visible = true;
		}

		private Image[] LoadingAnimationImages;
		private const int ANIMATION_LENGTH = 120;
		private int currentLoadingAnimationIndex = 0;
		private void InitLoadingAnimation() {
			List<Image> imageList = new List<Image>();
			for (int i = 0; i < ANIMATION_LENGTH; i++) {
				string fileName = "otomad helper loading_" + i.ToString().PadLeft(3, '0');
				Stream fileStream = ResourceHelper.GetEmbeddedResource("Assets.LoadingAnimation." + fileName + ".png");
				imageList.Add(Image.FromStream(fileStream));
			}
			LoadingAnimationImages = imageList.ToArray();
		}

		private void LoadingAnimationTimer_Tick(object sender, EventArgs e) {
			LoadingAnimationPicture.Image = LoadingAnimationImages[currentLoadingAnimationIndex];
			currentLoadingAnimationIndex++;
			currentLoadingAnimationIndex %= ANIMATION_LENGTH;
		}
	}
}
