using PathStatic = System.IO.Path;

namespace OtomadHelper.Helpers;

/// <summary>
/// 路径类，用于处理路径。<br />
/// 虽然系统自带有 <c>PathStatic</c> 类，但那是一个静态类，不怎么面向对象。
/// </summary>
public class Path : List<string> {
	/// <summary>
	/// 通过一个字符串构造一个路径类。
	/// </summary>
	/// <param name="path">路径字符串</param>
	public Path(string path) : base(path.Replace("\\", "/").TrimEnd('/').Split('/')) { }

	/// <summary>
	/// 通过一段目录数组构造一个路径类。
	/// </summary>
	/// <param name="arr">目录数组</param>
	public Path(string[] arr) : base(arr) { }

	/// <summary>
	/// 通过输入的内容逐个拼接成一个新的路径类。
	/// </summary>
	/// <param name="arr">内容</param>
	/// <returns>新的路径类</returns>
	public static Path r(params string[] arr) {
		Path path = new(arr[0]);
		for (int i = 1; i < arr.Length; i++)
			path += new Path(arr[i]);
		return path;
	}

	private char sep = PathStatic.DirectorySeparatorChar;

	private bool isWindows = true;

	/// <summary>
	/// 是否输入为 Windows 格式路径？
	/// 这将决定最终生成的路径的分隔符形式。
	/// 默认为 <c>true</c>。
	/// </summary>
	public bool IsWindows {
		get => isWindows;
		set {
			isWindows = value;
			sep = value ? PathStatic.DirectorySeparatorChar : PathStatic.AltDirectorySeparatorChar;
		}
	}

	/// <summary>
	/// 向上一级。
	/// </summary>
	public void UpOneLevel() {
		RemoveAt(Count - 1);
	}

	public override string ToString() => string.Join(sep.ToString(), this as List<string>);

	/// <summary>
	/// 拷贝一份当前实例的副本。
	/// </summary>
	/// <returns>副本</returns>
	public Path Copy() => new(ToString()) { IsWindows = IsWindows };

	private string GetLastItem() => this[Count - 1];

	/// <summary>
	/// 获取完整路径文本。
	/// </summary>
	public string FullPath => ToString();

	/// <summary>
	/// 读取或更改路径最终指向文件的文件名 + 扩展名。
	/// </summary>
	public string FullFileName {
		get => GetLastItem();
		set => this[Count - 1] = value;
	}

	private static readonly Regex extReg = new(@"(?<=\.)[^\.\\/:\*\?""<>\|]*$");

	/// <summary>
	/// 读取或更改路径最终指向文件的扩展名，<b>不包括</b>前导的句点 “.”。
	/// </summary>
	public string Extension {
		get => DotExtension.Substring(1);
		set => DotExtension = value;
	}

	/// <summary>
	/// 读取或更改路径最终指向文件的扩展名，包括前导的句点 “.”。
	/// </summary>
	public string DotExtension {
		get => PathStatic.GetExtension(FullFileName).ToLower();
		set {
			value = value.Trim().TrimStart('.');
			FullFileName = PathStatic.ChangeExtension(FullFileName, value);
		}
	}

	/// <summary>
	/// 读取或更改路径最终指向文件的文件名。
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
	/// 获取路径最终指向文件所在的目录。
	/// </summary>
	public string Directory => Count == 0 ? sep.ToString() : new Path(GetRange(0, Count - 1).ToArray()).ToString();

	/// <summary>
	/// 判断该文件是否存在？
	/// </summary>
	public bool IsExist => File.Exists(FullPath);

	/// <summary>
	/// 判断该文件是否是路径？
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
