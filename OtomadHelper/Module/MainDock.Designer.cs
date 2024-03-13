namespace OtomadHelper.Module {
	partial class MainDock {
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			this.Browser = new Microsoft.Web.WebView2.WinForms.WebView2();
			this.SplashContainer = new System.Windows.Forms.Panel();
			this.LoadingAnimationPicture = new APNGViewer.APNGBox();
			((System.ComponentModel.ISupportInitialize)(this.Browser)).BeginInit();
			this.SplashContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LoadingAnimationPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// Browser
			// 
			this.Browser.AllowExternalDrop = false;
			this.Browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Browser.CreationProperties = null;
			this.Browser.DefaultBackgroundColor = System.Drawing.Color.White;
			this.Browser.Location = new System.Drawing.Point(0, 0);
			this.Browser.Margin = new System.Windows.Forms.Padding(0);
			this.Browser.Name = "Browser";
			this.Browser.Size = new System.Drawing.Size(600, 300);
			this.Browser.TabIndex = 0;
			this.Browser.ZoomFactor = 1D;
			this.Browser.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.Browser_CoreWebView2InitializationCompleted);
			this.Browser.WebMessageReceived += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs>(this.Browser_WebMessageReceived);
			this.Browser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainDock_MouseUp);
			// 
			// SplashContainer
			// 
			this.SplashContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SplashContainer.Controls.Add(this.LoadingAnimationPicture);
			this.SplashContainer.Location = new System.Drawing.Point(0, 0);
			this.SplashContainer.Margin = new System.Windows.Forms.Padding(0);
			this.SplashContainer.Name = "SplashContainer";
			this.SplashContainer.Size = new System.Drawing.Size(600, 300);
			this.SplashContainer.TabIndex = 1;
			// 
			// LoadingAnimationPicture
			// 
			this.LoadingAnimationPicture.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LoadingAnimationPicture.APNGFile = null;
			this.LoadingAnimationPicture.Image = null;
			this.LoadingAnimationPicture.Location = new System.Drawing.Point(0, 0);
			this.LoadingAnimationPicture.Name = "LoadingAnimationPicture";
			this.LoadingAnimationPicture.Size = new System.Drawing.Size(600, 300);
			this.LoadingAnimationPicture.TabIndex = 0;
			this.LoadingAnimationPicture.TabStop = false;
			// 
			// MainDock
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.SplashContainer);
			this.Controls.Add(this.Browser);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Name = "MainDock";
			this.Size = new System.Drawing.Size(600, 300);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainDock_DragEnter);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.MainDock_DragOver);
			((System.ComponentModel.ISupportInitialize)(this.Browser)).EndInit();
			this.SplashContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.LoadingAnimationPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public Microsoft.Web.WebView2.WinForms.WebView2 Browser;
		private System.Windows.Forms.Panel SplashContainer;
		private APNGViewer.APNGBox LoadingAnimationPicture;
	}
}
