using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using OtomadHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Diagnostics;
using System.Web;
using System.Collections.Specialized;

namespace OtomadHelper.Module;
/// <summary>
/// Reading of response content stream happens asynchronously, and WebView2 does not 
/// directly dispose the stream once it read. Therefore, use the following stream
/// class, which properly disposes when WebView2 has read all data.For details, see
/// <a href="https://github.com/MicrosoftEdge/WebView2Feedback/issues/2513">
/// CoreWebView2 does not close stream content</a>.
/// </summary>
internal class ManagedStream : Stream {
	public ManagedStream(Stream s) {
		this.s = s;
	}

	public override bool CanRead => s.CanRead;

	public override bool CanSeek => s.CanSeek;

	public override bool CanWrite => s.CanWrite;

	public override long Length => s.Length;

	public override long Position { get => s.Position; set => s.Position = value; }

	public override void Flush() {
		throw new NotImplementedException();
	}

	public override long Seek(long offset, SeekOrigin origin) {
		return s.Seek(offset, origin);
	}

	public override void SetLength(long value) {
		throw new NotImplementedException();
	}

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

	public override void Write(byte[] buffer, int offset, int count) {
		throw new NotImplementedException();
	}

	private readonly Stream s;

	internal static void Handler(WebView2 webView) {
		const string HOST = "https://app/*";
		webView.CoreWebView2.AddWebResourceRequestedFilter(HOST, CoreWebView2WebResourceContext.All);
		webView.CoreWebView2.WebResourceRequested += (sender, args) => {
			Uri uri = new(args.Request.Uri);
			NameValueCollection query = HttpUtility.ParseQueryString(uri.Query); // 暂未使用
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
				Dictionary<string, string> contentTypes = new() {
					{ "html", "text/html" },
					{ "js", "text/javascript" },
					{ "css", "text/css" },
					{ "appcache", "text/cache-manifest" },
					{ "jpg", "image/jpeg" },
					{ "png", "image/png" },
					{ "gif", "image/gif" },
					{ "svg", "image/svg+xml" },
					{ "webp", "image/webp" },
					{ "apng", "image/apng" },
					{ "ico", "image/x-icon" },
					{ "cur", "image/vnd.microsoft.icon" },
					{ "bmp", "image/bmp" },
					{ "woff", "font/woff" },
					{ "woff2", "font/woff2" },
					{ "ttf", "font/ttf" },
					{ "json", "application/json" },
					{ "manifest", "application/manifest+json" },
					{ "ani", "application/x-navi-animation" },
				};
				string headers = "application/octet-stream";
				foreach (KeyValuePair<string, string> item in contentTypes)
					if (assetsFilePath.EndsWith("." + item.Key)) {
						headers = item.Value;
						break;
					}
				const int AGE = 1200;
				headers = $"""
					HTTP/1.1 200 OK
					Content-Type: {headers}
					Cache-Control: max-age={AGE}
					Age: {AGE}
					Keep-Alive: timeout={AGE}
					Date: {DateTime.UtcNow.ToUniversalTime():R}
					""";
				args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(managedStream, 200, "OK", headers);
			} catch (InvalidOperationException e) {
				args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(null, 415, e.Message, "");
			} catch (Exception) {
				args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(null, 404, "Not Found", "");
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
