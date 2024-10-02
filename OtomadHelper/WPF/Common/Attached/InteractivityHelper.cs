// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// <email>mailto:vlads@logoui.co.il</email>
// <created>16/00/11</created>
// <lastedit>16/00/11</lastedit>

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the 'Software'), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// ====================================================================================//

using System.Windows;

using Microsoft.Xaml.Behaviors;

using TriggerBase = Microsoft.Xaml.Behaviors.TriggerBase;
using TriggerCollection = Microsoft.Xaml.Behaviors.TriggerCollection;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// <see cref="FrameworkTemplate"/> for InteractivityElements instance.
/// </summary>
/// <remarks>
/// Subclassed for forward compatibility, perhaps one day <see cref="FrameworkTemplate"/>
/// will not be partially internal.
/// </remarks>
public class InteractivityTemplate : DataTemplate {

}

/// <summary>
/// Holder for interactivity entries
/// </summary>
[AttachedDependencyProperty<InteractivityTemplate>("Template")]
public partial class InteractivityItems : FrameworkElement {
	/// <summary>
	/// Storage for triggers.
	/// </summary>
	public new List<TriggerBase> Triggers { get; } = [];

	/// <summary>
	/// Storage for Behaviors.
	/// </summary>
	public List<Behavior> Behaviors { get; } = [];

	static partial void OnTemplateChanged(DependencyObject d, InteractivityTemplate? dt) {
#if !SILVERLIGHT
		dt!.Seal();
#endif
		InteractivityItems ih = (InteractivityItems)dt.LoadContent();
		BehaviorCollection bc = Interaction.GetBehaviors(d);
		TriggerCollection tc = Interaction.GetTriggers(d);

		foreach (Behavior behavior in ih.Behaviors)
			bc.Add(behavior);

		foreach (TriggerBase trigger in ih.Triggers)
			tc.Add(trigger);
	}
}
