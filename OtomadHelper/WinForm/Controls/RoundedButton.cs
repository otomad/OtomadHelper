using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OtomadHelper.WinForm.Controls;

[ToolboxBitmap(typeof(Button))]
public partial class RoundedButton : Button {
	public RoundedButton() {
		InitializeComponent();
		SetStyle(
			ControlStyles.UserPaint |
			ControlStyles.AllPaintingInWmPaint |
			ControlStyles.OptimizedDoubleBuffer |
			ControlStyles.ResizeRedraw |
			ControlStyles.SupportsTransparentBackColor, true
		);
		base.FlatStyle = FlatStyle.Flat;
		base.FlatAppearance.BorderSize = 0;
		Size = new Size(150, 40);
	}

	[Description("Specify the border corner radius of the button."), Category("Appearance"), DefaultValue(10)]
	public float BorderRadius { get; set { field = value; Invalidate(); } } = 10;
	[Description("If true, auto scale the border radius value by DPI; if false, remain the border radius value as it in graphically."), Category("Behavior"), DefaultValue(true)]
	public bool AutoScaleBorderRadiusByDpi { get; set { field = value; Invalidate(); } } = true;
	[Description("Represent the size, in pixels, of the border around the button."), Category("Appearance"), DefaultValue(0)]
	public int BorderSize { get; set { field = value; Invalidate(); } } = 0;
	[Description("Represent the color of the border around the button."), Category("Appearance")]
	public Color BorderColor { get => base.FlatAppearance.BorderColor; set => base.FlatAppearance.BorderColor = value; }
	[Description("Represent the color of the client area of the button when the mouse is pressed within the bounds of the control."), Category("Appearance")]
	public Color MouseDownBackColor { get => base.FlatAppearance.MouseDownBackColor; set => base.FlatAppearance.MouseDownBackColor = value; }
	[Description("Represent the color of the client area of the button when the mouse pointer is within the bounds of the control."), Category("Appearance")]
	public Color MouseOverBackColor { get => base.FlatAppearance.MouseOverBackColor; set => base.FlatAppearance.MouseOverBackColor = value; }

	[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete()]
	public new FlatButtonAppearance? FlatAppearance => null;
	[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete()]
	public new FlatStyle FlatStyle => base.FlatStyle;
	protected override bool ShowFocusCues => false;

	private GraphicsPath GetFigurePath(RectangleF rect, float radius) {
		radius = Enumerable.Min([radius, rect.Width / 2, rect.Height / 2]);
		GraphicsPath path = new();
		path.StartFigure();
		path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
		path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
		path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
		path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
		path.CloseFigure();
		return path;
	}

	protected override void OnPaint(PaintEventArgs e) {
		base.OnPaint(e);
		e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

		RectangleF rectSurface = new(2, 2, Width - 2, Height - 2);
		RectangleF rectBorder = new(1, 1, Width - 0.8f, Height - 1);

		float borderRadius = BorderRadius;
		if (AutoScaleBorderRadiusByDpi) {
			(double dpi, _) = this.Dpi;
			borderRadius *= (float)dpi;
		}

		if (borderRadius > 2) { // Rounded button
			using GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius);
			using GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - 1f);
			using Pen penSurface = new(BackColor, 2);
			using Pen penBorder = new(BorderColor, BorderSize);
			penBorder.Alignment = PenAlignment.Inset;
			Region = new(pathSurface); // Button surface
			e.Graphics.DrawPath(penSurface, pathSurface);
			if (BorderSize >= 1) // Button border
				e.Graphics.DrawPath(penBorder, pathBorder);
		} else { // Normal button
			Region = new(rectSurface); // Button surface
			if (BorderSize >= 1) { // Button border
				using Pen penBorder = new(BorderColor, BorderSize);
				e.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);
			}
		}
	}
}
