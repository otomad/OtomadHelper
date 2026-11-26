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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing.Design;
using System.IO;
using System.Security.Permissions;
using System.Runtime.ConstrainedExecution;

using Microsoft.Win32.SafeHandles;
using System.Drawing;

namespace Otomad.VegasScripts.OtomadHelper.V4 {
	/// <summary>
	/// Displays a Task Dialog.
	/// </summary>
	/// <remarks>
	/// The task dialog contains an application-defined message text and title, icons, and any combination of predefined push buttons.
	/// Task Dialogs are supported only on Windows Vista and above. No fallback is provided; if you wish to use task dialogs
	/// and support operating systems older than Windows Vista, you must provide a fallback yourself. Check the <see cref="OSSupportsTaskDialogs"/>
	/// property to see if task dialogs are supported. It is safe to instantiate the <see cref="TaskDialog"/> class on an older
	/// OS, but calling <see cref="Show"/> or <see cref="ShowDialog()"/> will throw an exception.
	/// </remarks>
	/// <threadsafety static="true" instance="false" />
	[DefaultProperty("MainInstruction"), DefaultEvent("ButtonClicked"), Description("Displays a task dialog.")]
	public partial class TaskDialog : Component, IWin32Window {
		public static DialogResult ShowTaskDialogWithCommandLinks(string text, string caption = "", string content = "", MessageBoxButtons buttons = MessageBoxButtons.OK, string[] buttonTitles = null, TaskDialogIcon icon = TaskDialogIcon.Custom, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1, Icon customIcon = null) {
			TaskDialog taskDialog = new TaskDialog {
				WindowTitle = caption,
				MainInstruction = text,
				Content = content,
				ButtonStyle = TaskDialogButtonStyle.CommandLinks,
				MainIcon = icon,
				CustomMainIcon = customIcon,
				AllowDialogCancellation = true,
			};
			DialogResult[] dialogResults = GetDialogResultsFromMessageBoxButtons(buttons);
			if (buttonTitles == null || buttonTitles.Length < dialogResults.Length)
				throw new ArgumentException("The provided button titles in ShowTaskDialogWithCommandLinks is not enough", "buttons");
			int defaultButtonIndex = defaultButton == MessageBoxDefaultButton.Button2 ? 1 : defaultButton == MessageBoxDefaultButton.Button3 ? 2 : 0;
			for (int i = 0; i < dialogResults.Length; i++) {
				DialogResult dialogResult = dialogResults[i];
				string buttonTitle = buttonTitles[i];
				bool isDefault = i == defaultButtonIndex;
				ButtonType buttonType = dialogResult == DialogResult.Cancel ? ButtonType.Cancel : ButtonType.Custom;
				TaskDialogButton button = new TaskDialogButton(buttonType) {
					Text = buttonTitle,
					DialogResult = dialogResult,
					Default = isDefault,
				};
				taskDialog.Buttons.Add(button);
			}
			TaskDialogButton resultButton = taskDialog.ShowDialog();
			return resultButton == null ? DialogResult.Cancel : resultButton.DialogResult;
		}

		private static DialogResult[] GetDialogResultsFromMessageBoxButtons(MessageBoxButtons buttons) {
			switch (buttons) {
				case MessageBoxButtons.OK:
					return new DialogResult[] { DialogResult.OK };
				case MessageBoxButtons.OKCancel:
					return new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
				case MessageBoxButtons.AbortRetryIgnore:
					return new DialogResult[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore };
				case MessageBoxButtons.YesNoCancel:
					return new DialogResult[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel };
				case MessageBoxButtons.YesNo:
					return new DialogResult[] { DialogResult.Yes, DialogResult.No };
				case MessageBoxButtons.RetryCancel:
					return new DialogResult[] { DialogResult.Retry, DialogResult.Cancel };
				default:
					throw new ArgumentOutOfRangeException("buttons", buttons, null);
			}
		}

		internal static void Test() {
			var _sampleCommandLinkTaskDialog = new TaskDialog {
				ButtonStyle = TaskDialogButtonStyle.CommandLinks,
				Content = "Besides regular buttons, task dialogs also support command links. Only custom buttons are shown as command links; standard buttons remain regular buttons.",
				MainInstruction = "This is a sample task dialog with command links.",
				WindowTitle = "Task dialog sample"
			};
			var _elevatedTaskDialogButton = new TaskDialogButton {
				CommandLinkNote = "Both regular buttons and command links can show the shield icon to indicate that the action they perform requires elevation. It is up to the application to actually perform the elevation.",
				ElevationRequired = true,
				Text = "An action requiring elevation"
			};
			var _otherTaskDialogButton = new TaskDialogButton {
				Text = "Some other action"
			};
			var _cancel2TaskDialogButton = new TaskDialogButton {
				DialogResult = DialogResult.Cancel,
			};
			_sampleCommandLinkTaskDialog.Buttons.Add(_elevatedTaskDialogButton);
			_sampleCommandLinkTaskDialog.Buttons.Add(_otherTaskDialogButton);
			_sampleCommandLinkTaskDialog.Buttons.Add(_cancel2TaskDialogButton);
			_sampleCommandLinkTaskDialog.ShowDialog();
		}

		#region Events

		/// <summary>
		/// Event raised when the task dialog has been created.
		/// </summary>
		/// <remarks>
		/// This event is raised once after calling <see cref="ShowDialog(IWin32Window)"/>, after the dialog
		/// is created and before it is displayed.
		/// </remarks>
		[Category("Behavior"), Description("Event raised when the task dialog has been created.")]
		public event EventHandler Created;
		/// <summary>
		/// Event raised when the task dialog has been destroyed.
		/// </summary>
		/// <remarks>
		/// The task dialog window no longer exists when this event is raised.
		/// </remarks>
		[Category("Behavior"), Description("Event raised when the task dialog has been destroyed.")]
		public event EventHandler Destroyed;
		/// <summary>
		/// Event raised when the user clicks a button on the task dialog.
		/// </summary>
		/// <remarks>
		/// Set the <see cref="CancelEventArgs.Cancel"/> property to <see langword="true" /> to prevent the dialog from being closed.
		/// </remarks>
		[Category("Action"), Description("Event raised when the user clicks a button.")]
		public event EventHandler<TaskDialogItemClickedEventArgs> ButtonClicked;
		/// <summary>
		/// Event raised when the user clicks a radio button on the task dialog.
		/// </summary>
		/// <remarks>
		/// The <see cref="CancelEventArgs.Cancel"/> property is ignored for this event.
		/// </remarks>
		[Category("Action"), Description("Event raised when the user clicks a button.")]
		public event EventHandler<TaskDialogItemClickedEventArgs> RadioButtonClicked;
		/// <summary>
		/// Event raised when the user clicks a hyperlink.
		/// </summary>
		[Category("Action"), Description("Event raised when the user clicks a hyperlink.")]
		public event EventHandler<HyperlinkClickedEventArgs> HyperlinkClicked;
		/// <summary>
		/// Event raised when the user clicks the verification check box.
		/// </summary>
		[Category("Action"), Description("Event raised when the user clicks the verification check box.")]
		public event EventHandler VerificationClicked;
		/// <summary>
		/// Event raised periodically while the dialog is displayed.
		/// </summary>
		/// <remarks>
		/// <para>
		///   This event is raised only when the <see cref="RaiseTimerEvent"/> property is set to <see langword="true" />. The event is
		///   raised approximately every 200 milliseconds.
		/// </para>
		/// <para>
		///   To reset the tick count, set the <see cref="TimerEventArgs.ResetTickCount" />
		///   property to <see langword="true" />.
		/// </para>
		/// </remarks>
		[Category("Behavior"), Description("Event raised periodically while the dialog is displayed.")]
		public event EventHandler<TimerEventArgs> Timer;
		/// <summary>
		/// Event raised when the user clicks the expand button on the task dialog.
		/// </summary>
		/// <remarks>
		/// The <see cref="ExpandButtonClickedEventArgs.Expanded"/> property indicates if the expanded information is visible
		/// or not after the click.
		/// </remarks>
		[Category("Action"), Description("Event raised when the user clicks the expand button on the task dialog.")]
		public event EventHandler<ExpandButtonClickedEventArgs> ExpandButtonClicked;
		/// <summary>
		/// Event raised when the user presses F1 while the dialog has focus.
		/// </summary>
		[Category("Action"), Description("Event raised when the user presses F1 while the dialog has focus.")]
		public event EventHandler HelpRequested;

		#endregion

		#region Fields

		private TaskDialogItemCollection<TaskDialogButton> _buttons;
		private TaskDialogItemCollection<TaskDialogRadioButton> _radioButtons;
		private TASKDIALOGCONFIG _config = new TASKDIALOGCONFIG();
		private TaskDialogIcon _mainIcon;
		private System.Drawing.Icon _customMainIcon;
		private System.Drawing.Icon _customFooterIcon;
		private TaskDialogIcon _footerIcon;
		private Dictionary<int, TaskDialogButton> _buttonsById;
		private Dictionary<int, TaskDialogRadioButton> _radioButtonsById;
		private IntPtr _handle;
		private int _progressBarMarqueeAnimationSpeed = 100;
		private int _progressBarMinimimum;
		private int _progressBarMaximum = 100;
		private int _progressBarValue;
		private ProgressBarState _progressBarState = ProgressBarState.Normal;
		private int _inEventHandler;
		private bool _updatePending;
		private System.Drawing.Icon _windowIcon;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDialog"/> class.
		/// </summary>
		public TaskDialog() {
			InitializeComponent();

			_config.cbSize = (uint)Marshal.SizeOf(typeof(TASKDIALOGCONFIG));
			_config.pfCallback = new TaskDialogCallback(TaskDialogCallback);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDialog"/> class with the specified container.
		/// </summary>
		/// <param name="container">The <see cref="IContainer"/> to add the <see cref="TaskDialog"/> to.</param>
		public TaskDialog(IContainer container) {
			if (container != null)
				container.Add(this);

			InitializeComponent();

			_config.cbSize = (uint)Marshal.SizeOf(typeof(TASKDIALOGCONFIG));
			_config.pfCallback = new TaskDialogCallback(TaskDialogCallback);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets a value that indicates whether the current operating system supports task dialogs.
		/// </summary>
		/// <value>
		/// Returns <see langword="true" /> for Windows Vista or later; otherwise <see langword="false" />.
		/// </value>
		public static bool OSSupportsTaskDialogs {
			get {
				return IsWindowsVistaOrLater;
			}
		}

		/// <summary>
		/// Gets a list of the buttons on the Task Dialog.
		/// </summary>
		/// <value>
		/// A list of the buttons on the Task Dialog.
		/// </value>
		/// <remarks>
		/// Custom buttons are displayed in the order they have in the collection. Standard buttons will always be displayed
		/// in the Windows-defined order, regardless of the order of the buttons in the collection.
		/// </remarks>
		[Localizable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("A list of the buttons on the Task Dialog.")]
		public TaskDialogItemCollection<TaskDialogButton> Buttons {
			get { return _buttons ?? (_buttons = new TaskDialogItemCollection<TaskDialogButton>(this)); }
		}

		/// <summary>
		/// Gets a list of the radio buttons on the Task Dialog.
		/// </summary>
		/// <value>
		/// A list of the radio buttons on the Task Dialog.
		/// </value>
		[Localizable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("A list of the radio buttons on the Task Dialog.")]
		public TaskDialogItemCollection<TaskDialogRadioButton> RadioButtons {
			get { return _radioButtons ?? (_radioButtons = new TaskDialogItemCollection<TaskDialogRadioButton>(this)); }
		}

		/// <summary>
		/// Gets or sets the window title of the task dialog.
		/// </summary>
		/// <value>
		/// The window title of the task dialog. The default is an empty string ("").
		/// </value>
		[Localizable(true), Category("Appearance"), Description("The window title of the task dialog."), DefaultValue("")]
		public string WindowTitle {
			get {
				return _config.pszWindowTitle ?? string.Empty;
			}
			set {
				_config.pszWindowTitle = string.IsNullOrEmpty(value) ? null : value;
				UpdateDialog();
			}
		}

		/// <summary>
		/// Gets or sets the dialog's main instruction.
		/// </summary>
		/// <value>
		/// The dialog's main instruction. The default is an empty string ("").
		/// </value>
		/// <remarks>
		/// The main instruction of a task dialog will be displayed in a larger font and a different color than
		/// the other text of the task dialog.
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("The dialog's main instruction."), DefaultValue(""), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
		public string MainInstruction {
			get { return _config.pszMainInstruction ?? string.Empty; }
			set {
				_config.pszMainInstruction = string.IsNullOrEmpty(value) ? null : value;
				SetElementText(TaskDialogElements.MainInstruction, MainInstruction);
			}
		}

		/// <summary>
		/// Gets or sets the dialog's primary content.
		/// </summary>
		/// <value>
		/// The dialog's primary content. The default is an empty string ("").
		/// </value>
		[Localizable(true), Category("Appearance"), Description("The dialog's primary content."), DefaultValue(""), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
		public string Content {
			get { return _config.pszContent ?? string.Empty; }
			set {
				_config.pszContent = string.IsNullOrEmpty(value) ? null : value;
				SetElementText(TaskDialogElements.Content, Content);
			}
		}

		/// <summary>
		/// Gets or sets the icon to be used in the title bar of the dialog.
		/// </summary>
		/// <value>
		/// An <see cref="System.Drawing.Icon"/> that represents the icon of the task dialog's window.
		/// </value>
		/// <remarks>
		/// This property is used only when the dialog is shown as a modeless dialog; if the dialog
		/// is modal, it will have no icon.
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("The icon to be used in the title bar of the dialog. Used only when the dialog is shown as a modeless dialog."), DefaultValue(null)]
		public System.Drawing.Icon WindowIcon {
			get {
				if (IsDialogRunning) {
					const int WM_GETICON = 0x007F, ICON_SMALL = 0;
					IntPtr icon = SendMessage(Handle, WM_GETICON, new IntPtr(ICON_SMALL), IntPtr.Zero);
					return System.Drawing.Icon.FromHandle(icon);
				}
				return _windowIcon;
			}
			set { _windowIcon = value; }
		}

		/// <summary>
		/// Gets or sets the icon to display in the task dialog.
		/// </summary>
		/// <value>
		/// A <see cref="TaskDialogIcon"/> that indicates the icon to display in the main content area of the task dialog.
		/// The default is <see cref="TaskDialogIcon.Custom"/>.
		/// </value>
		/// <remarks>
		/// When this property is set to <see cref="TaskDialogIcon.Custom"/>, use the <see cref="CustomMainIcon"/> property to
		/// specify the icon to use.
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("The icon to display in the task dialog."), DefaultValue(TaskDialogIcon.Custom)]
		public TaskDialogIcon MainIcon {
			get { return _mainIcon; }
			set {
				if (_mainIcon != value) {
					_mainIcon = value;
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a custom icon to display in the dialog.
		/// </summary>
		/// <value>
		/// An <see cref="System.Drawing.Icon"/> that represents the icon to display in the main content area of the task dialog,
		/// or <see langword="null" /> if no custom icon is used. The default value is <see langword="null"/>.
		/// </value>
		/// <remarks>
		/// This property is ignored if the <see cref="MainIcon"/> property has a value other than <see cref="TaskDialogIcon.Custom"/>.
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("A custom icon to display in the dialog."), DefaultValue(null)]
		public System.Drawing.Icon CustomMainIcon {
			get { return _customMainIcon; }
			set {
				if (_customMainIcon != value) {
					_customMainIcon = value;
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets the icon to display in the footer area of the task dialog.
		/// </summary>
		/// <value>
		/// A <see cref="TaskDialogIcon"/> that indicates the icon to display in the footer area of the task dialog.
		/// The default is <see cref="TaskDialogIcon.Custom"/>.
		/// </value>
		/// <remarks>
		/// <para>
		///   When this property is set to <see cref="TaskDialogIcon.Custom"/>, use the <see cref="CustomFooterIcon"/> property to
		///   specify the icon to use.
		/// </para>
		/// <para>
		///   The footer icon is displayed only if the <see cref="Footer"/> property is not an empty string ("").
		/// </para>
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("The icon to display in the footer area of the task dialog."), DefaultValue(TaskDialogIcon.Custom)]
		public TaskDialogIcon FooterIcon {
			get { return _footerIcon; }
			set {
				if (_footerIcon != value) {
					_footerIcon = value;
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a custom icon to display in the footer area of the task dialog.
		/// </summary>
		/// <value>
		/// An <see cref="System.Drawing.Icon"/> that represents the icon to display in the footer area of the task dialog,
		/// or <see langword="null" /> if no custom icon is used. The default value is <see langword="null"/>.
		/// </value>
		/// <remarks>
		/// <para>
		///   This property is ignored if the <see cref="FooterIcon"/> property has a value other than <see cref="TaskDialogIcon.Custom"/>.
		/// </para>
		/// <para>
		///   The footer icon is displayed only if the <see cref="Footer"/> property is not an empty string ("").
		/// </para>
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("A custom icon to display in the footer area of the task dialog."), DefaultValue(null)]
		public System.Drawing.Icon CustomFooterIcon {
			get { return _customFooterIcon; }
			set {
				if (_customFooterIcon != value) {
					_customFooterIcon = value;
					// TODO: This and customMainIcon don't need to use UpdateDialog, they can use TDM_UPDATE_ICON
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether custom buttons should be displayed as normal buttons or command links.
		/// </summary>
		/// <value>
		/// A <see cref="TaskDialogButtonStyle"/> that indicates the display style of custom buttons on the dialog.
		/// The default value is <see cref="TaskDialogButtonStyle.Standard"/>.
		/// </value>
		/// <remarks>
		/// <para>
		///   This property affects only custom buttons, not standard ones.
		/// </para>
		/// <para>
		///   If a custom button is being displayed on a task dialog
		///   with <see cref="TaskDialog.ButtonStyle"/> set to <see cref="Otomad.VegasScripts.OtomadHelper.V4.TaskDialogButtonStyle.CommandLinks"/>
		///   or <see cref="Otomad.VegasScripts.OtomadHelper.V4.TaskDialogButtonStyle.CommandLinksNoIcon"/>, you delineate the command from the
		///   note by placing a line break in the string specified by <see cref="TaskDialogItem.Text"/> property.
		/// </para>
		/// </remarks>
		[Category("Behavior"), Description("Indicates whether custom buttons should be displayed as normal buttons or command links."), DefaultValue(TaskDialogButtonStyle.Standard)]
		public TaskDialogButtonStyle ButtonStyle {
			get {
				return GetFlag(TaskDialogFlags.UseCommandLinksNoIcon) ? TaskDialogButtonStyle.CommandLinksNoIcon :
					GetFlag(TaskDialogFlags.UseCommandLinks) ? TaskDialogButtonStyle.CommandLinks :
					TaskDialogButtonStyle.Standard;
			}
			set {
				SetFlag(TaskDialogFlags.UseCommandLinks, value == TaskDialogButtonStyle.CommandLinks);
				SetFlag(TaskDialogFlags.UseCommandLinksNoIcon, value == TaskDialogButtonStyle.CommandLinksNoIcon);
				UpdateDialog();
			}
		}

		/// <summary>
		/// Gets or sets the label for the verification checkbox.
		/// </summary>
		/// <value>
		/// The label for the verification checkbox, or an empty string ("") if no verification checkbox
		/// is shown. The default value is an empty string ("").
		/// </value>
		/// <remarks>
		/// If no text is set, the verification checkbox will not be shown.
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("The label for the verification checkbox."), DefaultValue("")]
		public string VerificationText {
			get { return _config.pszVerificationText ?? string.Empty; }
			set {
				string realValue = string.IsNullOrEmpty(value) ? null : value;
				if (_config.pszVerificationText != realValue) {
					_config.pszVerificationText = realValue;
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the verification checkbox is checked ot not.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the verficiation checkbox is checked; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// <para>
		///   Set this property before displaying the dialog to determine the initial state of the check box.
		///   Use this property after displaying the dialog to determine whether the check box was checked when
		///   the user closed the dialog.
		/// </para>
		/// <note>
		///   This property is only used if <see cref="VerificationText"/> is not an empty string ("").
		/// </note>
		/// </remarks>
		[Category("Behavior"), Description("Indicates whether the verification checkbox is checked ot not."), DefaultValue(false)]
		public bool IsVerificationChecked {
			get { return GetFlag(TaskDialogFlags.VerificationFlagChecked); }
			set {
				if (value != IsVerificationChecked) {
					SetFlag(TaskDialogFlags.VerificationFlagChecked, value);
					if (IsDialogRunning)
						ClickVerification(value, false);
				}
			}
		}

		/// <summary>
		/// Gets or sets additional information to be displayed on the dialog.
		/// </summary>
		/// <value>
		/// Additional information to be displayed on the dialog. The default value is an empty string ("").
		/// </value>
		/// <remarks>
		/// <para>
		///   When this property is not an empty string (""), a control is shown on the task dialog that
		///   allows the user to expand and collapse the text specified in this property.
		/// </para>
		/// <para>
		///   The text is collapsed by default unless <see cref="ExpandedByDefault"/> is set to <see langword="true" />.
		/// </para>
		/// <para>
		///   The expanded text is shown in the main content area of the dialog, unless <see cref="ExpandFooterArea"/>
		///   is set to <see langword="true" />, in which case it is shown in the footer area.
		/// </para>
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("Additional information to be displayed on the dialog."), DefaultValue(""), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
		public string ExpandedInformation {
			get { return _config.pszExpandedInformation ?? string.Empty; }
			set {
				_config.pszExpandedInformation = string.IsNullOrEmpty(value) ? null : value;
				SetElementText(TaskDialogElements.ExpandedInformation, ExpandedInformation);
			}
		}

		/// <summary>
		/// Gets or sets the text to use for the control for collapsing the expandable information specified in <see cref="ExpandedInformation"/>.
		/// </summary>
		/// <value>
		/// The text to use for the control for collapsing the expandable information, or an empty string ("") if the
		/// operating system's default text is to be used. The default is an empty string ("")
		/// </value>
		/// <remarks>
		/// <para>
		///   If this text is not specified and <see cref="CollapsedControlText"/> is specified, the value of <see cref="CollapsedControlText"/>
		///   will be used for this property as well. If neither is specified, the operating system's default text is used.
		/// </para>
		/// <note>
		///   The control for collapsing or expanding the expandable information is displayed only if <see cref="ExpandedInformation"/> is not
		///   an empty string ("")
		/// </note>
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("The text to use for the control for collapsing the expandable information."), DefaultValue("")]
		public string ExpandedControlText {
			get { return _config.pszExpandedControlText ?? string.Empty; }
			set {
				string realValue = string.IsNullOrEmpty(value) ? null : value;
				if (_config.pszExpandedControlText != realValue) {
					_config.pszExpandedControlText = realValue;
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets the text to use for the control for expading the expandable information specified in <see cref="ExpandedInformation"/>.
		/// </summary>
		/// <value>
		/// The text to use for the control for expanding the expandable information, or an empty string ("") if the
		/// operating system's default text is to be used. The default is an empty string ("")
		/// </value>
		/// <remarks>
		/// <para>
		///   If this text is not specified and <see cref="ExpandedControlText"/> is specified, the value of <see cref="ExpandedControlText"/>
		///   will be used for this property as well. If neither is specified, the operating system's default text is used.
		/// </para>
		/// <note>
		///   The control for collapsing or expanding the expandable information is displayed only if <see cref="ExpandedInformation"/> is not
		///   an empty string ("")
		/// </note>
		/// </remarks>
		[Localizable(true), Category("Appearance"), Description("The text to use for the control for expanding the expandable information."), DefaultValue("")]
		public string CollapsedControlText {
			get { return _config.pszCollapsedControlText ?? string.Empty; }
			set {
				string realValue = string.IsNullOrEmpty(value) ? null : value;
				if (_config.pszCollapsedControlText != realValue) {
					_config.pszCollapsedControlText = string.IsNullOrEmpty(value) ? null : value;
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets the text to be used in the footer area of the task dialog.
		/// </summary>
		/// <value>
		/// The text to be used in the footer area of the task dialog, or an empty string ("")
		/// if the footer area is not displayed. The default value is an empty string ("").
		/// </value>
		[Localizable(true), Category("Appearance"), Description("The text to be used in the footer area of the task dialog."), DefaultValue(""), Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(UITypeEditor))]
		public string Footer {
			get { return _config.pszFooterText ?? string.Empty; }
			set {
				_config.pszFooterText = string.IsNullOrEmpty(value) ? null : value;
				SetElementText(TaskDialogElements.Footer, Footer);
			}
		}

		/// <summary>
		/// Specifies the width of the task dialog's client area in DLU's.
		/// </summary>
		/// <value>
		/// The width of the task dialog's client area in DLU's, or 0 to have the task dialog calculate the ideal width.
		/// The default value is 0.
		/// </value>
		[Localizable(true), Category("Appearance"), Description("the width of the task dialog's client area in DLU's. If 0, task dialog will calculate the ideal width."), DefaultValue(0)]
		public int Width {
			get { return (int)_config.cxWidth; }
			set {
				if (_config.cxWidth != (uint)value) {
					_config.cxWidth = (uint)value;
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether hyperlinks are allowed for the <see cref="Content"/>, <see cref="ExpandedInformation"/>
		/// and <see cref="Footer"/> properties.
		/// </summary>
		/// <value>
		/// <see langword="true" /> when hyperlinks are allowed for the <see cref="Content"/>, <see cref="ExpandedInformation"/>
		/// and <see cref="Footer"/> properties; otherwise, <see langword="false" />. The default value is <see langword="false" />.
		/// </value>
		/// <remarks>
		/// <para>
		///   When  this property is <see langword="true" />, the <see cref="Content"/>, <see cref="ExpandedInformation"/>
		///   and <see cref="Footer"/> properties can use hyperlinks in the following form: <c>&lt;A HREF="executablestring"&gt;Hyperlink Text&lt;/A&gt;</c>
		/// </para>
		/// <note>
		///   Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
		/// </note>
		/// <para>
		///   Task dialogs will not actually execute hyperlinks. To take action when the user presses a hyperlink, handle the
		///   <see cref="HyperlinkClicked"/> event.
		/// </para>
		/// </remarks>
		[Category("Behavior"), Description("Indicates whether hyperlinks are allowed for the Content, ExpandedInformation and Footer properties."), DefaultValue(false)]
		public bool EnableHyperlinks {
			get { return GetFlag(TaskDialogFlags.EnableHyperLinks); }
			set {
				if (EnableHyperlinks != value) {
					SetFlag(TaskDialogFlags.EnableHyperLinks, value);
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates that the dialog should be able to be closed using Alt-F4, Escape and the title
		/// bar's close button even if no cancel button is specified.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the dialog can be closed using Alt-F4, Escape and the title
		/// bar's close button even if no cancel button is specified; otherwise, <see langword="false" />.
		/// The default value is <see langword="false" />.
		/// </value>
		[Category("Behavior"), Description("Indicates that the dialog should be able to be closed using Alt-F4, Escape and the title bar's close button even if no cancel button is specified."), DefaultValue(false)]
		public bool AllowDialogCancellation {
			get { return GetFlag(TaskDialogFlags.AllowDialogCancellation); }
			set {
				if (AllowDialogCancellation != value) {
					SetFlag(TaskDialogFlags.AllowDialogCancellation, value);
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates that the string specified by the <see cref="ExpandedInformation" /> property
		/// should be displayed at the bottom of the dialog's footer area instead of immediately after the dialog's content.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the string specified by the <see cref="ExpandedInformation" /> property
		/// should be displayed at the bottom of the dialog's footer area instead of immediately after the dialog's content;
		/// otherwise, <see langword="false" />. The default value is <see langword="false" />.
		/// </value>
		[Category("Behavior"), Description("Indicates that the string specified by the ExpandedInformation property should be displayed at the bottom of the dialog's footer area instead of immediately after the dialog's content."), DefaultValue(false)]
		public bool ExpandFooterArea {
			get { return GetFlag(TaskDialogFlags.ExpandFooterArea); }
			set {
				if (ExpandFooterArea != value) {
					SetFlag(TaskDialogFlags.ExpandFooterArea, value);
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates that the string specified by the <see cref="ExpandedInformation"/> property
		/// should be displayed by default.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the string specified by the <see cref="ExpandedInformation"/> property
		/// should be displayed by default; <see langword="false" /> if it is hidden by default. The default value is
		/// <see langword="false" />.
		/// </value>
		[Category("Behavior"), Description("Indicates that the string specified by the ExpandedInformation property should be displayed by default."), DefaultValue(false)]
		public bool ExpandedByDefault {
			get { return GetFlag(TaskDialogFlags.ExpandedByDefault); }
			set {
				if (ExpandedByDefault != value) {
					SetFlag(TaskDialogFlags.ExpandedByDefault, value);
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the <see cref="Timer"/> event is raised periodically while the dialog
		/// is visible.
		/// </summary>
		/// <value>
		/// <see langword="true" /> when the <see cref="Timer"/> event is raised periodically while the dialog is visible; otherwise,
		/// <see langword="false" />. The default value is <see langword="false" />.
		/// </value>
		/// <remarks>
		/// The <see cref="Timer"/> event will be raised approximately every 200 milliseconds if this property is <see langword="true" />.
		/// </remarks>
		[Category("Behavior"), Description("Indicates whether the Timer event is raised periodically while the dialog is visible."), DefaultValue(false)]
		public bool RaiseTimerEvent {
			get { return GetFlag(TaskDialogFlags.CallbackTimer); }
			set {
				if (RaiseTimerEvent != value) {
					SetFlag(TaskDialogFlags.CallbackTimer, value);
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the dialog is centered in the parent window instead of the screen.
		/// </summary>
		/// <value>
		/// <see langword="true" /> when the dialog is centered relative to the parent window; <see langword="false" /> when it is centered on the screen.
		/// The default value is <see langword="false" />.
		/// </value>
		[Category("Layout"), Description("Indicates whether the dialog is centered in the parent window instead of the screen."), DefaultValue(false)]
		public bool CenterParent {
			get { return GetFlag(TaskDialogFlags.PositionRelativeToWindow); }
			set {
				if (CenterParent != value) {
					SetFlag(TaskDialogFlags.PositionRelativeToWindow, value);
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether text is displayed right to left.
		/// </summary>
		/// <value>
		/// <see langword="true" /> when the content of the dialog is displayed right to left; otherwise, <see langword="false" />.
		/// The default value is <see langword="false" />.
		/// </value>
		[Localizable(true), Category("Appearance"), Description("Indicates whether text is displayed right to left."), DefaultValue(false)]
		public bool RightToLeft {
			get { return GetFlag(TaskDialogFlags.RtlLayout); }
			set {
				if (RightToLeft != value) {
					SetFlag(TaskDialogFlags.RtlLayout, value);
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the dialog has a minimize box on its caption bar.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the dialog has a minimize box on its caption bar when modeless; otherwise,
		/// <see langword="false" />. The default is <see langword="false" />.
		/// </value>
		/// <remarks>
		/// A task dialog can only have a minimize box if it is displayed as a modeless dialog. The minimize box
		/// will never appear when using the designer "Preview" option, since that displays the dialog modally.
		/// </remarks>
		[Category("Window Style"), Description("Indicates whether the dialog has a minimize box on its caption bar."), DefaultValue(false)]
		public bool MinimizeBox {
			get { return GetFlag(TaskDialogFlags.CanBeMinimized); }
			set {
				if (MinimizeBox != value) {
					SetFlag(TaskDialogFlags.CanBeMinimized, value);
					UpdateDialog();
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of progress bar displayed on the dialog.
		/// </summary>
		/// <value>
		/// A <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2"/> that indicates the type of progress bar shown on the task dialog.
		/// </value>
		/// <remarks>
		/// <para>
		///   If this property is set to <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2.MarqueeProgressBar"/>, the marquee will
		///   scroll as long as the dialog is visible.
		/// </para>
		/// <para>
		///   If this property is set to <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2.ProgressBar"/>, the value of the
		///   <see cref="ProgressBarValue" /> property must be updated to advance the progress bar. This can be done e.g. by
		///   an asynchronous operation or from the <see cref="Timer"/> event.
		/// </para>
		/// <note>
		///   Updating the value of the progress bar using the <see cref="ProgressBarValue"/> while the dialog is visible property may only be done from
		///   the thread on which the task dialog was created.
		/// </note>
		/// </remarks>
		[Category("Behavior"), Description("The type of progress bar displayed on the dialog."), DefaultValue(ProgressBarStyle2.None)]
		public ProgressBarStyle2 ProgressBarStyle {
			get {
				if (GetFlag(TaskDialogFlags.ShowMarqueeProgressBar))
					return ProgressBarStyle2.MarqueeProgressBar;
				else if (GetFlag(TaskDialogFlags.ShowProgressBar))
					return ProgressBarStyle2.ProgressBar;
				else
					return ProgressBarStyle2.None;
			}
			set {
				SetFlag(TaskDialogFlags.ShowMarqueeProgressBar, value == ProgressBarStyle2.MarqueeProgressBar);
				SetFlag(TaskDialogFlags.ShowProgressBar, value == ProgressBarStyle2.ProgressBar);
				UpdateProgressBarStyle();
			}
		}

		/// <summary>
		/// Gets or sets the marquee animation speed of the progress bar in milliseconds.
		/// </summary>
		/// <value>
		/// The marquee animation speed of the progress bar in milliseconds. The default value is 100.
		/// </value>
		/// <remarks>
		/// This property is only used if the <see cref="ProgressBarStyle"/> property is
		/// <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2.MarqueeProgressBar"/>.
		/// </remarks>
		[Category("Behavior"), Description("The marquee animation speed of the progress bar in milliseconds."), DefaultValue(100)]
		public int ProgressBarMarqueeAnimationSpeed {
			get { return _progressBarMarqueeAnimationSpeed; }
			set {
				_progressBarMarqueeAnimationSpeed = value;
				UpdateProgressBarMarqueeSpeed();
			}
		}

		/// <summary>
		/// Gets or sets the lower bound of the range of the task dialog's progress bar.
		/// </summary>
		/// <value>
		/// The lower bound of the range of the task dialog's progress bar. The default value is 0.
		/// </value>
		/// <remarks>
		/// This property is only used if the <see cref="ProgressBarStyle"/> property is
		/// <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2.ProgressBar"/>.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The new property value is not smaller than <see cref="ProgressBarMaximum"/>.</exception>
		[Category("Behavior"), Description("The lower bound of the range of the task dialog's progress bar."), DefaultValue(0)]
		public int ProgressBarMinimum {
			get { return _progressBarMinimimum; }
			set {
				if (_progressBarMaximum <= value)
					throw new ArgumentOutOfRangeException("value");
				_progressBarMinimimum = value;
				UpdateProgressBarRange();
			}
		}

		/// <summary>
		/// Gets or sets the upper bound of the range of the task dialog's progress bar.
		/// </summary>
		/// <value>
		/// The upper bound of the range of the task dialog's progress bar. The default value is 100.
		/// </value>
		/// <remarks>
		/// This property is only used if the <see cref="ProgressBarStyle"/> property is
		/// <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2.ProgressBar"/>.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The new property value is not larger than <see cref="ProgressBarMinimum"/>.</exception>
		[Category("Behavior"), Description("The upper bound of the range of the task dialog's progress bar."), DefaultValue(100)]
		public int ProgressBarMaximum {
			get { return _progressBarMaximum; }
			set {
				if (value <= _progressBarMinimimum)
					throw new ArgumentOutOfRangeException("value");
				_progressBarMaximum = value;
				UpdateProgressBarRange();
			}
		}

		/// <summary>
		/// Gets or sets the current value of the task dialog's progress bar.
		/// </summary>
		/// <value>
		/// The current value of the task dialog's progress bar. The default value is 0.
		/// </value>
		/// <remarks>
		/// This property is only used if the <see cref="ProgressBarStyle"/> property is
		/// <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2.ProgressBar"/>.
		/// <note>
		///   Updating the value of the progress bar while the dialog is visible  may only be done from
		///   the thread on which the task dialog was created.
		/// </note>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The new property value is smaller than <see cref="ProgressBarMinimum"/> or larger than <see cref="ProgressBarMaximum"/>.</exception>
		[Category("Behavior"), Description("The current value of the task dialog's progress bar."), DefaultValue(0)]
		public int ProgressBarValue {
			get { return _progressBarValue; }
			set {
				if (value < ProgressBarMinimum || value > ProgressBarMaximum)
					throw new ArgumentOutOfRangeException("value");

				_progressBarValue = value;
				UpdateProgressBarValue();
			}
		}

		/// <summary>
		/// Gets or sets the state of the task dialog's progress bar.
		/// </summary>
		/// <value>
		/// A <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarState"/> indicating the state of the task dialog's progress bar.
		/// The default value is <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarState.Normal"/>.
		/// </value>
		/// <remarks>
		/// This property is only used if the <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2"/> property is
		/// <see cref="Otomad.VegasScripts.OtomadHelper.V4.ProgressBarStyle2.ProgressBar"/>.
		/// </remarks>
		[Category("Behavior"), Description("The state of the task dialog's progress bar."), DefaultValue(ProgressBarState.Normal)]
		public ProgressBarState ProgressBarState {
			get { return _progressBarState; }
			set {
				_progressBarState = value;
				UpdateProgressBarState();
			}
		}

		/// <summary>
		/// Gets or sets an object that contains data about the dialog.
		/// </summary>
		/// <value>
		/// An object that contains data about the dialog. The default value is <see langword="null" />.
		/// </value>
		/// <remarks>
		/// Use this property to store arbitrary information about the dialog.
		/// </remarks>
		[Category("Data"), Description("User-defined data about the component."), DefaultValue(null)]
		public object Tag { get; set; }
		#endregion

		#region Public methods

		/// <summary>
		/// Shows the task dialog as a modeless dialog.
		/// </summary>
		/// <returns>The button that the user clicked. Can be <see langword="null" /> if the user cancelled the dialog using the
		/// title bar close button.</returns>
		/// <remarks>
		/// <note>
		///   Although the dialog is modeless, this method does not return until the task dialog is closed.
		/// </note>
		/// </remarks>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///   One of the properties or a combination of properties is not valid.
		/// </para>
		/// <para>
		///   -or-
		/// </para>
		/// <para>
		///   The dialog is already running.
		/// </para>
		/// </exception>
		/// <exception cref="NotSupportedException">Task dialogs are not supported on the current operating system.</exception>
		public TaskDialogButton Show() {
			return ShowDialog(IntPtr.Zero);
		}

		/// <summary>
		/// Shows the task dialog as a modal dialog.
		/// </summary>
		/// <returns>The button that the user clicked. Can be <see langword="null" /> if the user cancelled the dialog using the
		/// title bar close button.</returns>
		/// <remarks>
		/// The dialog will use the active window as its owner. If the current process has no active window,
		/// the dialog will be displayed as a modeless dialog (identical to calling <see cref="Show"/>).
		/// </remarks>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///   One of the properties or a combination of properties is not valid.
		/// </para>
		/// <para>
		///   -or-
		/// </para>
		/// <para>
		///   The dialog is already running.
		/// </para>
		/// </exception>
		/// <exception cref="NotSupportedException">Task dialogs are not supported on the current operating system.</exception>
		public TaskDialogButton ShowDialog() {
			return ShowDialog((IWin32Window)null);
		}

		/// <summary>
		/// This method is for internal Otomad.VegasScripts.OtomadHelper.V4 use and should not be called from your code.
		/// </summary>
		/// <param name="owner"></param>
		/// <returns></returns>
		public TaskDialogButton ShowDialog(IWin32Window owner) {
			IntPtr ownerHandle;
			if (owner == null)
				ownerHandle = GetActiveWindow();
			else
				ownerHandle = owner.Handle;
			return ShowDialog(ownerHandle);
		}

		/// <summary>
		/// Shows the task dialog as a modal dialog.
		/// </summary>
		/// <param name="owner">The <see cref="IntPtr"/> Win32 handle that is the owner of this task dialog.</param>
		/// <returns>The button that the user clicked. Can be <see langword="null" /> if the user cancelled the dialog using the
		/// title bar close button.</returns>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///   One of the properties or a combination of properties is not valid.
		/// </para>
		/// <para>
		///   -or-
		/// </para>
		/// <para>
		///   The dialog is already running.
		/// </para>
		/// </exception>
		/// <exception cref="NotSupportedException">Task dialogs are not supported on the current operating system.</exception>
		/// <exception cref="InvalidOperationException">Thrown if task dialog is already being displayed.</exception>
		/// <exception cref="InvalidOperationException">Thrown if no buttons are present.</exception>
		public TaskDialogButton ShowDialog(IntPtr owner) {
			if (!OSSupportsTaskDialogs)
				throw new NotSupportedException("The operating system does not support task dialogs.");

			if (IsDialogRunning)
				throw new InvalidOperationException("The task dialog is already being displayed.");

			if (_buttons == null || _buttons.Count == 0)
				throw new InvalidOperationException("The task dialog must have buttons.");

			_config.hwndParent = owner;
			_config.dwCommonButtons = 0;
			_config.pButtons = IntPtr.Zero;
			_config.cButtons = 0;
			List<TASKDIALOG_BUTTON> buttons = SetupButtons();
			List<TASKDIALOG_BUTTON> radioButtons = SetupRadioButtons();

			SetupIcon();

			try {
				MarshalButtons(buttons, out _config.pButtons, out _config.cButtons);
				MarshalButtons(radioButtons, out _config.pRadioButtons, out _config.cRadioButtons);
				int buttonId;
				int radioButton;
				bool verificationFlagChecked;
				using (new ComCtlv6ActivationContext(true)) {
					TaskDialogIndirect(ref _config, out buttonId, out radioButton, out verificationFlagChecked);
				}
				IsVerificationChecked = verificationFlagChecked;

				TaskDialogRadioButton selectedRadioButton;
				if (_radioButtonsById.TryGetValue(radioButton, out selectedRadioButton))
					selectedRadioButton.Checked = true;

				TaskDialogButton selectedButton;
				if (_buttonsById.TryGetValue(buttonId, out selectedButton))
					return selectedButton;
				else
					return null;
			} finally {
				CleanUpButtons(ref _config.pButtons, ref _config.cButtons);
				CleanUpButtons(ref _config.pRadioButtons, ref _config.cRadioButtons);
			}
		}

		/// <summary>
		/// Simulates a click on the verification checkbox of the <see cref="TaskDialog"/>, if it exists.
		/// </summary>
		/// <param name="checkState"><see langword="true" /> to set the state of the checkbox to be checked; <see langword="false" /> to set it to be unchecked.</param>
		/// <param name="setFocus"><see langword="true" /> to set the keyboard focus to the checkbox; otherwise <see langword="false" />.</param>
		/// <exception cref="InvalidOperationException">The task dialog is not being displayed.</exception>
		public void ClickVerification(bool checkState, bool setFocus) {
			if (!IsDialogRunning)
				throw new InvalidOperationException("The task dialog is not current displayed.");

			SendMessage(Handle, (int)TaskDialogMessages.ClickVerification, new IntPtr(checkState ? 1 : 0), new IntPtr(setFocus ? 1 : 0));
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Raises the <see cref="HyperlinkClicked"/> event.
		/// </summary>
		/// <param name="e">The <see cref="HyperlinkClickedEventArgs"/> containing the data for the event.</param>
		protected virtual void OnHyperlinkClicked(HyperlinkClickedEventArgs e) {
			if (HyperlinkClicked != null)
				HyperlinkClicked(this, e);
		}

		/// <summary>
		/// Raises the <see cref="ButtonClicked"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TaskDialogItemClickedEventArgs"/> containing the data for the event.</param>
		protected virtual void OnButtonClicked(TaskDialogItemClickedEventArgs e) {
			if (ButtonClicked != null)
				ButtonClicked(this, e);
		}

		/// <summary>
		/// Raises the <see cref="RadioButtonClicked"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TaskDialogItemClickedEventArgs"/> containing the data for the event.</param>
		protected virtual void OnRadioButtonClicked(TaskDialogItemClickedEventArgs e) {
			if (RadioButtonClicked != null)
				RadioButtonClicked(this, e);
		}

		/// <summary>
		/// Raises the <see cref="VerificationClicked"/> event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> containing the data for the event.</param>
		protected virtual void OnVerificationClicked(EventArgs e) {
			if (VerificationClicked != null)
				VerificationClicked(this, e);
		}

		/// <summary>
		/// Raises the <see cref="Created"/> event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> containing the data for the event.</param>
		protected virtual void OnCreated(EventArgs e) {
			if (Created != null)
				Created(this, e);
		}

		/// <summary>
		/// Raises the <see cref="Timer"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TimerEventArgs"/> containing the data for the event.</param>
		protected virtual void OnTimer(TimerEventArgs e) {
			if (Timer != null)
				Timer(this, e);
		}

		/// <summary>
		/// Raises the <see cref="Destroyed"/> event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> containing the data for the event.</param>
		protected virtual void OnDestroyed(EventArgs e) {
			if (Destroyed != null)
				Destroyed(this, e);
		}

		/// <summary>
		/// Raises the <see cref="ExpandButtonClicked"/> event.
		/// </summary>
		/// <param name="e">The <see cref="ExpandButtonClickedEventArgs"/> containing the data for the event.</param>
		protected virtual void OnExpandButtonClicked(ExpandButtonClickedEventArgs e) {
			if (ExpandButtonClicked != null)
				ExpandButtonClicked(this, e);
		}

		/// <summary>
		/// Raises the <see cref="HelpRequested"/> event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> containing the data for the event.</param>
		protected virtual void OnHelpRequested(EventArgs e) {
			if (HelpRequested != null)
				HelpRequested(this, e);
		}

		#endregion

		#region Internal Members

		internal void SetItemEnabled(TaskDialogItem item) {
			if (IsDialogRunning) {
				SendMessage(Handle, (int)(item is TaskDialogButton ? TaskDialogMessages.EnableButton : TaskDialogMessages.EnableRadioButton), new IntPtr(item.Id), new IntPtr(item.Enabled ? 1 : 0));
			}
		}

		internal void SetButtonElevationRequired(TaskDialogButton button) {
			if (IsDialogRunning) {
				SendMessage(Handle, (int)TaskDialogMessages.SetButtonElevationRequiredState, new IntPtr(button.Id), new IntPtr(button.ElevationRequired ? 1 : 0));
			}
		}

		internal void ClickItem(TaskDialogItem item) {
			if (!IsDialogRunning)
				throw new InvalidOperationException("The task dialog is not current displayed.");

			SendMessage(Handle, (int)(item is TaskDialogButton ? TaskDialogMessages.ClickButton : TaskDialogMessages.ClickRadioButton), new IntPtr(item.Id), IntPtr.Zero);
		}

		#endregion

		#region Private members

		internal void UpdateDialog() {
			if (IsDialogRunning) {
				// If the navigate page message is sent from within the callback, the navigation won't
				// take place until the callback returns. Any further messages sent after the navigate
				// page message before the end of the callback will then be lost as the navigation occurs.
				// For that reason, we defer it all the way until the end.
				if (_inEventHandler > 0)
					_updatePending = true;
				else {
					_updatePending = false;
					CleanUpButtons(ref _config.pButtons, ref _config.cButtons);
					CleanUpButtons(ref _config.pRadioButtons, ref _config.cRadioButtons);
					_config.dwCommonButtons = 0;

					List<TASKDIALOG_BUTTON> buttons = SetupButtons();
					List<TASKDIALOG_BUTTON> radioButtons = SetupRadioButtons();

					SetupIcon();

					MarshalButtons(buttons, out _config.pButtons, out _config.cButtons);
					MarshalButtons(radioButtons, out _config.pRadioButtons, out _config.cRadioButtons);

					int size = Marshal.SizeOf(_config);
					IntPtr memory = Marshal.AllocHGlobal(size);
					try {
						Marshal.StructureToPtr(_config, memory, false);
						SendMessage(Handle, (int)TaskDialogMessages.NavigatePage, IntPtr.Zero, memory);
					} finally {
						Marshal.DestroyStructure(memory, typeof(TASKDIALOGCONFIG));
						Marshal.FreeHGlobal(memory);
					}
				}
			}
		}

		private bool IsDialogRunning {
			get {
				// Intentially not using the Handle property, since the cross-thread call check should not be performed here.
				return _handle != IntPtr.Zero;
			}
		}

		private void SetElementText(TaskDialogElements element, string text) {
			if (IsDialogRunning) {
				IntPtr newTextPtr = Marshal.StringToHGlobalUni(text);
				try {
					IntPtr result = SendMessage(Handle, (int)TaskDialogMessages.SetElementText, new IntPtr((int)element), newTextPtr);
				} finally {
					if (newTextPtr != IntPtr.Zero)
						Marshal.FreeHGlobal(newTextPtr);
				}
			}
		}

		private void SetupIcon() {
			SetupIcon(MainIcon, CustomMainIcon, TaskDialogFlags.UseHIconMain);
			SetupIcon(FooterIcon, CustomFooterIcon, TaskDialogFlags.UseHIconFooter);
		}

		private void SetupIcon(TaskDialogIcon icon, System.Drawing.Icon customIcon, TaskDialogFlags flag) {
			SetFlag(flag, false);
			if (icon == TaskDialogIcon.Custom) {
				if (customIcon != null) {
					SetFlag(flag, true);
					if (flag == TaskDialogFlags.UseHIconMain)
						_config.hMainIcon = customIcon.Handle;
					else
						_config.hFooterIcon = customIcon.Handle;
				}
			} else {
				if (flag == TaskDialogFlags.UseHIconMain)
					_config.hMainIcon = new IntPtr((int)icon);
				else
					_config.hFooterIcon = new IntPtr((int)icon);
			}
		}

		private static void CleanUpButtons(ref IntPtr buttons, ref uint count) {
			if (buttons != IntPtr.Zero) {
				int elementSize = Marshal.SizeOf(typeof(TASKDIALOG_BUTTON));
				for (int x = 0; x < count; ++x) {
					// This'll be safe until they introduce 128 bit machines. :)
					// It's the only way to do it without unsafe code.
					IntPtr offset = new IntPtr(buttons.ToInt64() + x * elementSize);
					Marshal.DestroyStructure(offset, typeof(TASKDIALOG_BUTTON));
				}
				Marshal.FreeHGlobal(buttons);
				buttons = IntPtr.Zero;
				count = 0;
			}
		}

		private static void MarshalButtons(List<TASKDIALOG_BUTTON> buttons, out IntPtr buttonsPtr, out uint count) {
			buttonsPtr = IntPtr.Zero;
			count = 0;
			if (buttons.Count > 0) {
				int elementSize = Marshal.SizeOf(typeof(TASKDIALOG_BUTTON));
				buttonsPtr = Marshal.AllocHGlobal(elementSize * buttons.Count);
				for (int x = 0; x < buttons.Count; ++x) {
					// This'll be safe until they introduce 128 bit machines. :)
					// It's the only way to do it without unsafe code.
					IntPtr offset = new IntPtr(buttonsPtr.ToInt64() + x * elementSize);
					Marshal.StructureToPtr(buttons[x], offset, false);
				}
				count = (uint)buttons.Count;
			}
		}

		private List<TASKDIALOG_BUTTON> SetupButtons() {
			_buttonsById = new Dictionary<int, TaskDialogButton>();
			List<TASKDIALOG_BUTTON> buttons = new List<TASKDIALOG_BUTTON>();
			_config.nDefaultButton = 0;
			foreach (TaskDialogButton button in Buttons) {
				if (button.Id < 1)
					throw new InvalidOperationException("The id of a task dialog item must be higher than 0.");
				_buttonsById.Add(button.Id, button);
				if (button.Default)
					_config.nDefaultButton = button.Id;
				if (button.ButtonType == ButtonType.Custom) {
					if (string.IsNullOrEmpty(button.Text))
						throw new InvalidOperationException("A custom button or radio button cannot have an empty label.");
					TASKDIALOG_BUTTON taskDialogButton = new TASKDIALOG_BUTTON {
						nButtonID = button.Id,
						pszButtonText = button.Text
					};
					if (ButtonStyle == TaskDialogButtonStyle.CommandLinks || ButtonStyle == TaskDialogButtonStyle.CommandLinksNoIcon && !string.IsNullOrEmpty(button.CommandLinkNote))
						taskDialogButton.pszButtonText += "\n" + button.CommandLinkNote;
					buttons.Add(taskDialogButton);
				} else {
					_config.dwCommonButtons |= button.ButtonFlag;
				}
			}
			return buttons;
		}

		private List<TASKDIALOG_BUTTON> SetupRadioButtons() {
			_radioButtonsById = new Dictionary<int, TaskDialogRadioButton>();
			List<TASKDIALOG_BUTTON> radioButtons = new List<TASKDIALOG_BUTTON>();
			_config.nDefaultRadioButton = 0;
			foreach (TaskDialogRadioButton radioButton in RadioButtons) {
				if (string.IsNullOrEmpty(radioButton.Text))
					throw new InvalidOperationException("A custom button or radio button cannot have an empty label.");
				if (radioButton.Id < 1)
					throw new InvalidOperationException("The id of a task dialog item must be higher than 0.");
				_radioButtonsById.Add(radioButton.Id, radioButton);
				if (radioButton.Checked)
					_config.nDefaultRadioButton = radioButton.Id;
				TASKDIALOG_BUTTON taskDialogButton = new TASKDIALOG_BUTTON {
					nButtonID = radioButton.Id,
					pszButtonText = radioButton.Text
				};
				radioButtons.Add(taskDialogButton);
			}
			SetFlag(TaskDialogFlags.NoDefaultRadioButton, _config.nDefaultRadioButton == 0);
			return radioButtons;
		}

		private void SetFlag(TaskDialogFlags flag, bool value) {
			if (value)
				_config.dwFlags |= flag;
			else
				_config.dwFlags &= ~flag;
		}

		private bool GetFlag(TaskDialogFlags flag) {
			return (_config.dwFlags & flag) != 0;
		}

		private uint TaskDialogCallback(IntPtr hwnd, uint uNotification, IntPtr wParam, IntPtr lParam, IntPtr dwRefData) {
			Interlocked.Increment(ref _inEventHandler);
			try {
				switch ((TaskDialogNotifications)uNotification) {
					case TaskDialogNotifications.Created:
						_handle = hwnd;
						DialogCreated();
						OnCreated(EventArgs.Empty);
						break;
					case TaskDialogNotifications.Destroyed:
						_handle = IntPtr.Zero;
						OnDestroyed(EventArgs.Empty);
						break;
					case TaskDialogNotifications.Navigated:
						DialogCreated();
						break;
					case TaskDialogNotifications.HyperlinkClicked:
						string url = Marshal.PtrToStringUni(lParam);
						OnHyperlinkClicked(new HyperlinkClickedEventArgs(url));
						break;
					case TaskDialogNotifications.ButtonClicked:
						TaskDialogButton button;
						if (_buttonsById.TryGetValue((int)wParam, out button)) {
							TaskDialogItemClickedEventArgs e = new TaskDialogItemClickedEventArgs(button);
							OnButtonClicked(e);
							if (e.Cancel)
								return 1;
						}
						break;
					case TaskDialogNotifications.VerificationClicked:
						IsVerificationChecked = ((int)wParam) == 1;
						OnVerificationClicked(EventArgs.Empty);
						break;
					case TaskDialogNotifications.RadioButtonClicked:
						TaskDialogRadioButton radioButton;
						if (_radioButtonsById.TryGetValue((int)wParam, out radioButton)) {
							radioButton.Checked = true; // there's no way to click a radio button without checking it, is there?
							TaskDialogItemClickedEventArgs e = new TaskDialogItemClickedEventArgs(radioButton);
							OnRadioButtonClicked(e);
						}
						break;
					case TaskDialogNotifications.Timer:
						TimerEventArgs timerEventArgs = new TimerEventArgs(wParam.ToInt32());
						OnTimer(timerEventArgs);
						return (uint)(timerEventArgs.ResetTickCount ? 1 : 0);
					case TaskDialogNotifications.ExpandoButtonClicked:
						OnExpandButtonClicked(new ExpandButtonClickedEventArgs(wParam.ToInt32() != 0));
						break;
					case TaskDialogNotifications.Help:
						OnHelpRequested(EventArgs.Empty);
						break;
				}
				return 0;
			} finally {
				Interlocked.Decrement(ref _inEventHandler);
				if (_updatePending)
					UpdateDialog();
			}
		}

		private void DialogCreated() {
			if (_config.hwndParent == IntPtr.Zero && _windowIcon != null) {
				const int WM_SETICON = 0x0080, ICON_SMALL = 0;
				SendMessage(Handle, WM_SETICON, new IntPtr(ICON_SMALL), _windowIcon.Handle);
			}

			foreach (TaskDialogButton button in Buttons) {
				if (!button.Enabled)
					SetItemEnabled(button);
				if (button.ElevationRequired)
					SetButtonElevationRequired(button);
			}
			UpdateProgressBarStyle();
			UpdateProgressBarMarqueeSpeed();
			UpdateProgressBarRange();
			UpdateProgressBarValue();
			UpdateProgressBarState();
		}

		private void UpdateProgressBarStyle() {
			if (IsDialogRunning) {
				SendMessage(Handle, (int)TaskDialogMessages.SetMarqueeProgressBar, new IntPtr(ProgressBarStyle == ProgressBarStyle2.MarqueeProgressBar ? 1 : 0), IntPtr.Zero);
			}
		}

		private void UpdateProgressBarMarqueeSpeed() {
			if (IsDialogRunning) {
				SendMessage(Handle, (int)TaskDialogMessages.SetProgressBarMarquee, new IntPtr(ProgressBarMarqueeAnimationSpeed > 0 ? 1 : 0), new IntPtr(ProgressBarMarqueeAnimationSpeed));
			}
		}

		private void UpdateProgressBarRange() {
			if (IsDialogRunning) {
				SendMessage(Handle, (int)TaskDialogMessages.SetProgressBarRange, IntPtr.Zero, new IntPtr(ProgressBarMaximum << 16 | ProgressBarMinimum));
			}
			if (ProgressBarValue < ProgressBarMinimum)
				ProgressBarValue = ProgressBarMinimum;
			if (ProgressBarValue > ProgressBarMaximum)
				ProgressBarValue = ProgressBarMaximum;
		}

		private void UpdateProgressBarValue() {
			if (IsDialogRunning) {
				SendMessage(Handle, (int)TaskDialogMessages.SetProgressBarPos, new IntPtr(ProgressBarValue), IntPtr.Zero);
			}
		}

		private void UpdateProgressBarState() {
			if (IsDialogRunning) {
				SendMessage(Handle, (int)TaskDialogMessages.SetProgressBarState, new IntPtr((int)ProgressBarState + 1), IntPtr.Zero);
			}
		}

		private void CheckCrossThreadCall() {
			IntPtr handle = _handle;
			if (handle != IntPtr.Zero) {
				int processId;
				int windowThreadId = GetWindowThreadProcessId(handle, out processId);
				int threadId = GetCurrentThreadId();
				if (windowThreadId != threadId)
					throw new InvalidOperationException("Cross-thread operation not valid: Task dialog accessed from a thread other than the thread it was created on while it is visible.");
			}
		}

		#endregion

		#region IWin32Window Members

		/// <summary>
		/// Gets the window handle of the task dialog.
		/// </summary>
		/// <value>
		/// The window handle of the task dialog when it is being displayed, or <see cref="IntPtr.Zero"/> when the dialog
		/// is not being displayed.
		/// </value>
		[Browsable(false)]
		public IntPtr Handle {
			get {
				CheckCrossThreadCall();
				return _handle;
			}
		}

		#endregion

		#region Native Methods

		public static bool IsWindowsVistaOrLater {
			get {
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(6, 0, 6000);
			}
		}

		public static bool IsWindowsXPOrLater {
			get {
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 1, 2600);
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetActiveWindow();

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist"), DllImport("comctl32.dll", PreserveSig = false)]
		public static extern void TaskDialogIndirect([In] ref TASKDIALOGCONFIG pTaskConfig, out int pnButton, out int pnRadioButton, [MarshalAs(UnmanagedType.Bool)] out bool pfVerificationFlagChecked);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetCurrentThreadId();

		#region Activation Context

		[DllImport("Kernel32.dll", SetLastError = true)]
		public extern static ActivationContextSafeHandle CreateActCtx(ref ACTCTX actctx);
		[DllImport("kernel32.dll"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public extern static void ReleaseActCtx(IntPtr hActCtx);
		[DllImport("Kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public extern static bool ActivateActCtx(ActivationContextSafeHandle hActCtx, out IntPtr lpCookie);
		[DllImport("Kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public extern static bool DeactivateActCtx(uint dwFlags, IntPtr lpCookie);

		#endregion

		#endregion
	}

	/// <summary>
	/// Class that provides data for the <see cref="TaskDialog.HyperlinkClicked"/> event.
	/// </summary>
	/// <threadsafety instance="false" static="true" />
	public class HyperlinkClickedEventArgs : EventArgs {
		/// <summary>
		/// Creates a new instance of the <see cref="HyperlinkClickedEventArgs"/> class with the specified URL.
		/// </summary>
		/// <param name="href">The URL of the hyperlink.</param>
		public HyperlinkClickedEventArgs(string href) {
			Href = href;
		}

		/// <summary>
		/// Gets the URL of the hyperlink that was clicked.
		/// </summary>
		/// <value>
		/// The value of the href attribute of the hyperlink.
		/// </value>
		public string Href { get; private set; }
	}

	/// <summary>
	/// Indicates the icon to use for a task dialog.
	/// </summary>
	public enum TaskDialogIcon {
		/// <summary>
		/// A custom icon or no icon if no custom icon is specified.
		/// </summary>
		Custom,
		/// <summary>
		/// System warning icon.
		/// </summary>
		Warning = 0xFFFF, // MAKEINTRESOURCEW(-1)
		/// <summary>
		/// System Error icon.
		/// </summary>
		Error = 0xFFFE, // MAKEINTRESOURCEW(-2)
		/// <summary>
		/// System Information icon.
		/// </summary>
		Information = 0xFFFD, // MAKEINTRESOURCEW(-3)
		/// <summary>
		/// Shield icon.
		/// </summary>
		Shield = 0xFFFC, // MAKEINTRESOURCEW(-4)
	}

	/// <summary>
	/// Provides data for the <see cref="TaskDialog.Timer"/> event.
	/// </summary>
	/// <threadsafety instance="false" static="true" />
	public class TimerEventArgs : EventArgs {
		/// <summary>
		/// Initializes a new instance of the <see cref="TimerEventArgs"/> class with the specified tick count.
		/// </summary>
		/// <param name="tickCount">The tick count.</param>
		public TimerEventArgs(int tickCount) {
			TickCount = tickCount;
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the tick count should be reset.
		/// </summary>
		/// <value>
		/// <see langword="true" /> to reset the tick count after the event handler returns; otherwise, <see langword="false" />.
		/// The default value is <see langword="false" />.
		/// </value>
		public bool ResetTickCount { get; set; }

		/// <summary>
		/// Gets the current tick count of the timer.
		/// </summary>
		/// <value>
		/// The number of milliseconds that has elapsed since the dialog was created or since the last time the event handler returned
		/// with the <see cref="ResetTickCount"/> property set to <see langword="true" />.
		/// </value>
		public int TickCount { get; private set; }
	}

	/// <summary>
	/// Represents the state of the progress bar on the task dialog.
	/// </summary>
	public enum ProgressBarState {
		/// <summary>
		/// Normal state.
		/// </summary>
		Normal,
		/// <summary>
		/// Error state
		/// </summary>
		Error,
		/// <summary>
		/// Paused state
		/// </summary>
		Paused
	}

	/// <summary>
	/// Provides data for the <see cref="TaskDialog.ExpandButtonClicked"/> event.
	/// </summary>
	/// <threadsafety instance="false" static="true" />
	public class ExpandButtonClickedEventArgs : EventArgs {
		/// <summary>
		/// Initializes a new instance of the <see cref="ExpandButtonClickedEventArgs"/> class with the specified expanded state.
		/// </summary>
		/// <param name="expanded"><see langword="true" /> if the the expanded content on the dialog is shown; otherwise, <see langword="false" />.</param>
		public ExpandButtonClickedEventArgs(bool expanded) {
			Expanded = expanded;
		}

		/// <summary>
		/// Gets a value that indicates if the expanded content on the dialog is shown.
		/// </summary>
		/// <value><see langword="true" /> if the expanded content on the dialog is shown; otherwise, <see langword="false" />.</value>
		public bool Expanded { get; private set; }
	}

	sealed class ComCtlv6ActivationContext : IDisposable {
		// Private data
		private IntPtr _cookie;
		private static ACTCTX _enableThemingActivationContext;
		private static ActivationContextSafeHandle _activationContext;
		private static bool _contextCreationSucceeded;
		private static readonly object _contextCreationLock = new object();

		public ComCtlv6ActivationContext(bool enable) {
			if (enable && TaskDialog.IsWindowsXPOrLater) {
				if (EnsureActivateContextCreated()) {
					if (!TaskDialog.ActivateActCtx(_activationContext, out _cookie)) {
						// Be sure cookie always zero if activation failed
						_cookie = IntPtr.Zero;
					}
				}
			}
		}

		~ComCtlv6ActivationContext() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060")]
		private void Dispose(bool _disposing) {
			if (_cookie != IntPtr.Zero) {
				if (TaskDialog.DeactivateActCtx(0, _cookie)) {
					// deactivation succeeded...
					_cookie = IntPtr.Zero;
				}
			}
		}

		private const string xpThemesMenifest = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?><assembly xmlns=\"urn:schemas-microsoft-com:asm.v1\" manifestVersion=\"1.0\"><description>Windows Forms Common Control manifest</description><dependency><dependentAssembly><assemblyIdentity type=\"win32\" name=\"Microsoft.Windows.Common-Controls\" version=\"6.0.0.0\" processorArchitecture=\"*\" publicKeyToken=\"6595b64144ccf1df\" language=\"*\" /></dependentAssembly></dependency></assembly>";

		private static bool EnsureActivateContextCreated() {
			lock (_contextCreationLock) {
				if (_contextCreationSucceeded) {
					return _contextCreationSucceeded;
				}

				string manifestTempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());

				File.WriteAllText(manifestTempFilePath, xpThemesMenifest);

				_enableThemingActivationContext = new ACTCTX {
					cbSize = Marshal.SizeOf(typeof(ACTCTX)),
					lpSource = manifestTempFilePath,
				};

				// Note this will fail gracefully if file specified
				// by manifestFilePath doesn't exist.
				_activationContext = TaskDialog.CreateActCtx(ref _enableThemingActivationContext);
				_contextCreationSucceeded = !_activationContext.IsInvalid;

				try {
					File.Delete(manifestTempFilePath);
				} catch (Exception) {
					// We tried to be tidy but something blocked us :(
				}

				// If we return false, we'll try again on the next call into
				// EnsureActivateContextCreated(), which is fine.
				return _contextCreationSucceeded;
			}
		}
	}

	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	public class ActivationContextSafeHandle : SafeHandleZeroOrMinusOneIsInvalid {
		public ActivationContextSafeHandle()
			: base(true) {
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle() {
			TaskDialog.ReleaseActCtx(handle);
			return true;
		}
	}

	/// <summary>
	/// Indicates the type of progress on a task dialog.
	/// </summary>
	public enum ProgressBarStyle2 {
		/// <summary>
		/// No progress bar is displayed on the dialog.
		/// </summary>
		None,
		/// <summary>
		/// A regular progress bar is displayed on the dialog.
		/// </summary>
		ProgressBar,
		/// <summary>
		/// A marquee progress bar is displayed on the dialog. Use this value for operations
		/// that cannot report concrete progress information.
		/// </summary>
		MarqueeProgressBar
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct TASKDIALOG_BUTTON {
		public int nButtonID;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszButtonText;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable"), StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct TASKDIALOGCONFIG {
		public uint cbSize;
		public IntPtr hwndParent;
		public IntPtr hInstance;
		public TaskDialogFlags dwFlags;
		public TaskDialogCommonButtonFlags dwCommonButtons;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszWindowTitle;
		public IntPtr hMainIcon;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszMainInstruction;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszContent;
		public uint cButtons;
		//[MarshalAs(UnmanagedType.LPArray)]
		public IntPtr pButtons;
		public int nDefaultButton;
		public uint cRadioButtons;
		//[MarshalAs(UnmanagedType.LPArray)]
		public IntPtr pRadioButtons;
		public int nDefaultRadioButton;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszVerificationText;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszExpandedInformation;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszExpandedControlText;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszCollapsedControlText;
		public IntPtr hFooterIcon;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszFooterText;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public TaskDialogCallback pfCallback;
		public IntPtr lpCallbackData;
		public uint cxWidth;
	}

	[Flags]
	public enum TaskDialogFlags {
		EnableHyperLinks = 0x0001,
		UseHIconMain = 0x0002,
		UseHIconFooter = 0x0004,
		AllowDialogCancellation = 0x0008,
		UseCommandLinks = 0x0010,
		UseCommandLinksNoIcon = 0x0020,
		ExpandFooterArea = 0x0040,
		ExpandedByDefault = 0x0080,
		VerificationFlagChecked = 0x0100,
		ShowProgressBar = 0x0200,
		ShowMarqueeProgressBar = 0x0400,
		CallbackTimer = 0x0800,
		PositionRelativeToWindow = 0x1000,
		RtlLayout = 0x2000,
		NoDefaultRadioButton = 0x4000,
		CanBeMinimized = 0x8000
	}

	public delegate uint TaskDialogCallback(IntPtr hwnd, uint uNotification, IntPtr wParam, IntPtr lParam, IntPtr dwRefData);

	public enum TaskDialogNotifications {
		Created = 0,
		Navigated = 1,
		ButtonClicked = 2,            // wParam = Button ID
		HyperlinkClicked = 3,            // lParam = (LPCWSTR)pszHREF
		Timer = 4,            // wParam = Milliseconds since dialog created or timer reset
		Destroyed = 5,
		RadioButtonClicked = 6,            // wParam = Radio Button ID
		DialogConstructed = 7,
		VerificationClicked = 8,             // wParam = 1 if checkbox checked, 0 if not, lParam is unused and always 0
		Help = 9,
		ExpandoButtonClicked = 10            // wParam = 0 (dialog is now collapsed), wParam != 0 (dialog is now expanded)
	}

	public enum TaskDialogElements {
		Content,
		ExpandedInformation,
		Footer,
		MainInstruction
	}

	public enum TaskDialogMessages {
		WM_USER = 0x400,
		NavigatePage = WM_USER + 101,
		ClickButton = WM_USER + 102, // wParam = Button ID
		SetMarqueeProgressBar = WM_USER + 103, // wParam = 0 (nonMarque) wParam != 0 (Marquee)
		SetProgressBarState = WM_USER + 104, // wParam = new progress state
		SetProgressBarRange = WM_USER + 105, // lParam = MAKELPARAM(nMinRange, nMaxRange)
		SetProgressBarPos = WM_USER + 106, // wParam = new position
		SetProgressBarMarquee = WM_USER + 107, // wParam = 0 (stop marquee), wParam != 0 (start marquee), lparam = speed (milliseconds between repaints)
		SetElementText = WM_USER + 108, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
		ClickRadioButton = WM_USER + 110, // wParam = Radio Button ID
		EnableButton = WM_USER + 111, // lParam = 0 (disable), lParam != 0 (enable), wParam = Button ID
		EnableRadioButton = WM_USER + 112, // lParam = 0 (disable), lParam != 0 (enable), wParam = Radio Button ID
		ClickVerification = WM_USER + 113, // wParam = 0 (unchecked), 1 (checked), lParam = 1 (set key focus)
		UpdateElementText = WM_USER + 114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
		SetButtonElevationRequiredState = WM_USER + 115, // wParam = Button ID, lParam = 0 (elevation not required), lParam != 0 (elevation required)
		UpdateIcon = WM_USER + 116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
	}

	public struct ACTCTX {
		public int cbSize;
		public uint dwFlags;
		public string lpSource;
		public ushort wProcessorArchitecture;
		public ushort wLangId;
		public string lpAssemblyDirectory;
		public string lpResourceName;
		public string lpApplicationName;
	}
}
