using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace OtomadHelper.Telemetry {
	internal class ResourceSchemeHandlerFactory : ISchemeHandlerFactory {
		public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request) {
			return new ResourceSchemeHandler();
		}

		public static string SchemeName {
			get {
				return "resource"; // 这里我设置的 SchemeName 为 nacollector，当然你也可以改成其他的
			}
		}
	}
}
