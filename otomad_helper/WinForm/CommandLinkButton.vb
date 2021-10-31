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
