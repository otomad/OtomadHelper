using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUICommuityBackdropType = WinUICommunity.Common.Helpers.BackdropType;

namespace OtomadHelper.UI;
public static class Extensions {
	public static WinUICommuityBackdropType ToWinUICommunityType(this BackdropType backdrop) {
		return backdrop == BackdropType.Acrylic ? WinUICommuityBackdropType.DesktopAcrylic :
			backdrop == BackdropType.Mica ? WinUICommuityBackdropType.Mica :
			backdrop == BackdropType.MicaAlt ? WinUICommuityBackdropType.MicaAlt :
			WinUICommuityBackdropType.DefaultColor;
	}
}
