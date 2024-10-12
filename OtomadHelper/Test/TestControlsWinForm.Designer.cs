namespace OtomadHelper.Test;

partial class TestControlsWinForm {
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
			this.ComboBoxBtn = new System.Windows.Forms.Button();
			this.PitchPickerBtn = new System.Windows.Forms.Button();
			this.ConfirmDeleteFlyoutBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ComboBoxBtn
			// 
			this.ComboBoxBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F);
			this.ComboBoxBtn.Location = new System.Drawing.Point(271, 182);
			this.ComboBoxBtn.Margin = new System.Windows.Forms.Padding(0);
			this.ComboBoxBtn.Name = "ComboBoxBtn";
			this.ComboBoxBtn.Size = new System.Drawing.Size(320, 69);
			this.ComboBoxBtn.TabIndex = 0;
			this.ComboBoxBtn.Text = "我是组合框";
			this.ComboBoxBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ComboBoxBtn.UseVisualStyleBackColor = true;
			this.ComboBoxBtn.Click += new System.EventHandler(this.Button_Click);
			// 
			// PitchPickerBtn
			// 
			this.PitchPickerBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F);
			this.PitchPickerBtn.Location = new System.Drawing.Point(271, 315);
			this.PitchPickerBtn.Margin = new System.Windows.Forms.Padding(0);
			this.PitchPickerBtn.Name = "PitchPickerBtn";
			this.PitchPickerBtn.Size = new System.Drawing.Size(320, 69);
			this.PitchPickerBtn.TabIndex = 1;
			this.PitchPickerBtn.Text = "我是音高选择器";
			this.PitchPickerBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.PitchPickerBtn.UseVisualStyleBackColor = true;
			this.PitchPickerBtn.Click += new System.EventHandler(this.Button_Click);
			// 
			// ConfirmDeleteFlyoutBtn
			// 
			this.ConfirmDeleteFlyoutBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F);
			this.ConfirmDeleteFlyoutBtn.Location = new System.Drawing.Point(271, 448);
			this.ConfirmDeleteFlyoutBtn.Margin = new System.Windows.Forms.Padding(0);
			this.ConfirmDeleteFlyoutBtn.Name = "ConfirmDeleteFlyoutBtn";
			this.ConfirmDeleteFlyoutBtn.Size = new System.Drawing.Size(320, 69);
			this.ConfirmDeleteFlyoutBtn.TabIndex = 2;
			this.ConfirmDeleteFlyoutBtn.Text = "我是确认删除浮窗";
			this.ConfirmDeleteFlyoutBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ConfirmDeleteFlyoutBtn.UseVisualStyleBackColor = true;
			this.ConfirmDeleteFlyoutBtn.Click += new System.EventHandler(this.Button_Click);
			// 
			// TestControlsWinForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(1280, 720);
			this.Controls.Add(this.ConfirmDeleteFlyoutBtn);
			this.Controls.Add(this.PitchPickerBtn);
			this.Controls.Add(this.ComboBoxBtn);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F);
			this.Margin = new System.Windows.Forms.Padding(5);
			this.Name = "TestControlsWinForm";
			this.Text = "TestControlsWinForm";
			this.ResumeLayout(false);

	}

	#endregion

	private System.Windows.Forms.Button ComboBoxBtn;
	private System.Windows.Forms.Button PitchPickerBtn;
	private System.Windows.Forms.Button ConfirmDeleteFlyoutBtn;
}
