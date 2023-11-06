﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack;
using System.Drawing;

namespace OtomadHelper.Helpers {
	internal static class ResourceHelper {
		/// <summary>
		/// 获取嵌入的资源。
		/// </summary>
		/// <param name="resourcePath">嵌入的资源文件相对于项目根目录的路径，使用“.”（点）分隔目录。</param>
		/// <returns>资源的字节序列。</returns>
		public static Stream GetEmbeddedResource(string resourcePath) {
			Assembly assembly = Assembly.GetExecutingAssembly();
			string assetsFilePath = assembly.GetName().Name + "." + resourcePath; // 你可以设置断点看看这里的值。
			if (assembly.GetManifestResourceInfo(assetsFilePath) == null)
				throw new FileNotFoundException("Cannot find embedded resource: " + assetsFilePath);
			return assembly.GetManifestResourceStream(assetsFilePath);
		}

		/// <summary>
		/// 获取本地文件的缩略图。
		/// </summary>
		/// <param name="filePath">本地文件路径。</param>
		/// <returns>文件的大缩略图。</returns>
		public static Bitmap GetFileThumbnail(string filePath) {
			ShellFile shellFile = ShellFile.FromFilePath(filePath);
			Bitmap thumb = shellFile.Thumbnail.ExtraLargeBitmap;
			return thumb;
		}

		/// <summary>
		/// 将嵌入的资源文件写入到本地磁盘的指定路径中。
		/// </summary>
		/// <param name="resourcePath">嵌入的资源文件路径。</param>
		/// <param name="filePath">本地文件路径。</param>
		public static void WriteResourceToFile(string resourcePath, string filePath) {
			using (Stream resource = GetEmbeddedResource(resourcePath)) {
				using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write)) {
					resource.CopyTo(file);
				}
			}
		}
	}
}