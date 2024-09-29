using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public partial class RadioButtonResourceDictionary : CustomControlResourceDictionary {
	private bool IsPressed { get; set; } = false;
	private void UpdateStates_Hover(object sender, MouseEventArgs e) => UpdateStates((RadioButton)sender);
	private void UpdateStates_Press(object sender, MouseButtonEventArgs e) { IsPressed = true; UpdateStates((RadioButton)sender); }
	private void UpdateStates_Lift(object sender, MouseButtonEventArgs e) { IsPressed = false; UpdateStates((RadioButton)sender); }
	private void UpdateStates_Check(object sender, RoutedEventArgs e) => UpdateStates((RadioButton)sender);

	private void UpdateStates(RadioButton radioButton) {
		string @checked = radioButton.IsChecked == true ? "Checked" : "Unchecked";
		string common = IsPressed ? "Pressed" : radioButton.IsMouseOver ? "MouseOver" : "Normal";
		string state = @checked + common;
		VisualStateManager.GoToState(radioButton, state, true);
	}
}
