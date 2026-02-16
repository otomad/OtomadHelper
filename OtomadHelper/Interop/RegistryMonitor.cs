namespace OtomadHelper.Interop;

/// <summary>
/// <b>RegistryMonitor</b> allows you to monitor specific registry key.
/// </summary>
/// <remarks>
/// If a monitored registry key changes, an event is fired. You can subscribe to these
/// events by adding a delegate to <see cref="RegChanged"/>.
/// <para>The Windows API provides a function
/// <a href="http://msdn.microsoft.com/library/en-us/sysinfo/base/regnotifychangekeyvalue.asp">
/// RegNotifyChangeKeyValue</a>, which is not covered by the
/// <see cref="Microsoft.Win32.RegistryKey"/> class. <see cref="RegistryMonitor"/> imports
/// that function and encapsulates it in a convenient manner.
/// </para>
/// <example>
/// This sample shows how to monitor <c>HKEY_CURRENT_USER\Environment</c> for changes:
/// <code>
/// public class MonitorSample {
///     static void Main() {
///         RegistryMonitor monitor = new RegistryMonitor(RegistryHive.CurrentUser, "Environment");
///         monitor.RegChanged += new EventHandler(OnRegChanged);
///         monitor.Start();
///
///         while(true);
///
///         monitor.Stop();
///     }
///
///     private void OnRegChanged(object sender, EventArgs e) {
///         Console.WriteLine("registry key has changed");
///     }
/// }
/// </code>
/// </example>
/// </remarks>
public class RegistryMonitor : IDisposable {
	#region P/Invoke
	[DllImport("advapi32.dll", SetLastError = true)]
	private static extern int RegOpenKeyEx(nint hKey, string subKey, uint options, int samDesired, out nint phkResult);

	[DllImport("advapi32.dll", SetLastError = true)]
	private static extern int RegNotifyChangeKeyValue(nint hKey, bool bWatchSubtree, RegChangeNotifyFilters dwNotifyFilter, nint hEvent, bool fAsynchronous);

	[DllImport("advapi32.dll", SetLastError = true)]
	private static extern int RegCloseKey(nint hKey);

	private const int KEY_QUERY_VALUE = 0x0001;
	private const int KEY_NOTIFY = 0x0010;
	private const int STANDARD_RIGHTS_READ = 0x00020000;

	private static readonly nint HKEY_CLASSES_ROOT = unchecked((int)0x80000000);
	private static readonly nint HKEY_CURRENT_USER = unchecked((int)0x80000001);
	private static readonly nint HKEY_LOCAL_MACHINE = unchecked((int)0x80000002);
	private static readonly nint HKEY_USERS = unchecked((int)0x80000003);
	private static readonly nint HKEY_PERFORMANCE_DATA = unchecked((int)0x80000004);
	private static readonly nint HKEY_CURRENT_CONFIG = unchecked((int)0x80000005);
	private static readonly nint HKEY_DYN_DATA = unchecked((int)0x80000006);

	/// <summary>
	/// Filter for notifications reported by <see cref="RegistryMonitor"/>.
	/// </summary>
	[Flags]
	public enum RegChangeNotifyFilters {
		/// <summary>Notify the caller if a subkey is added or deleted.</summary>
		Key = 1 << 0,
		/// <summary>Notify the caller of changes to the attributes of the key,
		/// such as the security descriptor information.</summary>
		Attribute = 1 << 1,
		/// <summary>Notify the caller of changes to a value of the key. This can
		/// include adding or deleting a value, or changing an existing value.</summary>
		Value = 1 << 2,
		/// <summary>Notify the caller of changes to the security descriptor
		/// of the key.</summary>
		Security = 1 << 3,
	}
	#endregion

	#region Event handling
	/// <summary>
	/// Occurs when the specified registry key has changed.
	/// </summary>
	public event EventHandler? RegChanged;

	/// <summary>
	/// Raises the <see cref="RegChanged"/> event.
	/// </summary>
	/// <remarks>
	/// <p>
	/// <b>OnRegChanged</b> is called when the specified registry key has changed.
	/// </p>
	/// <note type="inheritinfo">
	/// When overriding <see cref="OnRegChanged"/> in a derived class, be sure to call
	/// the base class's <see cref="OnRegChanged"/> method.
	/// </note>
	/// </remarks>
	protected virtual void OnRegChanged() {
		EventHandler? handler = RegChanged;
		handler?.Invoke(this, null);
	}

	/// <summary>
	/// Occurs when the access to the registry fails.
	/// </summary>
	public event ErrorEventHandler? Error;

	/// <summary>
	/// Raises the <see cref="Error"/> event.
	/// </summary>
	/// <param name="e">The <see cref="Exception"/> which occured while watching the registry.</param>
	/// <remarks>
	/// <p>
	/// <b>OnError</b> is called when an exception occurs while watching the registry.
	/// </p>
	/// <note type="inheritinfo">
	/// When overriding <see cref="OnError"/> in a derived class, be sure to call
	/// the base class's <see cref="OnError"/> method.
	/// </note>
	/// </remarks>
	protected virtual void OnError(Exception e) {
		ErrorEventHandler? handler = Error;
		handler?.Invoke(this, new ErrorEventArgs(e));
	}
	#endregion

	#region Private member variables
	private nint _registryHive;
	private string _registrySubName = null!;
	private readonly object _threadLock = new();
	private Thread? _thread;
	private bool _disposed = false;
	private readonly ManualResetEvent _eventTerminate = new(false);
	#endregion

	/// <summary>
	/// Initializes a new instance of the <see cref="RegistryMonitor"/> class.
	/// </summary>
	/// <param name="registryKey">The registry key to monitor.</param>
	public RegistryMonitor(RegistryKey registryKey) => InitRegistryKey(registryKey.Name);

	/// <summary>
	/// Initializes a new instance of the <see cref="RegistryMonitor"/> class.
	/// </summary>
	/// <param name="name">The name.</param>
	public RegistryMonitor(string name) {
		if (string.IsNullOrEmpty(name))
			throw new ArgumentNullException("name");

		InitRegistryKey(name);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="RegistryMonitor"/> class.
	/// </summary>
	/// <param name="registryHive">The registry hive.</param>
	/// <param name="subKey">The sub key.</param>
	public RegistryMonitor(RegistryHive registryHive, string subKey) => InitRegistryKey(registryHive, subKey);

	/// <summary>
	/// Disposes this object.
	/// </summary>
	public void Dispose() {
		Stop();
		_disposed = true;
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Gets or sets the <see cref="RegChangeNotifyFilter">RegChangeNotifyFilter</see>.
	/// </summary>
	public RegChangeNotifyFilters RegChangeNotifyFilter {
		get;
		set {
			lock (_threadLock) {
				if (IsMonitoring)
					throw new InvalidOperationException("Monitoring thread is already running");

				field = value;
			}
		}
	} = RegChangeNotifyFilters.Key | RegChangeNotifyFilters.Attribute | RegChangeNotifyFilters.Value | RegChangeNotifyFilters.Security;

	#region Initialization
	private void InitRegistryKey(RegistryHive hive, string name) {
		_registryHive = hive switch {
			RegistryHive.ClassesRoot => HKEY_CLASSES_ROOT,
			RegistryHive.CurrentConfig => HKEY_CURRENT_CONFIG,
			RegistryHive.CurrentUser => HKEY_CURRENT_USER,
			RegistryHive.DynData => HKEY_DYN_DATA,
			RegistryHive.LocalMachine => HKEY_LOCAL_MACHINE,
			RegistryHive.PerformanceData => HKEY_PERFORMANCE_DATA,
			RegistryHive.Users => HKEY_USERS,
			_ => throw new InvalidEnumArgumentException("hive", (int)hive, typeof(RegistryHive))
		};
		_registrySubName = name;
	}

	private void InitRegistryKey(string name) {
		string[] nameParts = name.Split('\\');

		_registryHive = 0;
		_registryHive = nameParts[0] switch {
			"HKEY_CLASSES_ROOT" or "HKCR" => HKEY_CLASSES_ROOT,
			"HKEY_CURRENT_USER" or "HKCU" => HKEY_CURRENT_USER,
			"HKEY_LOCAL_MACHINE" or "HKLM" => HKEY_LOCAL_MACHINE,
			"HKEY_USERS" or "HKU" => HKEY_USERS,
			"HKEY_CURRENT_CONFIG" or "HKCC" => HKEY_CURRENT_CONFIG,
			_ => throw new ArgumentException($"The registry hive '{nameParts[0]}' is not supported", nameof(name))
		};

		_registrySubName = string.Join("\\", nameParts, 1, nameParts.Length - 1);
	}
	#endregion

	/// <summary>
	/// <see langword="true" /> if this <see cref="RegistryMonitor"/> object is currently monitoring;
	/// otherwise, <see langword="false" />.
	/// </summary>
	public bool IsMonitoring => _thread is not null;

	/// <summary>
	/// Start monitoring.
	/// </summary>
	public void Start() {
		if (_disposed)
			throw new ObjectDisposedException(null, "This instance is already disposed");

		lock (_threadLock) {
			if (!IsMonitoring) {
				_eventTerminate.Reset();
				_thread = new(new ThreadStart(MonitorThread)) {
					IsBackground = true
				};
				_thread.Start();
			}
		}
	}

	/// <summary>
	/// Stops the monitoring thread.
	/// </summary>
	public void Stop() {
		if (_disposed)
			throw new ObjectDisposedException(null, "This instance is already disposed");

		lock (_threadLock) {
			Thread? thread = _thread;
			if (thread is not null) {
				_eventTerminate.Set();
				thread.Join();
			}
		}
	}

	private void MonitorThread() {
		try {
			ThreadLoop();
		} catch (Exception e) {
			OnError(e);
		}
		_thread = null;
	}

	private void ThreadLoop() {
		int result = RegOpenKeyEx(_registryHive, _registrySubName, 0, STANDARD_RIGHTS_READ | KEY_QUERY_VALUE | KEY_NOTIFY, out nint registryKey);
		if (result != 0)
			throw new Win32Exception(result);

		try {
			AutoResetEvent _eventNotify = new(false);
			WaitHandle[] waitHandles = [_eventNotify, _eventTerminate];
			while (!_eventTerminate.WaitOne(0, true)) {
				// Replace the obsolete `_eventNotify.Handle` property with `_eventNotify.SafeWaitHandle`.
				result = RegNotifyChangeKeyValue(registryKey, true, RegChangeNotifyFilter, _eventNotify.SafeWaitHandle.DangerousGetHandle(), true);
				if (result != 0)
					throw new Win32Exception(result);

				if (WaitHandle.WaitAny(waitHandles) == 0)
					OnRegChanged();
			}
		} finally {
			if (registryKey != 0)
				RegCloseKey(registryKey);
		}
	}
}
