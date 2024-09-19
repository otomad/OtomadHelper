/// <reference path="./midi.d.ts" />

import _Midi from "@tonejs/midi";
import { readFile } from "fs/promises";
import type { Plugin } from "vite";

const { Midi } = _Midi;
const midiExt = /\.midi?($|\?.*$)/i;
const midiKeyframesExt = /\.midi?\?keyframes$/i;
const getPath = (id: string) => midiExt.test(id) ? id.replace(/\?.*/, "") : false;

const TICK_GAP = 1;

export default (): Plugin => {
	return {
		name: "vite-plugin-midi",
		enforce: "pre",

		async transform(_src, id) {
			const filePath = getPath(id);
			if (!filePath) return;
			const isMidiKeyframes = midiKeyframesExt.test(id);

			const midiData = await readFile(filePath);
			const midi = new Midi(midiData);
			if (isMidiKeyframes) {
				const resultNotes: Record<string, { start: number; end: number }[]> = {};
				let maxTicks = 0, maxSeconds = 0;
				midi.tracks.forEach((track, i) => {
					if (!track.name) {
						const prevTrack = midi.tracks[i - 1];
						if (prevTrack?.name) track.name = prevTrack.name;
						else return;
					}
					if (track.notes.length === 0) return;
					const notes: { start: number; end: number }[] = [];
					for (const note of track.notes) {
						const prevNote = notes.at(-1);
						if (prevNote) {
							if (note.ticks === prevNote.start) continue;
							if (note.ticks <= prevNote.end) prevNote.end = note.ticks - TICK_GAP;
						}
						const end = note.ticks + note.durationTicks, endSeconds = note.time + note.duration;
						notes.push({ start: note.ticks, end });
						if (end > maxTicks) maxTicks = end;
						if (endSeconds > maxSeconds) maxSeconds = endSeconds;
					}
					resultNotes[track.name] = notes;
				});
				const resultKeyframes: typeof import("*.mid?keyframes").default.tracks = {};
				for (const [name, notes] of Object.entries(resultNotes)) {
					resultKeyframes[name] = [[], [], [], []];
					const result = resultKeyframes[name];
					let flipped = false;
					for (const { start, end } of notes) {
						const startPercent = start / maxTicks * 100, endPercent = end / maxTicks * 100;
						const i = !flipped ? 0 : 2;
						result[i].push(startPercent);
						result[i + 1].push(endPercent);
						flipped = !flipped;
					}
				}
				return "export default " + JSON.stringify({
					tracks: resultKeyframes,
					length: maxSeconds,
				});
			}

			return "export default " + JSON.stringify(midi);
		},
	};
};
