namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class PvRhythmVisualEffectAdvancedForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.EffectsPanel = new System.Windows.Forms.Panel();
			this.EffectsTable = new System.Windows.Forms.TableLayoutPanel();
			this.dock.SuspendLayout();
			this.EffectsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// dock
			// 
			this.dock.AutoSize = true;
			this.dock.BackColor = System.Drawing.SystemColors.Control;
			this.dock.ColumnCount = 1;
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.dock.Controls.Add(this.OkBtn, 0, 0);
			this.dock.Controls.Add(this.CancelBtn, 0, 1);
			this.dock.Dock = System.Windows.Forms.DockStyle.Right;
			this.dock.Location = new System.Drawing.Point(594, 0);
			this.dock.Margin = new System.Windows.Forms.Padding(5);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 3;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(118, 503);
			this.dock.TabIndex = 9;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(12, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "确定(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(12, 50);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			// 
			// EffectsPanel
			// 
			this.EffectsPanel.AutoScroll = true;
			this.EffectsPanel.Controls.Add(this.EffectsTable);
			this.EffectsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EffectsPanel.Location = new System.Drawing.Point(0, 0);
			this.EffectsPanel.Name = "EffectsPanel";
			this.EffectsPanel.Size = new System.Drawing.Size(594, 503);
			this.EffectsPanel.TabIndex = 10;
			// 
			// EffectsTable
			// 
			this.EffectsTable.AutoSize = true;
			this.EffectsTable.ColumnCount = 1;
			this.EffectsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.EffectsTable.Dock = System.Windows.Forms.DockStyle.Top;
			this.EffectsTable.Location = new System.Drawing.Point(0, 0);
			this.EffectsTable.Name = "EffectsTable";
			this.EffectsTable.Padding = new System.Windows.Forms.Padding(5);
			this.EffectsTable.RowCount = 1;
			this.EffectsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.EffectsTable.Size = new System.Drawing.Size(594, 10);
			this.EffectsTable.TabIndex = 0;
			// 
			// PvRhythmVisualEffectAdvancedForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(712, 503);
			this.Controls.Add(this.EffectsPanel);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(460, 460);
			this.Name = "PvRhythmVisualEffectAdvancedForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "映画节奏视觉效果";
			this.ResizeEnd += new System.EventHandler(this.PvRhythmVisualEffectAdvancedForm_Resize);
			this.dock.ResumeLayout(false);
			this.EffectsPanel.ResumeLayout(false);
			this.EffectsPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Panel EffectsPanel;
		private System.Windows.Forms.TableLayoutPanel EffectsTable;
	}
}