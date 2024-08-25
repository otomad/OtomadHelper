namespace OtomadHelper.Helpers;

public static class Misc {
	/// <summary>
	/// Open an Internet website link with the default browser.
	/// </summary>
	/// <remarks>
	/// If the link is invalid here (unfortunately when the link contains "?" or "=" will be invalid),
	/// it will unexpected open the explorer.
	/// </remarks>
	/// <param name="link">Internet website link.</param>
	public static void OpenLink(string link) {
		link = "\"" + link.Replace("\"", "\"\"") + "\""; // Escape the double quotes
		Process.Start("explorer.exe", link);
	}

	public static void RunCmdBackground(string command, bool waitForExit = false) {
		ProcessStartInfo processStartInfo = new("cmd.exe", " /c " + command) {
			CreateNoWindow = true,
			UseShellExecute = false,
		};
		Process process = Process.Start(processStartInfo); // still crash, don't know why.
		if (waitForExit) process.WaitForExit();
	}
}
