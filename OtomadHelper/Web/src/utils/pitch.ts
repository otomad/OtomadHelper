const keys = "C,C#,D,D#,E,F,F#,G,G#,A,A#,B".split(",");

export function midiNoteToSPN(midiNoteNumber: number) {
	return keys[midiNoteNumber % 12] + (midiNoteNumber / 12 | 0);
}
