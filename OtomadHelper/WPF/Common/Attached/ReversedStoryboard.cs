using System.Windows;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Common;

[DependencyProperty<Storyboard>("BasedOn", OnChanged = "Update")]
[DependencyProperty<bool>("ReverseEasing", DefaultValue = false, OnChanged = "Update")]
[AttachedDependencyProperty<ReversedStoryboard, TriggerBase>("AutoReverseTriggerActions")]
public partial class ReversedStoryboard : Storyboard {
	static partial void OnAutoReverseTriggerActionsChanged(TriggerBase trigger, ReversedStoryboard? value) {
		if (value is null) return;
		BeginStoryboard? enterBegin = trigger.EnterActions.FirstOrDefault(action => action is BeginStoryboard begin && begin.Storyboard is not null) as BeginStoryboard;
		BeginStoryboard? exitBegin = trigger.ExitActions.FirstOrDefault(action => action is BeginStoryboard begin && begin.Storyboard is not null) as BeginStoryboard;
		if ((enterBegin != null) == (exitBegin != null)) return; // If both or neither are defined, skip.
		BeginStoryboard begin = new();
		if (enterBegin != null) {
			begin.Storyboard = new ReversedStoryboard { BasedOn = enterBegin.Storyboard, ReverseEasing = value.ReverseEasing };
			trigger.ExitActions.Add(begin);
		} else if (exitBegin != null) {
			begin.Storyboard = new ReversedStoryboard { BasedOn = exitBegin.Storyboard, ReverseEasing = value.ReverseEasing };
			trigger.EnterActions.Add(begin);
		}
	}

	private void Update() {
		Children.Clear();
		if (BasedOn == null) return;
		foreach (Timeline originalTimeline in BasedOn.Children) {
			Timeline timeline = originalTimeline.Clone();

			PropertyInfo? fromProperty = timeline.GetType().GetProperty("From");
			PropertyInfo? toProperty = timeline.GetType().GetProperty("To");
			PropertyInfo? easingProperty = timeline.GetType().GetProperty("EasingFunction");
			if (fromProperty is null || toProperty is null || fromProperty.PropertyType != toProperty.PropertyType) return;

			object temp = fromProperty.GetValue(timeline);
			fromProperty.SetValue(timeline, toProperty.GetValue(timeline));
			toProperty.SetValue(timeline, temp);

			if (ReverseEasing && easingProperty != null) {
				object unknownEasing = easingProperty.GetValue(timeline);
				if (unknownEasing is EasingFunctionBase easingFunction) {
					EasingFunctionBase easing = ReverseEasingFunction(easingFunction);
					easingProperty.SetValue(timeline, easing);
				}
			}

			Children.Add(timeline);
		}
	}

	private static EasingFunctionBase ReverseEasingFunction(EasingFunctionBase easingFunction) {
		EasingFunctionBase easing = (EasingFunctionBase)easingFunction.Clone();
		easing.EasingMode = easing.EasingMode switch {
			EasingMode.EaseIn => EasingMode.EaseOut,
			EasingMode.EaseOut => EasingMode.EaseIn,
			_ => EasingMode.EaseInOut,
		};
		return easing;
	}
}
