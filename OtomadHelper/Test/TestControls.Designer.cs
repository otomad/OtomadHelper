namespace OtomadHelper.Test;

partial class TestControls {
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
			this.ComboBoxBtn.Location = new System.Drawing.Point(234, 151);
			this.ComboBoxBtn.Margin = new System.Windows.Forms.Padding(0);
			this.ComboBoxBtn.Name = "ComboBoxBtn";
			this.ComboBoxBtn.Size = new System.Drawing.Size(200, 43);
			this.ComboBoxBtn.TabIndex = 0;
			this.ComboBoxBtn.Text = "我是组合框";
			this.ComboBoxBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ComboBoxBtn.UseVisualStyleBackColor = true;
			this.ComboBoxBtn.Click += new System.EventHandler(this.ComboBoxBtn_Click);
			// 
			// TestControls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.ComboBoxBtn);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F);
			this.Name = "TestControls";
			this.Text = "TestControls";
			this.ResumeLayout(false);

	}

	#endregion

	private System.Windows.Forms.Button ComboBoxBtn;
}