using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Otomad.VegasScript.OtomadHelper.V4 {

	public class CommandLinkButton : Button {
		private bool _commandLink = false;
		private string _commandLinkNote = "";

		public CommandLinkButton() : base() {
			// 在基类上设置默认属性值以避免过时的警告
			base.FlatStyle = FlatStyle.System;
		}

		[Category("Appearance"), DefaultValue(false), Description("指定此按钮应使用命令链接样式。（仅适用于 Windows Vista 及更高版本。）")]
		public bool CommandLink {
			get {
				return _commandLink;
			}
			set {
				if (_commandLink != value) {
					_commandLink = value;
					UpdateCommandLink();
				}
			}
		}

		[Category("Appearance"), DefaultValue(""), Description("设置命令链接按钮的说明文字。（仅适用于 Windows Vista 及更高版本。）"),
			Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		public string CommandLinkNote {
			get {
				return _commandLinkNote;
			}
			set {
				if (_commandLinkNote != value) {
					_commandLinkNote = value;
					UpdateCommandLink();
				}
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), Obsolete("CommandLinkButton 控件不支持此属性。"), DefaultValue(typeof(FlatStyle), "System")]
		public new FlatStyle FlatStyle {
			// 将默认展开样式设置为“系统”，并隐藏此属性，
			// 因为如果不将其设置为“系统”，任何自定义属性都无法工作
			get {
				return base.FlatStyle;
			}
			set {
				base.FlatStyle = value;
			}
		}

		#region P/Invoke Stuff
		private const int BS_COMMANDLINK = 0xE;
		private const int BCM_SETNOTE = 0x1609;

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
		private extern static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

		internal void UpdateCommandLink() {
			RecreateHandle();
			SendMessage(Handle, BCM_SETNOTE, IntPtr.Zero, _commandLinkNote);
		}

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				if (CommandLink) cp.Style |= BS_COMMANDLINK;
				return cp;
			}
		}
		#endregion
	}
}

/* VB Source Code

Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class ButtonEx : Inherits Button

	Private _commandLink As Boolean
	Private _commandLinkNote As String

	Public Sub New() : MyBase.New()
		'Set default property values on the base class to avoid the Obsolete warning
		MyBase.FlatStyle = FlatStyle.System
	End Sub

	<Category("Appearance")> _
	<DefaultValue(False)> _
	<Description("Specifies this button should use the command link style. " & _
				 "(Only applies under Windows Vista and later.)")> _
	Public Property CommandLink As Boolean
		Get
			Return _commandLink
		End Get
		Set(ByVal value As Boolean)
			If _commandLink <> value Then
				_commandLink = value
				Me.UpdateCommandLink()
			End If
		End Set
	End Property

	<Category("Appearance")> _
	<DefaultValue("")> _
	<Description("Sets the description text for a command link button. " & _
				 "(Only applies under Windows Vista and later.)")> _
	Public Property CommandLinkNote As String
		Get
			Return _commandLinkNote
		End Get
		Set(value As String)
			If _commandLinkNote <> value Then
				_commandLinkNote = value
				Me.UpdateCommandLink()
			End If
		End Set
	End Property

	<Browsable(False)> <EditorBrowsable(EditorBrowsableState.Never)> _
	<DebuggerBrowsable(DebuggerBrowsableState.Never)> _
	<Obsolete("This property is not supported on the ButtonEx control.")> _
	<DefaultValue(GetType(FlatStyle), "System")> _
	Public Shadows Property FlatStyle As FlatStyle
		'Set the default flat style to "System", and hide this property because
		'none of the custom properties will work without it set to "System"
		Get
			Return MyBase.FlatStyle
		End Get
		Set(ByVal value As FlatStyle)
			MyBase.FlatStyle = value
		End Set
	End Property

#Region "P/Invoke Stuff"
	Private Const BS_COMMANDLINK As Integer = &HE
	Private Const BCM_SETNOTE As Integer = &H1609

	<DllImport("user32.dll", CharSet:=CharSet.Unicode, SetLastError:=False)> _
	Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, _
										<MarshalAs(UnmanagedType.LPWStr)> ByVal lParam As String) As IntPtr
	End Function

	Private Sub UpdateCommandLink()
		Me.RecreateHandle()
		SendMessage(Me.Handle, BCM_SETNOTE, IntPtr.Zero, _commandLinkNote)
	End Sub

	Protected Overrides ReadOnly Property CreateParams As CreateParams
		Get
			Dim cp As CreateParams = MyBase.CreateParams

			If Me.CommandLink Then
				cp.Style = cp.Style Or BS_COMMANDLINK
			End If

			Return cp
		End Get
	End Property
#End Region

End Class

 */