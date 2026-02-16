namespace OtomadHelper.Services;

/// <summary>
/// Monitor <c>IsWindowVisible</c> and <c>IsIconic</c> Win32 function return value changes.
/// </summary>
public class WindowVisibilityMonitor : IDisposable {
	private readonly nint targetHwnd;
	private nint hEventHook = 0;

	[DllImport("user32.dll")]
	private static extern nint SetWinEventHook(
		uint eventMin, uint eventMax, nint hmodWinEventProc,
		WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

	[DllImport("user32.dll")]
	private static extern bool UnhookWinEvent(nint hWinEventHook);

	private delegate void WinEventDelegate(
		nint hWinEventHook, uint eventType, nint hWnd,
		int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

	private const uint WINEVENT_OUTOFCONTEXT = 0;
	private const uint EVENT_OBJECT_SHOW = 0x8002;
	private const uint EVENT_OBJECT_HIDE = 0x8003;
	private const uint EVENT_OBJECT_STATECHANGE = 0x800A;
	private const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
	private const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;

	private readonly WinEventDelegate eventDelegate;
	public event EventHandler<bool>? VisibilityChanged;

	public WindowVisibilityMonitor(nint hWnd) {
		targetHwnd = hWnd;
		eventDelegate = WinEventCallback;
	}

	public void StartMonitoring() {
		hEventHook = SetWinEventHook(
			Math.Min(EVENT_OBJECT_SHOW, EVENT_SYSTEM_MINIMIZESTART), // Minimum event listened
			Math.Max(EVENT_OBJECT_STATECHANGE, EVENT_SYSTEM_MINIMIZEEND), // Maximum event listened
			0, // Global hook
			eventDelegate, // Callback function
			0, // All processes
			0, // All threads
			WINEVENT_OUTOFCONTEXT // Flag bit
		);
	}

	private void WinEventCallback(
		nint hWinEventHook, uint eventType, nint hWnd,
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
		if (hEventHook != 0) {
			UnhookWinEvent(hEventHook);
			hEventHook = 0;
		}
	}

	~WindowVisibilityMonitor() => StopMonitoring();
}
