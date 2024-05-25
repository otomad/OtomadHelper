using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace OtomadHelper.Module;

/// <summary>
/// Reading of response content stream happens asynchronously, and WebView2 does not
/// directly dispose the stream once it read. Therefore, use the following stream
/// class, which properly disposes when WebView2 has read all data. For details, see
/// <a href="https://github.com/MicrosoftEdge/WebView2Feedback/issues/2513">
/// CoreWebView2 does not close stream content</a>.
/// </summary>
internal class ManagedStream : Stream {
	#region Overrides
	private readonly Stream s;

	public ManagedStream(Stream s) => this.s = s;

	public override bool CanRead => s.CanRead;

	public override bool CanSeek => s.CanSeek;

	public override bool CanWrite => s.CanWrite;

	public override long Length => s.Length;

	public override long Position { get => s.Position; set => s.Position = value; }

	public override long Seek(long offset, SeekOrigin origin) => s.Seek(offset, origin);

	public override void Flush() => throw new NotImplementedException();

	public override void SetLength(long value) => throw new NotImplementedException();

	public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();

	public override int Read(byte[] buffer, int offset, int count) {
		int read = 0;
		try {
			read = s.Read(buffer, offset, count);
			if (read == 0)
				s.Dispose();
		} catch {
			s.Dispose();
			throw;
		}
		return read;
	}
	#endregion

	internal const string RESOURCE_HOST = "https://app.otomadhelper.example/";

	internal static void Handler(WebView2 webView) {
		const string HOST = RESOURCE_HOST + "*";
		webView.CoreWebView2.AddWebResourceRequestedFilter(HOST, CoreWebView2WebResourceContext.All);
		webView.CoreWebView2.WebResourceRequested += (sender, args) => {
			CoreWebView2Environment webViewEnv = webView.CoreWebView2.Environment;
			Uri uri = new(args.Request.Uri);
			NameValueCollection query = HttpUtility.ParseQueryString(uri.Query); // Not used yet
			string file = uri.AbsolutePath.Substring(1);
			file = Uri.UnescapeDataString(file);
			string[] fileSlug = file.Split('/');
			string virtualPath = fileSlug.FirstOrDefault();
			string assetsFilePath = "Web.dist." + file.Replace("/", ".");
			try {
				if (virtualPath != null) {
					string path = string.Join("/", fileSlug.Skip(1));
					switch (virtualPath) {
						case "thumbnail":
							Handler_Thumbnail(webView, args, path, false);
							return;
						case "fileicon":
							Handler_Thumbnail(webView, args, path, true);
							return;
						default:
							break;
					}
				}
				Stream fileStream = ResourceHelper.GetEmbeddedResource(assetsFilePath);
				ManagedStream managedStream = new(fileStream);
				string contentType = new Path(file).Extension switch {
					"html" => "text/html",
					"js" => "text/javascript",
					"css" => "text/css",
					"appcache" => "text/cache-manifest",
					"jpg" => "image/jpeg",
					"png" => "image/png",
					"gif" => "image/gif",
					"svg" => "image/svg+xml",
					"webp" => "image/webp",
					"apng" => "image/apng",
					"ico" => "image/vnd.microsoft.icon",
					"cur" => "image/x-win-bitmap",
					"bmp" => "image/bmp",
					"woff" => "font/woff",
					"woff2" => "font/woff2",
					"ttf" => "font/ttf",
					"json" => "application/json",
					"manifest" => "application/manifest+json",
					"ani" => "application/x-navi-animation",
					_ => "application/octet-stream",
				};
				const int AGE = 1200;
				string headers = $"""
					HTTP/1.1 200 OK
					Content-Type: {contentType}
					Cache-Control: max-age={AGE}
					Age: {AGE}
					Keep-Alive: timeout={AGE}
					Date: {DateTime.UtcNow.ToUniversalTime():R}
					""";
				args.Response = webViewEnv.CreateWebResourceResponse(managedStream, 200, "OK", headers);
			} catch (InvalidOperationException e) {
				args.Response = webViewEnv.CreateWebResourceResponse(null, 415, e.Message, "");
			} catch (Exception) {
				args.Response = webViewEnv.CreateWebResourceResponse(null, 404, "Not Found", "");
			}
		};
	}

	private static void Handler_Thumbnail(WebView2 webView, CoreWebView2WebResourceRequestedEventArgs args, string filePath, bool allowIcon = false) {
		filePath = filePath.Replace("/", "\\");
		BitmapSource thumb = ResourceHelper.GetFileThumbnail(filePath, allowIcon);
		PngBitmapEncoder encoder = new();
		MemoryStream memoryStream = new();
		encoder.Frames.Add(BitmapFrame.Create(thumb));
		encoder.Save(memoryStream);
		ManagedStream managedStream = new(memoryStream);
		string headers = "image/png";
		args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(managedStream, 200, "OK", headers);
	}
}
