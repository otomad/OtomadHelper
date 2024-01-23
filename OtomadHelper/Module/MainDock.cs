﻿using Microsoft.Web.WebView2.Core;
using OtomadHelper.Helpers;
using OtomadHelper.Test;
using APNGLib;
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
using ScriptPortal.MediaSoftware.Skins;
using System.IO.Packaging;

namespace OtomadHelper.Module {
	public partial class MainDock : UserControl {
		public MainDock() {
			InitializeComponent();
			Dock = DockStyle.Fill;
			//MainWindow window = new();
			//window.Show();

#if VEGAS_ENV
			BackColor = Skins.Colors.ButtonFace;
			ForeColor = Skins.Colors.ButtonText;
#endif
			InitLoadingAnimation();
			Browser.EnsureCoreWebView2Async();
		}

		private void Browser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
			ManagedStream.Handler(Browser);
			Browser.Source = new Uri("http://app/index.html"); // "http://www.sunchateau.com/free/ua.htm"
		}

		private void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e) {
			LoadingAnimationPicture.Visible = false;
			LoadingAnimationPicture.Stop();
			Browser.Visible = true;
		}

		private APNGBox LoadingAnimationPicture = null!;
		private void InitLoadingAnimation() {
			try {
				APNG apng = new();
				Stream fileStream = ResourceHelper.GetEmbeddedResource("Assets.LoadingAnimation.apng");
				apng.Load(fileStream);
				LoadingAnimationPicture = new APNGBox(apng) {
					Location = new Point((Width - (int)apng.Width) / 2, (Height - (int)apng.Height) / 2),
					Anchor = AnchorStyles.None,
				};
				Controls.Add(LoadingAnimationPicture);
				LoadingAnimationPicture.Start();
			} catch (Exception) { }
		}
	}
}