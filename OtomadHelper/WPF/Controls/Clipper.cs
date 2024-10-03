/// <summary>
/// <see href="https://stackoverflow.com/a/59376318/19553213" />
/// </summary>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

/// <remarks>
/// <example>
/// <code>
/// <![CDATA[
/// <c:Clipper Constraint="WidthAndHeight">
///     <Control c:Clipper.HeightFraction="0.5" c:Clipper.WidthFraction="0.5" />
/// </c:Clipper>
/// ]]>
/// </code>
/// </example>
/// </remarks>
[AttachedDependencyProperty<double, UIElement>("WidthFraction", DefaultValue = 1d, OnChanged = "OnClippingInvalidated", Validate = true)]
[AttachedDependencyProperty<double, UIElement>("HeightFraction", DefaultValue = 1d, OnChanged = "OnClippingInvalidated", Validate = true)]
[DependencyProperty<Brush>("Background", DefaultValueExpression = "System.Windows.Media.Brushes.Transparent", AffectsRender = true)]
[DependencyProperty<ClipperConstraintSource>("Constraint", DefaultValue = ClipperConstraintSource.WidthAndHeight, Validate = true)]
public sealed partial class Clipper : Decorator {
	//public static readonly DependencyProperty WidthFractionProperty = DependencyProperty.RegisterAttached("WidthFraction", typeof(double), typeof(Clipper), new(1d, OnClippingInvalidated), IsFraction);
	//public static readonly DependencyProperty HeightFractionProperty = DependencyProperty.RegisterAttached("HeightFraction", typeof(double), typeof(Clipper), new(1d, OnClippingInvalidated), IsFraction);
	//public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(Clipper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));
	//public static readonly DependencyProperty ConstraintProperty = DependencyProperty.Register("Constraint", typeof(ClipperConstraintSource), typeof(Clipper), new(ClipperConstraintSource.WidthAndHeight, OnClippingInvalidated), IsValidConstraintSource);

	private Size childSize;
	private DependencyPropertySubscriber? childVerticalAlignmentSubcriber;
	private DependencyPropertySubscriber? childHorizontalAlignmentSubscriber;

	public Clipper() {
		base.ClipToBounds = true;
	}

	//public Brush Background {
	//	get => (Brush)GetValue(BackgroundProperty);
	//	set => SetValue(BackgroundProperty, value);
	//}

	//public ClipperConstraintSource Constraint {
	//	get => (ClipperConstraintSource)GetValue(ConstraintProperty);
	//	set => SetValue(ConstraintProperty, value);
	//}

	[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
	public new bool ClipToBounds => true;

	//[AttachedPropertyBrowsableForChildren]
	//public static double GetWidthFraction(DependencyObject obj) => (double)obj.GetValue(WidthFractionProperty);

	//public static void SetWidthFraction(DependencyObject obj, double value) => obj.SetValue(WidthFractionProperty, value);

	//[AttachedPropertyBrowsableForChildren]
	//public static double GetHeightFraction(DependencyObject obj) => (double)obj.GetValue(HeightFractionProperty);

	//public static void SetHeightFraction(DependencyObject obj, double value) => obj.SetValue(HeightFractionProperty, value);

	protected override Size MeasureOverride(Size constraint) {
		if (Child is null) return Size.Empty;

		Child.Measure(Constraint switch {
			ClipperConstraintSource.WidthAndHeight => constraint,
			ClipperConstraintSource.Width => new(constraint.Width, double.PositiveInfinity),
			ClipperConstraintSource.Height => new(double.PositiveInfinity, constraint.Height),
			ClipperConstraintSource.Nothing => new(double.PositiveInfinity, double.PositiveInfinity),
			_ => throw new NotImplementedException(),
		});

		Size finalSize = Child.DesiredSize;
		if (Child is FrameworkElement childElement) {
			if (childElement.HorizontalAlignment == HorizontalAlignment.Stretch && constraint.Width > finalSize.Width && !double.IsInfinity(constraint.Width))
				finalSize.Width = constraint.Width;

			if (childElement.VerticalAlignment == VerticalAlignment.Stretch && constraint.Height > finalSize.Height && !double.IsInfinity(constraint.Height))
				finalSize.Height = constraint.Height;
		}

		childSize = finalSize;

		finalSize.Width *= GetWidthFraction(Child);
		finalSize.Height *= GetHeightFraction(Child);

		return finalSize;
	}

	protected override Size ArrangeOverride(Size arrangeSize) {
		if (Child is null) return Size.Empty;

		Size childSize = this.childSize;
		Size clipperSize = new(
			Math.Min(arrangeSize.Width, childSize.Width * GetWidthFraction(Child)),
			Math.Min(arrangeSize.Height, childSize.Height * GetHeightFraction(Child))
		);
		double offsetX = 0d;
		double offsetY = 0d;

		if (Child is FrameworkElement childElement) {
			if (childSize.Width > clipperSize.Width) {
				switch (childElement.HorizontalAlignment) {
					case HorizontalAlignment.Right:
						offsetX = -(childSize.Width - clipperSize.Width);
						break;

					case HorizontalAlignment.Center:
						offsetX = -(childSize.Width - clipperSize.Width) / 2;
						break;
				}
			}

			if (childSize.Height > clipperSize.Height) {
				switch (childElement.VerticalAlignment) {
					case VerticalAlignment.Bottom:
						offsetY = -(childSize.Height - clipperSize.Height);
						break;

					case VerticalAlignment.Center:
						offsetY = -(childSize.Height - clipperSize.Height) / 2;
						break;
				}
			}
		}

		Child.Arrange(new(new(offsetX, offsetY), childSize));

		return clipperSize;
	}

	protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved) {
		void UpdateLayout(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			if (e.NewValue is HorizontalAlignment.Stretch or VerticalAlignment.Stretch)
				InvalidateMeasure();
			else
				InvalidateArrange();
		}

		childHorizontalAlignmentSubscriber?.Unsubscribe();
		childVerticalAlignmentSubcriber?.Unsubscribe();

		if (visualAdded is FrameworkElement childElement) {
			childHorizontalAlignmentSubscriber = new(childElement, HorizontalAlignmentProperty, UpdateLayout);
			childVerticalAlignmentSubcriber = new(childElement, VerticalAlignmentProperty, UpdateLayout);
		}
	}

	protected override void OnRender(DrawingContext drawingContext) {
		base.OnRender(drawingContext);
		drawingContext.DrawRectangle(Background, null, new(RenderSize));
	}

	private static partial bool IsWidthFractionValid(double value) => IsFraction(value);
	private static partial bool IsHeightFractionValid(double value) => IsFraction(value);
	private static bool IsFraction(double numericValue) => numericValue is >= 0d and <= 1d;

	partial void OnConstraintChanged() => OnClippingInvalidated(this);
	private static void OnClippingInvalidated(UIElement element) {
		if (VisualTreeHelper.GetParent(element) is Clipper translator)
			translator.InvalidateMeasure();
	}

	private static partial bool IsConstraintValid(ClipperConstraintSource value) => Enum.IsDefined(typeof(ClipperConstraintSource), value);
}

public enum ClipperConstraintSource {
	WidthAndHeight,
	Width,
	Height,
	Nothing,
}

[DependencyProperty<object>("Value")]
public partial class DependencyPropertySubscriber : DependencyObject {
	private readonly PropertyChangedCallback handler;

	public DependencyPropertySubscriber(DependencyObject dependencyObject, DependencyProperty dependencyProperty, PropertyChangedCallback handler) {
		if (dependencyObject is null) throw new ArgumentNullException(nameof(dependencyObject));
		if (dependencyProperty is null) throw new ArgumentNullException(nameof(dependencyProperty));
		if (handler is null) throw new ArgumentNullException(nameof(handler));
		this.handler = handler;

		Binding binding = new() {
			Path = new(dependencyProperty),
			Source = dependencyObject,
			Mode = BindingMode.OneWay,
		};
		BindingOperations.SetBinding(this, ValueProperty, binding);
	}

	public void Unsubscribe() => BindingOperations.ClearBinding(this, ValueProperty);

	partial void OnValueChanged(object? oldValue, object? newValue) => handler(this, new(ValueProperty, oldValue, newValue));
}
