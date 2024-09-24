export default class Pitch {
	static readonly #noteNames = "C,C#,D,D#,E,F,F#,G,G#,A,A#,B".split(",");
	static readonly #centerC5 = Pitch.pitchMap("C", 5)!;
	static readonly #centerA4 = Pitch.pitchMap("A", 4)!;

	#noteNumber: number = Pitch.#centerC5;

	constructor(spn: string);
	constructor(noteName: string, octave: number);
	constructor(noteNumber: number);
	constructor(...args: unknown[]) {
		if (args.length === 2)
			this.noteNameAndOctave = { noteName: args[0] as string, octave: args[1] as number };
		else
			if (typeof args[0] === "string")
				this.spn = args[0];
			else if (typeof args[0] === "number")
				this.noteNumber = args[0];
	}

	static parseSpn(spn: string) {
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
		return { noteName, octave };
	}

	static pitchMap(noteName: string, octave: number) {
		const noteNameIndex = Pitch.#noteNames.indexOf(noteName);
		if (noteNameIndex === -1) return null;
		const noteNumber = noteNameIndex + octave * 12;
		return noteNumber;
	}

	private set noteNameAndOctave(value: ReturnType<typeof Pitch.parseSpn>) {
		const noteNumber = Pitch.pitchMap(value.noteName, value.octave);
		if (noteNumber != null) this.#noteNumber = noteNumber;
	}

	get spn() {
		return this.noteName + this.octave;
	}

	set spn(spn) {
		this.noteNameAndOctave = Pitch.parseSpn(spn);
	}

	get noteName() {
		return Pitch.#noteNames[this.#noteNumber % 12];
	}

	set noteName(noteName) {
		this.noteNameAndOctave = { noteName, octave: this.octave };
	}

	get octave() {
		return this.#noteNumber / 12 | 0;
	}

	set octave(octave) {
		this.noteNameAndOctave = { noteName: this.noteName, octave };
	}

	get noteNumber() {
		return this.#noteNumber;
	}

	set noteNumber(value) {
		this.#noteNumber = value;
	}

	get frequency() {
		return 440 * 2 ** ((this.#noteNumber - Pitch.#centerA4) / 12);
	}
}
