
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.trackBar1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.numericUpDown1, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(407, 189);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// trackBar1
			// 
			this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trackBar1.Location = new System.Drawing.Point(3, 3);
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(275, 183);
			this.trackBar1.TabIndex = 0;
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.numericUpDown1.Location = new System.Drawing.Point(284, 3);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(120, 23);
			this.numericUpDown1.TabIndex = 1;
			// 
			// IntegerTrackWithBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "IntegerTrackWithBox";
			this.Size = new System.Drawing.Size(407, 189);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
	}
}
