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
			((System.ComponentModel.ISupportInitialize)(this.Browser)).BeginInit();
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
			this.Browser.Size = new System.Drawing.Size(564, 314);
			this.Browser.TabIndex = 0;
			this.Browser.ZoomFactor = 1D;
			this.Browser.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.Browser_CoreWebView2InitializationCompleted);
			// 
			// MainDock
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.Browser);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Name = "MainDock";
			this.Size = new System.Drawing.Size(564, 314);
			((System.ComponentModel.ISupportInitialize)(this.Browser)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Web.WebView2.WinForms.WebView2 Browser;
	}
}
