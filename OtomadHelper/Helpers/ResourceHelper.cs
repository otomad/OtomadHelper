using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OtomadHelper.Helpers {
	internal static class ResourceHelper {
		/// <summary>
		/// 获取嵌入的资源。
		/// </summary>
		/// <param name="filePath">文件相对于项目根目录的路径，使用“.”（点）分隔目录。</param>
		/// <returns>资源的字节序列。</returns>
		public static Stream GetEmbeddedResource(string filePath) {
			Assembly assembly = Assembly.GetExecutingAssembly();
			string assetsFilePath = assembly.GetName().Name + "." + filePath; // 你可以设置断点看看这里的值。
			if (assembly.GetManifestResourceInfo(assetsFilePath) == null)
				throw new FileNotFoundException("Cannot find embedded resource: " + assetsFilePath);
			return assembly.GetManifestResourceStream(assetsFilePath);
		}
	}
}
