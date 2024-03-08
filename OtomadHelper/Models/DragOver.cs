using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtomadHelper.Models;
public class DragOver : BaseWebMessageEvent {
	public string Extension { get; set; } = "";
	public string ContentType { get; set; } = "";
	public bool IsDirectory { get; set; } = false;
	public bool IsDragging { get; set; } = false;
}
