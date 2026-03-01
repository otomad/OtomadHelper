namespace OtomadHelper.Interop;

/// <summary>
/// Provides functionality to load and retrieve resource strings from a library (DLL).
/// </summary>
public class LibraryResourceStringLoader : IDisposable {
	/// <summary>
	/// Gets the handle to the loaded library.
	/// </summary>
	private nint LibraryHandle { get; init; }

	/// <summary>
	/// Gets the buffer used for retrieving resource strings.
	/// </summary>
	private StringBuilder Buffer { get; init; }

	/// <summary>
	/// Initializes a new instance of the <see cref="LibraryResourceStringLoader" /> class.
	/// </summary>
	/// <param name="libraryHandle">The handle to the loaded library.</param>
	/// <param name="capacity">The capacity of the buffer for resource strings.</param>
	private LibraryResourceStringLoader(nint libraryHandle, int capacity) {
		LibraryHandle = libraryHandle;
		Buffer = new(capacity);
	}

	/// <summary>
	/// Loads a library and creates an instance of <see cref="LibraryResourceStringLoader" />.
	/// </summary>
	/// <param name="dllPath">The path to the DLL to load.</param>
	/// <param name="expectedMaxStringLength">The expected maximum length of resource strings.</param>
	/// <returns>An instance of <see cref="LibraryResourceStringLoader"/> or null if the library could not be loaded.</returns>
	public static LibraryResourceStringLoader? Load(string dllPath, int expectedMaxStringLength = 1024) {
		nint handle = LoadLibrary(dllPath);
		return handle == 0 ? null : new(handle, expectedMaxStringLength);
	}

	/// <summary>
	/// Retrieves a resource string from the loaded library.
	/// </summary>
	/// <param name="resourceId">The ID of the resource string.</param>
	/// <returns>The resource string.</returns>
	public string GetString(uint resourceId) {
		int length = LoadString(LibraryHandle, resourceId, Buffer, Buffer.Capacity);
		return Buffer.ToString(0, length);
	}

	/// <summary>
	/// Retrieves a resource string from the loaded library.
	/// </summary>
	/// <param name="resourceId">The ID of the resource string.</param>
	/// <returns>The resource string.</returns>
	public string GetString(int resourceId) => GetString((uint)resourceId);

	/// <summary>
	/// Releases the resources used by the <see cref="LibraryResourceStringLoader"/> class.
	/// </summary>
	public void Dispose() {
		FreeLibrary(LibraryHandle);
		GC.SuppressFinalize(this);
	}

	[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern nint LoadLibrary(string lpFileName);

	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern int LoadString(nint hInstance, uint uid, StringBuilder lpBuffer, int nBufferMax);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool FreeLibrary(nint hModule);
}
