
namespace VegasScript {
	partial class IntegerTrackWithBox {
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
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.Track = new System.Windows.Forms.TrackBar();
			this.Numeric = new System.Windows.Forms.NumericUpDown();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Track)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Numeric)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.Track, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.Numeric, 1, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 1;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(377, 39);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// Track
			// 
			this.Track.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Track.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Track.Location = new System.Drawing.Point(0, 0);
			this.Track.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
			this.Track.Maximum = 100;
			this.Track.Name = "Track";
			this.Track.Size = new System.Drawing.Size(294, 39);
			this.Track.TabIndex = 0;
			this.Track.TickFrequency = 10;
			this.Track.Scroll += new System.EventHandler(this.Track_Scroll);
			// 
			// Numeric
			// 
			this.Numeric.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Numeric.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.Numeric.Location = new System.Drawing.Point(302, 0);
			this.Numeric.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this.Numeric.Name = "Numeric";
			this.Numeric.Size = new System.Drawing.Size(75, 23);
			this.Numeric.TabIndex = 1;
			this.Numeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
			// 
			// IntegerTrackWithBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "IntegerTrackWithBox";
			this.Size = new System.Drawing.Size(377, 39);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Track)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Numeric)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.TrackBar Track;
		private System.Windows.Forms.NumericUpDown Numeric;
	}
}
