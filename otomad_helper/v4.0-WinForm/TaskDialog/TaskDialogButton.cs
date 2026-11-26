#region Copyright 2009-2021 Ookii Dialogs Contributors
//
// Licensed under the BSD 3-Clause License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://opensource.org/licenses/BSD-3-Clause
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Otomad.VegasScripts.OtomadHelper.V4 {
	/// <summary>
	/// A button on a <see cref="TaskDialog"/>.
	/// </summary>
	/// <threadsafety instance="false" static="true" />
	public class TaskDialogButton : TaskDialogItem {
		private ButtonType _type;
		private bool _elevationRequired;
		private bool _default;
		private string _commandLinkNote;
		private DialogResult _dialogResult = DialogResult.None;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDialogButton"/> class.
		/// </summary>
		public TaskDialogButton() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDialogButton"/> class with the specified button type.
		/// </summary>
		/// <param name="type">The type of the button.</param>
		public TaskDialogButton(ButtonType type)
			: base((int)type) {
			_type = type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDialogButton"/> class with the specified container.
		/// </summary>
		/// <param name="container">The <see cref="IContainer"/> to add the <see cref="TaskDialogButton"/> to.</param>
		public TaskDialogButton(IContainer container)
			: base(container) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDialogButton"/> class with the specified text.
		/// </summary>
		/// <param name="text">The text of the button.</param>
		public TaskDialogButton(string text) {
			Text = text;
		}

		/// <summary>
		/// Gets or sets the type of the button.
		/// </summary>
		/// <value>
		/// One of the <see cref="Otomad.VegasScripts.OtomadHelper.V4.ButtonType"/> values that indicates the type of the button. The default value
		/// is <see cref="Otomad.VegasScripts.OtomadHelper.V4.ButtonType.Custom"/>.
		/// </value>
		[Category("Appearance"), Description("The type of the button."), DefaultValue(ButtonType.Custom)]
		public ButtonType ButtonType {
			get { return _type; }
			set {
				if (value != ButtonType.Custom) {
					CheckDuplicateButton(value, null);
					_type = value;
					base.Id = (int)value;
				} else {
					_type = value;
					AutoAssignId();
					UpdateOwner();
				}
			}
		}

		/// <summary>
		/// Gets or sets the text of the note associated with a command link button.
		/// </summary>
		/// <value>
		/// The text of the note associated with a command link button.
		/// </value>
		/// <remarks>
		/// <para>
		///   This property applies only to buttons where the <see cref="Type"/> property
		///   is <see cref="Otomad.VegasScripts.OtomadHelper.V4.ButtonType.Custom"/>. For other button types, it is ignored.
		/// </para>
		/// <para>
		///   In addition, it is used only if the <see cref="TaskDialog.ButtonStyle"/> property is set to
		///   <see cref="TaskDialogButtonStyle.CommandLinks"/> or <see cref="TaskDialogButtonStyle.CommandLinksNoIcon"/>;
		///   otherwise, it is ignored.
		/// </para>
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("The text of the note associated with a command link button."), DefaultValue(""), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
		public string CommandLinkNote {
			get { return _commandLinkNote ?? string.Empty; }
			set {
				_commandLinkNote = value;
				UpdateOwner();
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates if the button is the default button on the dialog.
		/// </summary>
		/// <value><see langword="true" /> if the button is the default button; otherwise, <see langword="false" />.
		/// The default value is <see langword="false" />.</value>
		/// <remarks>
		/// If no button has this property set to <see langword="true" />, the first button on the dialog will be the default button.
		/// </remarks>
		[Category("Behavior"), Description("Indicates if the button is the default button on the dialog."), DefaultValue(false)]
		public bool Default {
			get { return _default; }
			set {
				_default = value;
				if (value && Owner != null) {
					foreach (TaskDialogButton button in Owner.Buttons) {
						if (button != this)
							button.Default = false;
					}
				}
				UpdateOwner();
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the Task Dialog button or command link should have a
		/// User Account Control (UAC) shield icon (in other words, whether the action invoked by the
		/// button requires elevation).
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the button contains a UAC shield icon; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// Elevation is not performed by the task dialog; the code implementing the operation that results from
		/// the button being clicked is responsible for performing elevation if required.
		/// </remarks>
		[Category("Behavior"), Description("Indicates whether the Task Dialog button or command link should have a User Account Control (UAC) shield icon (in other words, whether the action invoked by the button requires elevation)."), DefaultValue(false)]
		public bool ElevationRequired {
			get { return _elevationRequired; }
			set {
				_elevationRequired = value;
				if (Owner != null)
					Owner.SetButtonElevationRequired(this);
			}
		}

		internal override int Id {
			get {
				return base.Id;
			}
			set {
				if (base.Id != value) {
					if (_type != ButtonType.Custom)
						throw new InvalidOperationException("Cannot change the id for a standard button.");
					base.Id = value;
				}
			}
		}

		internal override void AutoAssignId() {
			if (_type == ButtonType.Custom)
				base.AutoAssignId();
		}

		internal override void CheckDuplicate(TaskDialogItem itemToExclude) {
			CheckDuplicateButton(_type, itemToExclude);
			base.CheckDuplicate(itemToExclude);
		}

		internal TaskDialogCommonButtonFlags ButtonFlag {
			get {
				switch (_type) {
					case ButtonType.Ok:
						return TaskDialogCommonButtonFlags.OkButton;
					case ButtonType.Yes:
						return TaskDialogCommonButtonFlags.YesButton;
					case ButtonType.No:
						return TaskDialogCommonButtonFlags.NoButton;
					case ButtonType.Cancel:
						return TaskDialogCommonButtonFlags.CancelButton;
					case ButtonType.Retry:
						return TaskDialogCommonButtonFlags.RetryButton;
					case ButtonType.Close:
						return TaskDialogCommonButtonFlags.CloseButton;
					default:
						return 0;
				}
			}
		}

		public DialogResult DialogResult {
			get { return _dialogResult; }
			set { _dialogResult = value; }
		}

		/// <summary>
		/// Gets the collection that items of this type are part of.
		/// </summary>
		/// <value>
		/// If the <see cref="TaskDialogButton"/> is currently associated with a <see cref="TaskDialog"/>, the
		/// <see cref="TaskDialog.Buttons"/> collection of that <see cref="TaskDialog"/>; otherwise, <see langword="null" />.
		/// </value>
		protected override System.Collections.IEnumerable ItemCollection {
			get {
				if (Owner != null)
					return Owner.Buttons;
				return null;
			}
		}

		private void CheckDuplicateButton(ButtonType type, TaskDialogItem itemToExclude) {
			if (type != ButtonType.Custom && Owner != null) {
				foreach (TaskDialogButton button in Owner.Buttons) {
					if (button != this && button != itemToExclude && button.ButtonType == type)
						throw new InvalidOperationException("The task dialog already has a non-custom button with the same type.");
				}
			}
		}
	}

	/// <summary>
	/// Represents the type of a task dialog button.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
	public enum ButtonType {
		/// <summary>
		/// The button is a custom button.
		/// </summary>
		Custom = 0,
		/// <summary>
		/// The button is the common OK button.
		/// </summary>
		Ok = 1,
		/// <summary>
		/// The button is the common Yes button.
		/// </summary>
		Yes = 6,
		/// <summary>
		/// The button is the common No button.
		/// </summary>
		No = 7,
		/// <summary>
		/// The button is the common Cancel button.
		/// </summary>
		Cancel = 2,
		/// <summary>
		/// The button is the common Retry button.
		/// </summary>
		Retry = 4,
		/// <summary>
		/// The button is the common Close button.
		/// </summary>
		Close = 8
	}

	/// <summary>
	/// Indicates the display style of custom buttons on a task dialog.
	/// </summary>
	public enum TaskDialogButtonStyle {
		/// <summary>
		/// Custom buttons are displayed as regular buttons.
		/// </summary>
		Standard,
		/// <summary>
		/// Custom buttons are displayed as command links using a standard task dialog glyph.
		/// </summary>
		CommandLinks,
		/// <summary>
		/// Custom buttons are displayed as command links without a glyph.
		/// </summary>
		CommandLinksNoIcon
	}

	[Flags]
	public enum TaskDialogCommonButtonFlags {
		OkButton = 0x0001, // selected control return value IDOK
		YesButton = 0x0002, // selected control return value IDYES
		NoButton = 0x0004, // selected control return value IDNO
		CancelButton = 0x0008, // selected control return value IDCANCEL
		RetryButton = 0x0010, // selected control return value IDRETRY
		CloseButton = 0x0020  // selected control return value IDCLOSE
	}
}
