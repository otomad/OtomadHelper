using PathStatic = System.IO.Path;

namespace OtomadHelper.Helpers;

/// <summary>
/// Path class, used to handle paths.<br />
/// Although the system owned the <see cref="System.IO.Path"/> class, it is a static class and not very object-oriented.
/// </summary>
public class Path : List<string> {
	/// <summary>
	/// Construct a path class from a string.
	/// </summary>
	/// <param name="path">Path string.</param>
	public Path(string path) {
		path = path.Replace(new Regex(@".*:/+"), match => {
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
	/// The input content is spliced one by one into a new path class.
	/// </summary>
	/// <param name="arr">Content.</param>
	/// <returns>A new path class.</returns>
	[Obsolete("Use `new Path(...)` instead")]
	public static Path Resolve(params string[] paths) => new(paths);

	/// <summary>
	/// Protocol string like: <c>http://</c>, <c>file:///</c>, etc.
	/// </summary>
	public string Protocol { get; set; } = "";

	private char sep = PathStatic.DirectorySeparatorChar;

	private bool isWindows = true;

	/// <summary>
	/// Is the input a Windows format path?<br />
	/// This will determine the delimiter form of the final generated path.<br />
	/// Defaults to <c>true</c>.
	/// </summary>
	public bool IsWindows {
		get => isWindows;
		set {
			isWindows = value;
			sep = value ? PathStatic.DirectorySeparatorChar : PathStatic.AltDirectorySeparatorChar;
		}
	}

	/// <inheritdoc cref="List{T}.Add(T)"/>
	public new void Add(string path) {
		string[] paths = path.Replace(new Regex(@"[/\\]+|\0[/\\]*"), "/").TrimEnd('/').Split('/');
		foreach (string dir in paths) {
			if (dir is null) continue;
			else if (dir.Length == 0) Clear();
			else if (dir.Match(new(@"^\.+$")).Success) {
				if (dir == ".") {
				} else if (dir == "..") {
					if (Count >= 1) Up();
				} else
					throw new Exception($"Invalid path directory: `{dir}`");
				continue;
			} else
				base.Add(dir);
		}
	}

	public new void AddRange(IEnumerable<string> paths) => Add(JoinPaths(paths));

	private const string leadingSlashReplacement = "\0";
	/// <summary>
	/// If a path with leading slash, it will jump to the root path.
	/// Directly join it with slash to a string, will cause an issue.
	/// So we need to replace that special slash to a spacial symbol.
	/// </summary>
	private static string JoinPaths(IEnumerable<string> paths) =>
		paths.Select(path => path.StartsWith("/") || path.StartsWith("\\") ? leadingSlashReplacement + path[1..] : path).Join('/');

	[Obsolete("Do not use it")]
	public new void Insert(int index, string path) => throw new NotImplementedException();
	[Obsolete("Do not use it")]
	public new void InsertRange(int index, IEnumerable<string> paths) => throw new NotImplementedException();

	/// <summary>
	/// Go up one directory level.
	/// </summary>
	public void Up() => RemoveAt(Count - 1);

	public override string ToString() => Protocol + this.Join(sep);

	/// <summary>
	/// Make a copy of the current instance.
	/// </summary>
	/// <returns>Copy.</returns>
	public Path Copy() => new(ToString()) { IsWindows = IsWindows };

	private string GetLastItem() => this[^1];

	/// <summary>
	/// Get the full path text.
	/// </summary>
	public string FullPath => ToString();

	/// <summary>
	/// Get or set the filename + extension of the file that the path ultimately points to.
	/// </summary>
	public string FullFileName {
		get => GetLastItem();
		set => this[^1] = value;
	}

	private static readonly Regex extReg = new(@"(?<=\.)[^\.\\/:\*\?""<>\|]*$");

	/// <summary>
	/// Get or set the extension of the file that the path ultimately points to, <b>excluding</b> the leading period ".".
	/// </summary>
	public string Extension {
		get => DotExtension[1..];
		set => DotExtension = value;
	}

	/// <summary>
	/// Get or set the extension of the file that the path ultimately points to, including the leading period ".".
	/// </summary>
	public string DotExtension {
		get => PathStatic.GetExtension(FullFileName).ToLowerInvariant();
		set {
			value = value.Trim().TrimStart('.');
			FullFileName = PathStatic.ChangeExtension(FullFileName, value);
		}
	}

	/// <summary>
	/// Get or set the filename of the file that the path ultimately points to.
	/// </summary>
	public string BaseName {
		get {
			if (Extension == "") return FullFileName;
			string fileName = extReg.Replace(FullFileName, "");
			return fileName[..^1];
		}
		set => FullFileName = value + '.' + Extension;
	}

	/// <summary>
	/// Get the directory where the file that the path ultimately points to is located.
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

	public static Path operator +(string _path1, Path path2) {
		Path path1 = new(_path1);
		path1.AddRange(path2);
		return path1;
	}

	public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is not null && obj is Path path && this == path;

	public override int GetHashCode() {
		int hash = 0;
		foreach (string dir in this)
			hash ^= dir.GetHashCode();
		return hash;
	}

	public static implicit operator string(Path path) => path.ToString();

	public static implicit operator Uri(Path path) => new(path.ToString());
}
