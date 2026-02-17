using PathStatic = System.IO.Path;

namespace OtomadHelper.Helpers;

/// <summary>
/// Path class, used to handle paths.<br />
/// Although the system owned the <see cref="PathStatic" /> class, it is a static class and not very object-oriented.
/// </summary>
public class Path :
	IList, IList<string>, IReadOnlyList<string> {
	protected readonly List<string> directories = [];

	/// <summary>
	/// Construct a path class from a string.
	/// </summary>
	/// <param name="path">Path string.</param>
	public Path(string path) {
		path = path.Replace(new(@"^.*:/+"), match => {
			Protocol = match.Value;
			return "";
		});
		Add(path);
	}

	/// <summary>
	/// Construct a path class from an array of directories.
	/// </summary>
	/// <param name="dirs">Directory array.</param>
	public Path(params IEnumerable<string> dirs) : this(JoinPaths(dirs)) { }

	/// <summary>
	/// Construct a path class from a URI object.
	/// </summary>
	/// <param name="uri">URI object.</param>
	public Path(Uri uri) : this(uri.ToString()) { }

	/// <summary>
	/// Make a clone of an existed <see cref="Path" /> object instance.
	/// </summary>
	/// <param name="path"><see cref="Path" /> object instance.</param>
	public Path(Path path) {
		directories.AddRange(path.directories);
		Protocol = path.Protocol;
		IsWindows = path.IsWindows;
	}

	/// <summary>
	/// Construct a path class from a string builder (buffer).
	/// </summary>
	/// <param name="buffer">Path string buffer.</param>
	public Path(StringBuilder buffer) : this(buffer.ToString()) { }

	/// <summary>
	/// Protocol string like: <c>http://</c>, <c>file:///</c>, etc.
	/// </summary>
	public string Protocol { get; set; } = "";

	private char sep = PathStatic.DirectorySeparatorChar;

	/// <summary>
	/// Is the input a Windows format path?<br />
	/// This will determine the delimiter form of the final generated path.<br />
	/// Defaults to <c>true</c>.
	/// </summary>
	public bool IsWindows {
		get => field;
		set {
			field = value;
			sep = value ? PathStatic.DirectorySeparatorChar : PathStatic.AltDirectorySeparatorChar;
		}
	}

	/// <inheritdoc cref="List{T}.Add(T)"/>
	public void Add(string path) {
		string[] paths = path.Replace(new Regex(@"[/\\]+|\0[/\\]*"), "/").TrimEnd('/').Split('/');
		foreach (string dir in paths) {
			if (dir is null) continue;
			else if (dir.Length == 0) Clear();
			else if (dir.IsMatch(new(@"^\.+$"))) {
				if (dir == ".") {
				} else if (dir == "..") {
					if (Count >= 1) Up();
				} else
					throw new($"Invalid path directory: `{dir}`");
				continue;
			} else
				AddBasically(dir);
		}
	}

	/// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
	public void AddRange(IEnumerable<string> paths) => Add(JoinPaths(paths));

	private const string leadingSlashReplacement = "\0";
	/// <summary>
	/// If a path with leading slash, it will jump to the root path.
	/// Directly join it with slash to a string, will cause an issue.
	/// So we need to replace that special slash to a spacial symbol.
	/// </summary>
	private static string JoinPaths(IEnumerable<string> paths) =>
		paths.Select(path => path.StartsWith("/") || path.StartsWith("\\") ? leadingSlashReplacement + path[1..] : path).Join('/');

	/// <summary>
	/// Go up one directory level.
	/// </summary>
	public void Up() => RemoveAt(Count - 1);

	/// <inheritdoc cref="FullPath" />
	public override string ToString() => Protocol + this.Join(sep);

	/// <summary>
	/// Make a clone of the current instance.
	/// </summary>
	/// <returns>Cloned new instance.</returns>
	public Path Clone() => [.. this];

	/// <summary>
	/// Get the full path text.
	/// </summary>
	/// <remarks>
	/// For example, the path "<c>C:\Windows\MyFile.txt</c>" returns "<c>C:\Windows\MyFile.txt</c>".
	/// </remarks>
	public string FullPath => ToString();

	/// <summary>
	/// Get the directory name of the file path.
	/// </summary>
	/// <remarks>
	/// Returns the directory information for path, or <see langword="null" /> if path denotes a root directory or is <see langword="null" />,
	/// or <see cref="string.Empty" /> if path does not contain directory information.
	/// <para>For example, the path "<c>C:\Windows\MyFile.txt</c>" returns "<c>C:\Windows</c>".</para>
	/// </remarks>
	public string DirName => PathStatic.GetDirectoryName(FullPath);

	/// <summary>
	/// Get or set the filename + extension of the file that the path finally points to.
	/// </summary>
	/// <remarks>
	/// For example, the path "<c>C:\Windows\MyFile.txt</c>" returns "<c>MyFile.txt</c>".
	/// </remarks>
	public string FileName {
		get => this[^1];
		set => this[^1] = value;
	}

	private static readonly Regex extReg = new(@"(?<=\.)[^\.\\/:\*\?""<>\|]*$");

	/// <summary>
	/// Get or set the extension of the file that the path finally points to, <b>excluding</b> the leading period ".".
	/// </summary>
	/// <remarks>
	/// For example, the path "<c>C:\Windows\MyFile.txt</c>" returns "<c>txt</c>".
	/// </remarks>
	public string Extension {
		get => DotExtension[1..];
		set => DotExtension = value;
	}

	/// <summary>
	/// Get or set the extension of the file that the path finally points to, including the leading period ".".
	/// </summary>
	/// <remarks>
	/// For example, the path "<c>C:\Windows\MyFile.txt</c>" returns "<c>.txt</c>".
	/// </remarks>
	public string DotExtension {
		get => PathStatic.GetExtension(FileName).ToLowerInvariant();
		set {
			value = value.Trim().TrimStart('.');
			FileName = PathStatic.ChangeExtension(FileName, value);
		}
	}

	/// <summary>
	/// Get or set the filename without extension of the file that the path finally points to.
	/// </summary>
	/// <remarks>
	/// For example, the path "<c>C:\Windows\MyFile.txt</c>" returns "<c>MyFile</c>".
	/// </remarks>
	public string FileRoot {
		get {
			if (Extension == "") return FileName;
			string fileName = extReg.Replace(FileName, "");
			return fileName[..^1];
		}
		set => FileName = value + '.' + Extension;
	}

	#region File System Operations
	/// <summary>
	/// Get the directory where the file that the path finally points to is located.
	/// </summary>
	public string Directory => Protocol + (Count == 0 ? sep.ToString() : new Path([.. GetRange(0, Count - 1)]).ToString());

	/// <summary>
	/// Determine whether the file exists?
	/// </summary>
	public bool IsExist => File.Exists(FullPath);

	/// <summary>
	/// Determine whether the path is a directory?
	/// </summary>
	public bool IsDirectory => File.GetAttributes(FullPath).HasFlag(FileAttributes.Directory);

	/// <summary>
	/// Create the directory with current path if it is not exist.
	/// </summary>
	/// <returns>
	/// Return <see langword="null" /> if create successfully, or return corresponding <see cref="Exception" /> instance if raise any exception.
	/// </returns>
	public Exception? CreateDirectory() {
		try {
			System.IO.Directory.CreateDirectory(FullPath);
			return null;
		} catch (Exception e) {
			return e;
		}
	}
	#endregion

	#region Operator Overloading
	public static bool operator ==(Path path1, Path path2) => path1.SequenceEqual(path2);

	public static bool operator !=(Path path1, Path path2) => !(path1 == path2);

	public static Path operator +(Path? path1, Path path2) {
		if (path1 is null) return path2;
		path1.AddRange(path2);
		return path1;
	}

	public static Path operator +(Path path1, string path2) {
		path1.Add(path2);
		return path1;
	}

	// NOTE: `string` + `Path` will be converted to `string` instead of `Path`!

	public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is not null && obj is Path path && this == path;

	public override int GetHashCode() {
		HashCode hashCode = new();
		hashCode.Add(Protocol);
		foreach (string dir in this)
			hashCode.Add(dir);
		return hashCode.ToHashCode();
	}

	public static implicit operator string(Path path) => path.ToString();

	public static implicit operator Uri(Path path) => new(path.ToString());

	public static implicit operator List<string>(Path path) => [.. path.directories];

	public static implicit operator string[](Path path) => [.. path.directories];

	public static explicit operator Path(string path) => new(path);

	public static explicit operator Path(StringBuilder buffer) => new(buffer);

	public static explicit operator Path(Uri uri) => new(uri);
	#endregion

	#region Implementation
	public IEnumerator<string> GetEnumerator() => directories.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	void ICollection.CopyTo(Array array, int index) => ((ICollection)directories).CopyTo(array, index);
	object ICollection.SyncRoot => ((ICollection)directories).SyncRoot;
	bool ICollection.IsSynchronized => ((ICollection)directories).IsSynchronized;

	void ICollection<string>.CopyTo(string[] array, int arrayIndex) => directories.CopyTo(array, arrayIndex);
	bool ICollection<string>.Remove(string item) => throw new NotImplementedException();
	bool ICollection<string>.IsReadOnly => false;

	int IList.Add(object value) => throw new NotImplementedException();
	void IList.Insert(int index, object value) => throw new NotImplementedException();
	void IList.Remove(object value) => throw new NotImplementedException();
	bool IList.Contains(object value) => ((IList)directories).Contains(value);
	int IList.IndexOf(object value) => ((IList)directories).IndexOf(value);
	bool IList.IsFixedSize => ((IList)directories).IsFixedSize;
	bool IList.IsReadOnly => false;
	object IList.this[int index] {
		get => directories[index];
		set => directories[index] = (string)value;
	}

	void IList<string>.Insert(int index, string item) => directories.Insert(index, item);

	public int Count => directories.Count;
	public void Clear() => directories.Clear();
	public bool Contains(string item) => directories.Contains(item);
	public int IndexOf(string item) => directories.IndexOf(item);
	public void RemoveAt(int index) => directories.RemoveAt(index);
	public string this[int index] {
		get => directories[index];
		set => directories[index] = value;
	}

	/// <inheritdoc cref="List{T}.GetRange(int, int)" />
	protected List<string> GetRange(int index, int count) => directories.GetRange(index, count);

	/// <inheritdoc cref="List{T}.Add(T)"/>
	protected void AddBasically(string item) => directories.Add(item);
	#endregion
}
