namespace OtomadHelper.Services;

public enum TimerType {
	/// <summary>
	/// <see cref="System.Timers.Timer"/>
	/// </summary>
	Timer,
	/// <summary>
	/// <see cref="System.Threading.Timer"/>
	/// </summary>
	Thread,
	/// <summary>
	/// <see cref="System.Windows.Forms.Timer"/>
	/// </summary>
	WinForm,
	/// <summary>
	/// <see cref="System.Windows.Threading.DispatcherTimer"/>
	/// </summary>
	WPF,
}

/// <inheritdoc cref="System.Timers.Timer" />
public interface ITimer {
	/// <summary>
	/// Which of the various timer types?
	/// </summary>
	public TimerType Type { get; }

	/// <inheritdoc cref="System.Windows.Threading.DispatcherTimer.Interval" />
	public double Milliseconds { get; set; }

	/// <inheritdoc cref="System.Windows.Forms.Timer.Start" />
	/// <param name="immediate">Run the timer callback immediately?</param>
	public ITimer Start(bool immediate = false);

	/// <inheritdoc cref="System.Windows.Forms.Timer.Stop" />
	public ITimer Stop();

	/// <summary>
	/// Run the timer only once with a time out delay.
	/// </summary>
	public ITimer SingleShot();

	public bool IsStart { get; }

	#region Implements

	/// <inheritdoc cref="TimerType.WinForm" />
	public class WinForm : ITimer {
		private readonly System.Windows.Forms.Timer timer;
		private readonly EventHandler callback;

		public bool IsStart { get; private set; }
		public TimerType Type => TimerType.WinForm;

		public WinForm(Action Callback, double ms) {
			timer = new();
			callback = (sender, e) => Callback();
			timer.Tick += callback;
			timer.Interval = (int)ms;
		}

		public double Milliseconds {
			get => timer.Interval;
			set => timer.Interval = (int)value;
		}

		public ITimer Start(bool immediate = false) {
			IsStart = true;
			timer.Enabled = true;
			timer.Start();
			if (immediate)
				callback(timer, EventArgs.Empty);
			return this;
		}

		public ITimer Stop() {
			timer.Stop();
			timer.Enabled = false;
			IsStart = false;
			return this;
		}

		public ITimer SingleShot() {
			timer.Tick += (sender, e) => Stop();
			Start();
			return this;
		}

		public static ITimer Timeout(Action Callback, double ms) =>
			new WinForm(Callback, ms).SingleShot();

		public static ITimer Interval(Action Callback, double ms) =>
			new WinForm(Callback, ms).Start();

		public static async Task Delay(double ms) {
			TaskCompletionSource<bool> taskCompletionSource = new(false);
			Timeout(() => taskCompletionSource.SetResult(true), ms);
			await taskCompletionSource.Task;
			return;
		}
	}

	/// <inheritdoc cref="TimerType.WPF" />
	public class WPF : ITimer {
		private readonly System.Windows.Threading.DispatcherTimer timer;
		private readonly EventHandler callback;

		public bool IsStart { get; private set; }
		public TimerType Type => TimerType.WPF;

		public WPF(Action Callback, double ms) {
			timer = new();
			callback = (sender, e) => Callback();
			timer.Tick += callback;
			timer.Interval = TimeSpan.FromMilliseconds(ms);
		}

		public double Milliseconds {
			get => timer.Interval.TotalMilliseconds;
			set => timer.Interval = TimeSpan.FromMilliseconds(value);
		}

		public ITimer Start(bool immediate = false) {
			IsStart = true;
			timer.IsEnabled = true;
			timer.Start();
			if (immediate)
				callback(timer, EventArgs.Empty);
			return this;
		}

		public ITimer Stop() {
			timer.Stop();
			timer.IsEnabled = false;
			IsStart = false;
			return this;
		}

		public ITimer SingleShot() {
			timer.Tick += (sender, e) => Stop();
			Start();
			return this;
		}

		public static ITimer Timeout(Action Callback, double ms) =>
			new WPF(Callback, ms).SingleShot();

		public static ITimer Interval(Action Callback, double ms) =>
			new WPF(Callback, ms).Start();

		public static async Task Delay(double ms) {
			TaskCompletionSource<bool> taskCompletionSource = new(false);
			Timeout(() => taskCompletionSource.SetResult(true), ms);
			await taskCompletionSource.Task;
			return;
		}
	}

	/// <inheritdoc cref="TimerType.Timer" />
	public class Timer : ITimer {
		private readonly System.Timers.Timer timer;
		private readonly System.Timers.ElapsedEventHandler callback;

		public bool IsStart { get; private set; }
		public TimerType Type => TimerType.Timer;

		public Timer(Action Callback, double ms) {
			timer = new();
			callback = (sender, e) => Callback();
			timer.Elapsed += callback;
			timer.Interval = ms;
			timer.AutoReset = true;
		}

		public double Milliseconds {
			get => timer.Interval;
			set => timer.Interval = value;
		}

		public ITimer Start(bool immediate = false) {
			IsStart = true;
			timer.Enabled = true;
			timer.Start();
			if (immediate)
				callback(timer, null);
			return this;
		}

		public ITimer Stop() {
			timer.Stop();
			timer.Enabled = false;
			IsStart = false;
			return this;
		}

		public ITimer SingleShot() {
			timer.Elapsed += (sender, e) => Stop();
			Start();
			return this;
		}

		public static ITimer Timeout(Action Callback, double ms) =>
			new Timer(Callback, ms).SingleShot();

		public static ITimer Interval(Action Callback, double ms) =>
			new Timer(Callback, ms).Start();

		public static async Task Delay(double ms) {
			TaskCompletionSource<bool> taskCompletionSource = new(false);
			Timeout(() => taskCompletionSource.SetResult(true), ms);
			await taskCompletionSource.Task;
			return;
		}
	}

	/// <inheritdoc cref="TimerType.Thread" />
	public class Thread : ITimer {
		private System.Threading.Timer? timer;
		private readonly TimerCallback callback;
		private double ms;

		public bool IsStart { get; private set; }
		public TimerType Type => TimerType.Thread;

		public Thread(Action Callback, double ms) {
			callback = state => Callback();
			this.ms = ms;
		}

		public double Milliseconds {
			get => ms;
			set {
				ms = value;
				timer?.Change((long)value, 0);
			}
		}

		public ITimer Start(bool immediate = false) {
			IsStart = true;
			timer = new(callback, this, (long)ms, 0);
			if (immediate)
				callback(null);
			return this;
		}

		public ITimer Stop() {
			timer?.Dispose();
			IsStart = false;
			return this;
		}

		public ITimer SingleShot() {
			timer = new(callback, this, (long)ms, System.Threading.Timeout.Infinite);
			return this;
		}

		public static ITimer Timeout(Action Callback, double ms) =>
			new Thread(Callback, ms).SingleShot();

		public static ITimer Interval(Action Callback, double ms) =>
			new Thread(Callback, ms).Start();

		public static async Task Delay(double ms) {
			TaskCompletionSource<bool> taskCompletionSource = new(false);
			Timeout(() => taskCompletionSource.SetResult(true), ms);
			await taskCompletionSource.Task;
			return;
		}
	}

	#endregion
}