using CefSharp;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace OtomadHelper.Telemetry {
	public class ResourceSchemeHandler : ResourceHandler {
		public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback) {
			string[] names = GetType().Assembly.GetManifestResourceNames();

			Console.WriteLine(names);

			Uri u = new Uri(request.Url);
			string file = u.Authority + u.AbsolutePath; // 注：目录名需全为小写字母，否则将无法得到 Resource

			Assembly ass = Assembly.GetExecutingAssembly();
			string resourcePath = ass.GetName().Name + "." + file.Replace("/", "."); // 你可以设置断点看看这里的值

			Task.Run(() => {
				using (callback) {
					if (ass.GetManifestResourceInfo(resourcePath) != null) {
						Stream stream = ass.GetManifestResourceStream(resourcePath);
						string mimeType = "application/octet-stream";
						switch (Path.GetExtension(file)) {
							case ".html":
								mimeType = "text/html";
								break;
							case ".js":
								mimeType = "text/javascript";
								break;
							case ".css":
								mimeType = "text/css";
								break;
							case ".jpg":
								mimeType = "image/jpeg";
								break;
							case ".png":
								mimeType = "image/png";
								break;
							case ".svg":
								mimeType = "image/svg+xml";
								break;
							case ".appcache":
								break;
							case ".manifest":
								mimeType = "text/cache-manifest";
								break;
						}

						// Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
						stream.Position = 0;
						// Populate the response values - No longer need to implement GetResponseHeaders (unless you need to perform a redirect)
						ResponseLength = stream.Length;
						MimeType = mimeType;
						StatusCode = (int)HttpStatusCode.OK;
						Stream = stream;

						callback.Continue();
					} else {
						callback.Cancel();
					}
				}
			});

			return CefReturnValue.Continue;
		}
	}
}
