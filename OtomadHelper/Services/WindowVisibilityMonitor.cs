namespace OtomadHelper.Services;

/// <summary>
/// Monitor <c>IsWindowVisible</c> and <c>IsIconic</c> Win32 function return value changes.
/// </summary>
public class WindowVisibilityMonitor : IDisposable {
	private readonly IntPtr targetHwnd;
	private IntPtr hEventHook = IntPtr.Zero;

	[DllImport("user32.dll")]
	private static extern IntPtr SetWinEventHook(
		uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
		WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

	[DllImport("user32.dll")]
	private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

	private delegate void WinEventDelegate(
		IntPtr hWinEventHook, uint eventType, IntPtr hWnd,
		int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

	private const uint WINEVENT_OUTOFCONTEXT = 0;
	private const uint EVENT_OBJECT_SHOW = 0x8002;
	private const uint EVENT_OBJECT_HIDE = 0x8003;
	private const uint EVENT_OBJECT_STATECHANGE = 0x800A;
	private const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
	private const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;

	private readonly WinEventDelegate eventDelegate;
	public event EventHandler<bool>? VisibilityChanged;

	public WindowVisibilityMonitor(IntPtr hWnd) {
		targetHwnd = hWnd;
		eventDelegate = WinEventCallback;
	}

	public void StartMonitoring() {
		hEventHook = SetWinEventHook(
			Math.Min(EVENT_OBJECT_SHOW, EVENT_SYSTEM_MINIMIZESTART), // Minimum event listened
			Math.Max(EVENT_OBJECT_STATECHANGE, EVENT_SYSTEM_MINIMIZEEND), // Maximum event listened
			IntPtr.Zero, // Global hook
			eventDelegate, // Callback function
			0, // All processes
			0, // All threads
			WINEVENT_OUTOFCONTEXT // Flag bit
		);
	}

	private void WinEventCallback(
		IntPtr hWinEventHook, uint eventType, IntPtr hWnd,
		int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
		// Filter non window events or non target window events
		if (idObject != 0 || idChild != 0 || hWnd != targetHwnd)
			return;

		// Check if the event type is related to visibility or minimization
		bool relevantEvent = eventType is (EVENT_OBJECT_SHOW or EVENT_OBJECT_HIDE or EVENT_OBJECT_STATECHANGE or EVENT_SYSTEM_MINIMIZESTART or EVENT_SYSTEM_MINIMIZEEND);
		if (!relevantEvent) return;

		// Trigger event if status changes
		VisibilityChanged?.Invoke(targetHwnd, VisibilityState);
	}

	public bool Visible => IsWindowVisible(targetHwnd);
	public bool Iconic => IsIconic(targetHwnd);
	public bool VisibilityState => !Iconic && Visible;

	public void Dispose() {
		StopMonitoring();
		GC.SuppressFinalize(this);
	}

	public void StopMonitoring() {
		if (hEventHook != IntPtr.Zero) {
			UnhookWinEvent(hEventHook);
			hEventHook = IntPtr.Zero;
		}
	}

	~WindowVisibilityMonitor() => StopMonitoring();
}
