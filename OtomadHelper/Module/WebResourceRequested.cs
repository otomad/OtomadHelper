using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using OtomadHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OtomadHelper.Module {
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
			webView.CoreWebView2.AddWebResourceRequestedFilter("https://app/*", CoreWebView2WebResourceContext.All);
			webView.CoreWebView2.WebResourceRequested += delegate (object sender, CoreWebView2WebResourceRequestedEventArgs args) {
				string file = args.Request.Uri.Substring("https://app/*".Length - 1);
				string assetsFilePath = "Web.dist." + file.Replace("/", ".");
				try {
					Stream fileStream = ResourceHelper.GetEmbeddedResource(assetsFilePath);
					ManagedStream managedStream = new ManagedStream(fileStream);
					Dictionary<string, string> contentTypes = new Dictionary<string, string> {
						{ "html", "text/html" },
						{ "js", "application/javascript" },
						{ "css", "text/css" },
						{ "jpg", "image/jpeg" },
						{ "png", "image/png" },
						{ "svg", "image/svg+xml" },
						{ "manifest", "text/cache-manifest" },
					};
					string headers = "application/octet-stream";
					foreach (KeyValuePair<string, string> item in contentTypes)
						if (assetsFilePath.EndsWith("." + item.Key)) {
							headers = "Content-Type: " + item.Value;
							break;
						}
					args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(managedStream, 200, "OK", headers);
				} catch (Exception) {
					args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(null, 404, "Not found", "");
				}
			};
		}
	}
}
