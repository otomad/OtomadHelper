using Timer = System.Timers.Timer;

namespace OtomadHelper.Helpers;

public class DebounceThrottle {
	private Timer? _timerDbc;
	private Timer? _timerTrt;

	/// <summary>
	/// 延迟timesMs后执行。在此期间如果再次调用，则重新计时。
	/// </summary>
	/// <param name="invoker">同步对象，一般为Control控件。如不需同步可传null。</param>
	public void Debounce(int ms, Action action, ISynchronizeInvoke? invoker = null) {
		lock (this) {
			if (_timerDbc == null) {
				_timerDbc = new(ms) {
					AutoReset = false,
				};
				_timerDbc.Elapsed += (sender, e) => {
					_timerDbc.Stop();
					_timerDbc.Close();
					_timerDbc = null;
					InvokeAction(action, invoker);
				};
			}
			_timerDbc.Stop();
			_timerDbc.Start();
		}
	}

	/// <summary>
	/// 即刻执行，执行之后，在<paramref name="ms"/>内再次调用无效。
	/// </summary>
	/// <param name="ms">不应期，这段时间内调用无效。</param>
	/// <param name="invoker">同步对象，一般为控件。如不需同步可传null。</param>
	public void Throttle(int ms, Action action, ISynchronizeInvoke? invoker = null) {
		Monitor.Enter(this);
		bool needExit = true;
		try {
			if (_timerTrt == null) {
				_timerTrt = new(ms) {
					AutoReset = false,
				};
				_timerTrt.Elapsed += (sender, e) => {
					_timerTrt.Stop();
					_timerTrt.Close();
					_timerTrt = null;
				};
				_timerTrt.Start();
				Monitor.Exit(this);
				needExit = false;
				InvokeAction(action, invoker);//这个过程不能锁
			}
		} finally {
			if (needExit)
				Monitor.Exit(this);
		}
	}

	/// <summary>
	/// 延迟<paramref name="ms"/>后执行。
	/// </summary>
	public void Delay(int ms, Action action, ISynchronizeInvoke? invoker = null) {
		Debounce(ms, action, invoker);
	}

	private static void InvokeAction(Action action, ISynchronizeInvoke? invoker = null) {
		if (invoker is null)
			action();
		else if (invoker.InvokeRequired)
			invoker.Invoke(action, null);
		else
			action();
	}
}
