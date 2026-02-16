namespace OtomadHelper.Interop;
public class LibraryResourceStringLoader : IDisposable {
	private nint LibraryHandle { get; init; }
	private StringBuilder Buffer { get; init; }

	private LibraryResourceStringLoader(nint libraryHandle, int capacity) {
		LibraryHandle = libraryHandle;
		Buffer = new(capacity);
	}

	public static LibraryResourceStringLoader? Load(string dllPath, int expectedMaxStringLength = 1024) {
		nint handle = LoadLibrary(dllPath);
		return handle == 0 ? null : new(handle, expectedMaxStringLength);
	}

	public string GetString(uint resourceId) {
		int length = LoadString(LibraryHandle, resourceId, Buffer, Buffer.Capacity);
		return Buffer.ToString(0, length);
	}

	public string GetString(int resourceId) => GetString((uint)resourceId);

	public void Dispose() => FreeLibrary(LibraryHandle);

	[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern nint LoadLibrary(string lpFileName);

	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern int LoadString(nint hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool FreeLibrary(nint hModule);
}
