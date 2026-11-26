namespace Otomad.VegasScripts.OtomadHelper.V4 {
	partial class InputDialogForm {
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
			this._primaryPanel = new System.Windows.Forms.Panel();
			this._inputTextBox = new System.Windows.Forms.TextBox();
			this._secondaryPanel = new System.Windows.Forms.Panel();
			this._cancelButton = new System.Windows.Forms.Button();
			this._okButton = new System.Windows.Forms.Button();
			this._primaryPanel.SuspendLayout();
			this._secondaryPanel.SuspendLayout();
			this.SuspendLayout();
			//
			// _primaryPanel
			//
			this._primaryPanel.Controls.Add(this._inputTextBox);
			this._primaryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._primaryPanel.Location = new System.Drawing.Point(0, 0);
			this._primaryPanel.Name = "_primaryPanel";
			this._primaryPanel.Size = new System.Drawing.Size(343, 101);
			this._primaryPanel.TabIndex = 0;
			this._primaryPanel.Paint += new System.Windows.Forms.PaintEventHandler(this._primaryPanel_Paint);
			//
			// _inputTextBox
			//
			this._inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this._inputTextBox.Location = new System.Drawing.Point(12, 69);
			this._inputTextBox.Name = "_inputTextBox";
			this._inputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._inputTextBox.Size = new System.Drawing.Size(319, 20);
			this._inputTextBox.TabIndex = 0;
			//
			// _secondaryPanel
			//
			this._secondaryPanel.Controls.Add(this._cancelButton);
			this._secondaryPanel.Controls.Add(this._okButton);
			this._secondaryPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._secondaryPanel.Location = new System.Drawing.Point(0, 101);
			this._secondaryPanel.Name = "_secondaryPanel";
			this._secondaryPanel.Size = new System.Drawing.Size(343, 45);
			this._secondaryPanel.TabIndex = 1;
			this._secondaryPanel.Paint += new System.Windows.Forms.PaintEventHandler(this._secondaryPanel_Paint);
			//
			// _cancelButton
			//
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(256, 12);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 21);
			this._cancelButton.TabIndex = 1;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			//
			// _okButton
			//
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.Location = new System.Drawing.Point(175, 12);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 21);
			this._okButton.TabIndex = 0;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			this._okButton.Click += new System.EventHandler(this._okButton_Click);
			//
			// InputDialogForm
			//
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(343, 146);
			this.Controls.Add(this._primaryPanel);
			this.Controls.Add(this._secondaryPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputDialogForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NewInputBoxForm";
			this.UseSystemFont = true;
			this.Load += new System.EventHandler(this.NewInputBoxForm_Load);
			this._primaryPanel.ResumeLayout(false);
			this._primaryPanel.PerformLayout();
			this._secondaryPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel _primaryPanel;
		private System.Windows.Forms.Panel _secondaryPanel;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.TextBox _inputTextBox;
	}
}
