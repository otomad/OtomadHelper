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
			this.SuspendLayout();
			// 
			// ComboBoxBtn
			// 
			this.ComboBoxBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F);
			this.ComboBoxBtn.Location = new System.Drawing.Point(374, 242);
			this.ComboBoxBtn.Margin = new System.Windows.Forms.Padding(0);
			this.ComboBoxBtn.Name = "ComboBoxBtn";
			this.ComboBoxBtn.Size = new System.Drawing.Size(320, 69);
			this.ComboBoxBtn.TabIndex = 0;
			this.ComboBoxBtn.Text = "我是组合框";
			this.ComboBoxBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ComboBoxBtn.UseVisualStyleBackColor = true;
			this.ComboBoxBtn.Click += new System.EventHandler(this.ComboBoxBtn_Click);
			// 
			// TestControlsWinForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(1280, 720);
			this.Controls.Add(this.ComboBoxBtn);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F);
			this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.Name = "TestControlsWinForm";
			this.Text = "TestControlsWinForm";
			this.ResumeLayout(false);

	}

	#endregion

	private System.Windows.Forms.Button ComboBoxBtn;
}