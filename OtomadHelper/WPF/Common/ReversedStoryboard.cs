using System.Windows;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Common;

[DependencyProperty<Storyboard>("BasedOn", OnChanged = "Update")]
[DependencyProperty<bool>("ReverseEasing", DefaultValue = false, OnChanged = "Update")]
[AttachedDependencyProperty<ReversedStoryboard, TriggerBase>("AutoReverse")]
public partial class ReversedStoryboard : Storyboard {
	static partial void OnAutoReverseChanged(TriggerBase trigger, ReversedStoryboard? value) {
		if (value is null) return;
		BeginStoryboard? enterBegin = trigger.EnterActions.FirstOrDefault(action => action is BeginStoryboard begin && begin.Storyboard is not null) as BeginStoryboard;
		BeginStoryboard? exitBegin = trigger.ExitActions.FirstOrDefault(action => action is BeginStoryboard begin && begin.Storyboard is not null) as BeginStoryboard;
		if ((enterBegin != null) == (exitBegin != null)) return; // If both or neither are defined, skip.
		BeginStoryboard begin = new();
		if (enterBegin != null) {
			begin.Storyboard = new ReversedStoryboard() { BasedOn = enterBegin.Storyboard, ReverseEasing = value.ReverseEasing };
			trigger.ExitActions.Add(begin);
		} else if (exitBegin != null) {
			begin.Storyboard = new ReversedStoryboard() { BasedOn = exitBegin.Storyboard, ReverseEasing = value.ReverseEasing };
			trigger.EnterActions.Add(begin);
		}
	}

	private void Update() {
		Children.Clear();
		if (BasedOn == null) return;
		foreach (Timeline originalTimeline in BasedOn.Children) {
			Timeline timeline = originalTimeline.Clone();
			switch (timeline) {
				case DoubleAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				#region messy
				case ByteAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case ColorAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case DecimalAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case Int16Animation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case Int32Animation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case Int64Animation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case PointAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case Point3DAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case QuaternionAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case RectAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case Rotation3DAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case SingleAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case SizeAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case ThicknessAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case VectorAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				case Vector3DAnimation ani:
					(ani.From, ani.To) = (ani.To, ani.From);
					ReverseEasingFunction(ani.EasingFunction);
					break;
				default:
					break;
				#endregion
			}
			Children.Add(timeline);
		}
	}

	private IEasingFunction ReverseEasingFunction(IEasingFunction originalEasing) {
		if (!ReverseEasing) return originalEasing;
		if (originalEasing is EasingFunctionBase easingFunction) {
			EasingFunctionBase easing = (EasingFunctionBase)easingFunction.Clone();
			easing.EasingMode = easing.EasingMode switch {
				EasingMode.EaseIn => EasingMode.EaseOut,
				EasingMode.EaseOut => EasingMode.EaseIn,
				_ => EasingMode.EaseInOut,
			};
			return easing;
		}
		return originalEasing;
	}
}
