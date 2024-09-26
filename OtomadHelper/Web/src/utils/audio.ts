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
export function beep(type: OscillatorCommonType, hz: number, ms: number, vol: number) {
	const audioContext = new AudioContext();
	const oscillator = audioContext.createOscillator();
	oscillator.type = type;
	oscillator.frequency.setValueAtTime(hz, audioContext.currentTime);

	const volume = audioContext.createGain();
	volume.gain.value = vol;
	oscillator.connect(volume);
	volume.connect(audioContext.destination);

	oscillator.start();
	const stop = () => oscillator.stop();
	const promise = delay(ms).then(() => {
		stop();
		return audioContext.close();
	});

	return { stop, promise };
}
