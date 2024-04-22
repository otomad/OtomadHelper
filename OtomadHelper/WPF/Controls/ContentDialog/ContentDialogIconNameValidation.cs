using System.ComponentModel.DataAnnotations;

namespace OtomadHelper.WPF.Controls;

public sealed class ContentDialogIconNameValidation : ValidationAttribute {
	public override bool IsValid(object value) {
		if (value is not string iconName) return false;
		return ContentDialogIconNameToSymbolConverter.IsValidIconName(iconName);
	}
}
