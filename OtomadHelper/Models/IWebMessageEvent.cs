using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtomadHelper.Models;
public abstract class BaseWebMessageEvent {
	public string Type {
		get {
			string type = GetType().Name;
			return char.ToLowerInvariant(type[0]) + type.Substring(1);
		}
	}
}
