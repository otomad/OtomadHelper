const audioContext = new AudioContext();

type OscillatorCommonType = Exclude<OscillatorType, "custom">;

/**
 * Plays a beep sound using the Web Audio API.
 *
 * This function creates an oscillator node with the specified type, frequency, and duration.
 * The oscillator is then connected to the audio destination, started, and stopped after the specified duration.
 *
 * @param type - The type of oscillator to use. Must be one of the following: 'sine', 'square', 'sawtooth', 'triangle'.
 * @param hz - The frequency of the oscillator in Hz.
 * @param ms - The duration of the beep in milliseconds.
 * @param vol - The volume of the beep, represented as a value between 0 (silent) and 1 (full volume).
 *
 * @returns A promise that resolves when the beep is finished playing.
 */
export async function beep(type: OscillatorCommonType, hz: number, ms: number, vol: number) {
	const oscillator = audioContext.createOscillator();
	oscillator.type = type;
	oscillator.frequency.setValueAtTime(hz, audioContext.currentTime);

	const volume = audioContext.createGain();
	volume.gain.value = vol;
	oscillator.connect(volume);
	volume.connect(audioContext.destination);

	oscillator.start();
	await delay(ms);
	oscillator.stop();

	oscillator.disconnect();
	volume.disconnect();
}

export interface Pitch {
	noteName: string;
	octave: number;
	noteNumber: number;
	frequency: number;
}

export function pitchMap(spn: string) {
	const groups = spn?.match(/(?<noteName>[A-G][#♯b♭]?)(?<octave>\d+)/i)?.groups as undefined ?? { noteName: "", octave: "" };
	const octave = +groups.octave;
	let noteName = groups.noteName
		.toUpperCase()
		.replaceAll("♯", "#")
		.replace(/(?<=[A-G])[b♭]/i, "b");
	if (noteName.endsWith("b"))
		noteName = {
			Db: "C#",
			Eb: "D#",
			Gb: "F#",
			Ab: "G#",
			Bb: "A#",
		}[noteName] ?? noteName;

	function pitchMap(noteName: string, octave: number) {
		const noteNames = "C,C#,D,D#,E,F,F#,G,G#,A,A#,B".split(",");
		const noteNameIndex = noteNames.indexOf(noteName);
		if (noteNameIndex === -1) return null;
		const noteNumber = noteNameIndex + octave * 12;
		return noteNumber;
	}

	const centerC5 = pitchMap("C", 5)!, centerA4 = pitchMap("A", 4)!;
	const noteNumber = pitchMap(noteName, octave) ?? centerC5;
	const frequency = 440 * 2 ** ((noteNumber - centerA4) / 12);

	return { noteName, octave, noteNumber, frequency } as Pitch;
}
