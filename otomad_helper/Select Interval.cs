using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using ScriptPortal.Vegas;

public class EntryPoint {
	public void FromVegas(Vegas vegas) {
		Project project = vegas.Project;
		foreach (Track track in project.Tracks) {
			bool select = true; // 默认选择单数，若欲为双数则改为 false
			foreach (TrackEvent trackEvent in track.Events) {
				if (trackEvent.Selected) {
					trackEvent.Selected = select;
					select = !select;
				}
			}
		}
	}
}