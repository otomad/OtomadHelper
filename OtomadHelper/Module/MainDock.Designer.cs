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
			this.components = new System.ComponentModel.Container();
			this.Browser = new Microsoft.Web.WebView2.WinForms.WebView2();
			this.LoadingAnimationPicture = new System.Windows.Forms.PictureBox();
			this.LoadingAnimationTimer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.Browser)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LoadingAnimationPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// Browser
			// 
			this.Browser.AllowExternalDrop = true;
			this.Browser.CreationProperties = null;
			this.Browser.DefaultBackgroundColor = System.Drawing.Color.White;
			this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Browser.Location = new System.Drawing.Point(0, 0);
			this.Browser.Name = "Browser";
			this.Browser.Size = new System.Drawing.Size(600, 300);
			this.Browser.TabIndex = 0;
			this.Browser.Visible = false;
			this.Browser.ZoomFactor = 1D;
			this.Browser.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.Browser_CoreWebView2InitializationCompleted);
			this.Browser.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.Browser_NavigationCompleted);
			// 
			// LoadingAnimationPicture
			// 
			this.LoadingAnimationPicture.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LoadingAnimationPicture.BackColor = System.Drawing.Color.Transparent;
			this.LoadingAnimationPicture.Image = global::OtomadHelper.Properties.Resources.otomad_helper_loading_000;
			this.LoadingAnimationPicture.Location = new System.Drawing.Point(172, 22);
			this.LoadingAnimationPicture.Name = "LoadingAnimationPicture";
			this.LoadingAnimationPicture.Size = new System.Drawing.Size(256, 256);
			this.LoadingAnimationPicture.TabIndex = 1;
			this.LoadingAnimationPicture.TabStop = false;
			// 
			// LoadingAnimationTimer
			// 
			this.LoadingAnimationTimer.Tick += new System.EventHandler(this.LoadingAnimationTimer_Tick);
			// 
			// MainDock
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.LoadingAnimationPicture);
			this.Controls.Add(this.Browser);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Name = "MainDock";
			this.Size = new System.Drawing.Size(600, 300);
			((System.ComponentModel.ISupportInitialize)(this.Browser)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LoadingAnimationPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Web.WebView2.WinForms.WebView2 Browser;
		private System.Windows.Forms.PictureBox LoadingAnimationPicture;
		private System.Windows.Forms.Timer LoadingAnimationTimer;
	}
}
