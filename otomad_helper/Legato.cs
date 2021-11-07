#if !UNUSUAL

using System.Collections.Generic;
using ScriptPortal.Vegas;

namespace Otomad.VegasScripts {
	public class EntryPoint {
		public Track[] GetSelectedTracks() {
			List<Track> tracks = new List<Track>();
			foreach (Track track in vegas.Project.Tracks)
				if (track.Selected)
					tracks.Add(track);
			return tracks.ToArray();
		}
		public static TrackEvent RelativeAdjustPlaybackRate(TrackEvent trackEvent, double rate, bool forceStretch = false) {
			double originalRate = trackEvent.PlaybackRate;
			double expectRate = rate * originalRate;
			trackEvent.AdjustPlaybackRate(expectRate, true);
			double actualRate = trackEvent.PlaybackRate;
			trackEvent.Length = Timecode.FromMilliseconds(trackEvent.Length.ToMilliseconds() / (forceStretch ? expectRate : actualRate) * originalRate);
			return trackEvent;
		}
		public Vegas vegas;
		public void FromVegas(Vegas myVegas) {
			vegas = myVegas;
			foreach (Track track in GetSelectedTracks()) {
				/* for (int i = 1; i < track.Events.Count; i++) {
					TrackEvent currentEvent = track.Events[i];
					TrackEvent previousEvent = track.Events[i - 1];
					currentEvent.Start = previousEvent.End;
				} */
				for (int i = 0; i < track.Events.Count - 1; i++) {
					TrackEvent currentEvent = track.Events[i];
					TrackEvent nextEvent = track.Events[i + 1];
					double rate = currentEvent.Length.ToMilliseconds() / (nextEvent.Start - currentEvent.Start).ToMilliseconds();
					RelativeAdjustPlaybackRate(currentEvent, rate, true);
				}
			}
		}
	}
}

#else

public class EntryPoint{public void FromVegas(ScriptPortal.Vegas.Vegas v){foreach(var t in v.Project.Tracks)if(t.Selected)for(int i=1;i<t.Events.Count;i++)t.Events[i].Start=t.Events[i-1].End;}}

#endif