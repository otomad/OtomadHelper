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
	public Path(string path) : base(path.Replace("\\", "/").TrimEnd('/').Split('/')) { }

	/// <summary>
	/// Construct a path class from an array of directories.
	/// </summary>
	/// <param name="arr">Directory array.</param>
	public Path(string[] arr) : base(arr) { }

	/// <summary>
	/// The input content is spliced one by one into a new path class.
	/// </summary>
	/// <param name="arr">Content.</param>
	/// <returns>A new path class.</returns>
	public static Path r(params string[] arr) {
		Path path = new(arr[0]);
		for (int i = 1; i < arr.Length; i++)
			path += new Path(arr[i]);
		return path;
	}

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

	/// <summary>
	/// Go up one directory level.
	/// </summary>
	public void UpOneLevel() {
		RemoveAt(Count - 1);
	}

	public override string ToString() => string.Join(sep.ToString(), this as List<string>);

	/// <summary>
	/// Make a copy of the current instance.
	/// </summary>
	/// <returns>Copy.</returns>
	public Path Copy() => new(ToString()) { IsWindows = IsWindows };

	private string GetLastItem() => this[Count - 1];

	/// <summary>
	/// Get the full path text.
	/// </summary>
	public string FullPath => ToString();

	/// <summary>
	/// Get or set the filename + extension of the file that the path ultimately points to.
	/// </summary>
	public string FullFileName {
		get => GetLastItem();
		set => this[Count - 1] = value;
	}

	private static readonly Regex extReg = new(@"(?<=\.)[^\.\\/:\*\?""<>\|]*$");

	/// <summary>
	/// Get or set the extension of the file that the path ultimately points to, <b>excluding</b> the leading period ".".
	/// </summary>
	public string Extension {
		get => DotExtension.Substring(1);
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
	public string FileName {
		get {
			if (Extension == "") return FullFileName;
			string fileName = extReg.Replace(FullFileName, "");
			return fileName.Substring(0, fileName.Length - 1);
		}
		set => FullFileName = value + '.' + Extension;
	}

	/// <summary>
	/// Get the directory where the file that the path ultimately points to is located.
	/// </summary>
	public string Directory => Count == 0 ? sep.ToString() : new Path(GetRange(0, Count - 1).ToArray()).ToString();

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

	public static Path operator +(Path path1, Path path2) {
		if (path1 is null) return path2;
		path1.AddRange(path2);
		return path1;
	}

	public static Path operator +(Path path1, string path2) {
		path1.AddRange(new Path(path2));
		return path1;
	}

	public static Path operator +(string _path1, Path path2) {
		Path path1 = new(_path1);
		for (int i = 0; i < path1.Count; i++)
			path1.Insert(i, path1[i]);
		return path1;
	}

	public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is not null && obj is Path path && this == path;

	public override int GetHashCode() {
		int hash = 0;
		foreach (string file in this)
			hash ^= file.GetHashCode();
		return hash;
	}

	public static implicit operator string(Path path) => path.ToString();
}
